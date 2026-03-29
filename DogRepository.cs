using Dapper;
using Dapper3.Models;
using System.Data.SqlClient;

namespace Dapper3.DataAccess
{
    public class DogRepository
    {
        private readonly string _connectionString;

        public DogRepository(string connectionString)
        {
            _connectionString = connectionString;
        }

        public void InitTables()
        {
            using SqlConnection con = new SqlConnection(_connectionString);
            con.Open();

            string createAdopters = @"
            IF OBJECT_ID(N'dbo.Adopters', N'U') IS NULL
            BEGIN
                CREATE TABLE Adopters (
                    Id INT PRIMARY KEY IDENTITY(1,1),
                    FullName NVARCHAR(MAX) NOT NULL,
                    PhoneNumber NVARCHAR(20) NOT NULL
                );
            END
            ";

            string createDogs = @"
            IF OBJECT_ID(N'dbo.Dogs', N'U') IS NULL
            BEGIN
                CREATE TABLE Dogs (
                    Id INT PRIMARY KEY IDENTITY(1,1),
                    Name NVARCHAR(100) NOT NULL,
                    Age INT NOT NULL,
                    Breed NVARCHAR(100) NOT NULL,
                    IsAdopted BIT DEFAULT 0,
                    AdopterId INT NULL,
                    FOREIGN KEY (AdopterId) REFERENCES Adopters(Id)
                );
            END
            ";

            con.Execute(createAdopters);
            con.Execute(createDogs);
        }

        // ========== СОБАКИ ==========
        public void AddDog(Dog dog)
        {
            using SqlConnection con = new SqlConnection(_connectionString);
            con.Open();

            string query = @"
                INSERT INTO Dogs (Name, Age, Breed, IsAdopted, AdopterId)
                VALUES (@Name, @Age, @Breed, @IsAdopted, @AdopterId)
            ";

            con.Execute(query, dog);
        }

        public List<Dog> GetAllDogs()
        {
            using SqlConnection con = new SqlConnection(_connectionString);
            con.Open();

            string query = @"
                SELECT D.Id, D.Name, D.Age, D.Breed, D.IsAdopted, D.AdopterId, 
                       A.Id, A.FullName, A.PhoneNumber
                FROM Dogs D
                LEFT JOIN Adopters A ON D.AdopterId = A.Id
                ORDER BY D.Name
            ";

            var dogsMap = new Dictionary<int, Dog>();

            var result = con.Query<Dog, Adopter, Dog>(query,
                (d, a) =>
                {
                    if (!dogsMap.TryGetValue(d.Id, out Dog dog))
                    {
                        dog = d;
                        dogsMap.Add(d.Id, dog);
                    }

                    if (a != null)
                    {
                        dog.Adopter = a;
                    }

                    return dog;
                },
                splitOn: "Id"
            ).Distinct();

            return result.ToList();
        }

        public Dog GetDogById(int id)
        {
            using SqlConnection con = new SqlConnection(_connectionString);
            con.Open();

            string query = @"
                SELECT D.Id, D.Name, D.Age, D.Breed, D.IsAdopted, D.AdopterId, 
                       A.Id, A.FullName, A.PhoneNumber
                FROM Dogs D
                LEFT JOIN Adopters A ON D.AdopterId = A.Id
                WHERE D.Id = @Id
            ";

            return con.Query<Dog, Adopter, Dog>(query,
                (d, a) =>
                {
                    d.Adopter = a;
                    return d;
                },
                new { Id = id },
                splitOn: "Id"
            ).FirstOrDefault();
        }

        public List<Dog> GetAvailableDogs()
        {
            using SqlConnection con = new SqlConnection(_connectionString);
            con.Open();

            string query = @"
                SELECT D.Id, D.Name, D.Age, D.Breed, D.IsAdopted, D.AdopterId
                FROM Dogs D
                WHERE D.IsAdopted = 0
                ORDER BY D.Name
            ";

            return con.Query<Dog>(query).ToList();
        }

