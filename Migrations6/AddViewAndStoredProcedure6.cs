using Microsoft.EntityFrameworkCore.Migrations;

namespace EFConsole5.Migrations
{
    public partial class AddViewAndStoredProcedure6 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            // VIEW: Користувачі та фільми, які вони додали
            migrationBuilder.Sql(@"
                CREATE VIEW vw_UsersMovies AS
                SELECT 
                    u.Id AS UserId,
                    u.Username,
                    u.Email,
                    m.Id AS MovieId,
                    m.Title,
                    m.Description,
                    m.ReleaseYear,
                    m.AddedDate
                FROM Users u
                LEFT JOIN Movies m ON u.Id = m.UserId
                ORDER BY u.Username, m.Title;
            ");

            // STORED PROCEDURE: Додати користувача
            migrationBuilder.Sql(@"
                CREATE PROCEDURE sp_AddUser
                    @Username NVARCHAR(100),
                    @Email NVARCHAR(100)
                AS
                BEGIN
                    INSERT INTO Users (Username, Email, CreatedDate)
                    VALUES (@Username, @Email, GETDATE());
                    
                    SELECT @@IDENTITY AS UserId;
                END;
            ");

            // STORED PROCEDURE: Додати фільм
            migrationBuilder.Sql(@"
                CREATE PROCEDURE sp_AddMovie
                    @Title NVARCHAR(200),
                    @Description NVARCHAR(500),
                    @ReleaseYear INT,
                    @UserId INT
                AS
                BEGIN
                    INSERT INTO Movies (Title, Description, ReleaseYear, UserId, AddedDate)
                    VALUES (@Title, @Description, @ReleaseYear, @UserId, GETDATE());
                    
                    SELECT @@IDENTITY AS MovieId;
                END;
            ");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql("DROP VIEW IF EXISTS vw_UsersMovies;");
            migrationBuilder.Sql("DROP PROCEDURE IF EXISTS sp_AddUser;");
            migrationBuilder.Sql("DROP PROCEDURE IF EXISTS sp_AddMovie;");
        }
    }
}