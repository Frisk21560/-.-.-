namespace DapperDemo.Models
{
    public class Dog
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public int Age { get; set; }
        public string Breed { get; set; }
        public bool IsAdopted { get; set; } = false;

        public override string ToString()
        {
            string status = IsAdopted ? "✓ Забрана" : "✗ У притулку";
            return $"ID: {Id} | Ім'я: {Name} | Вік: {Age} років | Порода: {Breed} | Статус: {status}";
        }
    }
}