        public List<Dog> GetAdoptedDogs()
        {
            using SqlConnection con = new SqlConnection(_connectionString);
            con.Open();

            string query = @"
                SELECT D.Id, D.Name, D.Age, D.Breed, D.IsAdopted, D.AdopterId, 
                       A.Id, A.FullName, A.PhoneNumber
                FROM Dogs D
                INNER JOIN Adopters A ON D.AdopterId = A.Id
                WHERE D.IsAdopted = 1
                ORDER BY A.FullName
            ";

            var dogsMap = new Dictionary<int, Dog>();

            var result = con.Query<Dog, Adopter, Dog>(query,
                (d, a) =>
                {
                    if (!dogsMap.TryGetValue(d.Id, out Dog dog))
                    {
                        dog = d;
                        dogsMap.Add(d.Id, dog);
                    }

                    if (a != null)
                    {
                        dog.Adopter = a;
                    }

                    return dog;
                },
                splitOn: "Id"
            ).Distinct();

            return result.ToList();
        }

        public void DeleteDog(int id)
        {
            using SqlConnection con = new SqlConnection(_connectionString);
            con.Open();

            string query = "DELETE FROM Dogs WHERE Id = @Id";
            con.Execute(query, new { Id = id });
        }

        // ========== ОПІКУНИ ==========
        public void AddAdopter(Adopter adopter)
        {
            using SqlConnection con = new SqlConnection(_connectionString);
            con.Open();

            string query = @"
                INSERT INTO Adopters (FullName, PhoneNumber)
                VALUES (@FullName, @PhoneNumber)
            ";

            con.Execute(query, adopter);
        }

        public List<Adopter> GetAllAdopters()
        {
            using SqlConnection con = new SqlConnection(_connectionString);
            con.Open();

            string query = @"
                SELECT A.Id, A.FullName, A.PhoneNumber, D.Id, D.Name, D.Age, D.Breed, D.IsAdopted, D.AdopterId
                FROM Adopters A
                LEFT JOIN Dogs D ON A.Id = D.AdopterId
                ORDER BY A.FullName
            ";

            var adoptersMap = new Dictionary<int, Adopter>();

            var result = con.Query<Adopter, Dog, Adopter>(query,
                (a, d) =>
                {
                    if (!adoptersMap.TryGetValue(a.Id, out Adopter adopter))
                    {
                        adopter = a;
                        adopter.Dogs = new List<Dog>();
                        adoptersMap.Add(a.Id, adopter);
                    }

                    if (d != null)
                    {
                        adopter.Dogs.Add(d);
                    }

                    return adopter;
                },
                splitOn: "Id"
            ).Distinct();

            return result.ToList();
        }

        public void AdoptDog(int dogId, int adopterId)
        {
            using SqlConnection con = new SqlConnection(_connectionString);
            con.Open();

            string query = @"
                UPDATE Dogs
                SET IsAdopted = 1, AdopterId = @AdopterId
                WHERE Id = @DogId
            ";

            con.Execute(query, new { DogId = dogId, AdopterId = adopterId });
        }

        public List<Dog> SearchDogsByName(string name)
        {
            using SqlConnection con = new SqlConnection(_connectionString);
            con.Open();

            string query = @"
                SELECT D.Id, D.Name, D.Age, D.Breed, D.IsAdopted, D.AdopterId, 
                       A.Id, A.FullName, A.PhoneNumber
                FROM Dogs D
                LEFT JOIN Adopters A ON D.AdopterId = A.Id
                WHERE D.Name LIKE '%' + @Name + '%'
                ORDER BY D.Name
            ";

            return con.Query<Dog, Adopter, Dog>(query,
                (d, a) =>
                {
                    d.Adopter = a;
                    return d;
                },
                new { Name = name },
                splitOn: "Id"
            ).Distinct().ToList();
        }

        public List<Dog> SearchDogsByBreed(string breed)
        {
            using SqlConnection con = new SqlConnection(_connectionString);
            con.Open();

            string query = @"
                SELECT D.Id, D.Name, D.Age, D.Breed, D.IsAdopted, D.AdopterId, 
                       A.Id, A.FullName, A.PhoneNumber
                FROM Dogs D
                LEFT JOIN Adopters A ON D.AdopterId = A.Id
                WHERE D.Breed LIKE '%' + @Breed + '%'
                ORDER BY D.Breed
            ";

            return con.Query<Dog, Adopter, Dog>(query,
                (d, a) =>
                {
                    d.Adopter = a;
                    return d;
                },
                new { Breed = breed },
                splitOn: "Id"
            ).Distinct().ToList();
        }
    }
}