namespace Dapper3.Models
{
    public class Adopter
    {
        public int Id { get; set; }
        public string FullName { get; set; } = null!;
        public string PhoneNumber { get; set; } = null!;

        public List<Dog> Dogs { get; set; } = new List<Dog>();

        public override string ToString()
        {
            return $"ID: {Id} | Ім'я: {FullName} | Телефон: {PhoneNumber} | Собак: {Dogs.Count}";
        }
    }
}