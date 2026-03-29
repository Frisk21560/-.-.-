namespace Dapper3.Models
{
    public class Dog
    {
        public int Id { get; set; }
        public string Name { get; set; } = null!;
        public int Age { get; set; }
        public string Breed { get; set; } = null!;
        public bool IsAdopted { get; set; } = false;
        public int? AdopterId { get; set; }

        public Adopter Adopter { get; set; } = null!;

        public override string ToString()
        {
            string status = IsAdopted ? $"✓ Забрана ({Adopter?.FullName})" : "✗ У притулку";
            return $"ID: {Id} | Ім'я: {Name} | Вік: {Age} років | Порода: {Breed} | Статус: {status}";
        }
    }
}