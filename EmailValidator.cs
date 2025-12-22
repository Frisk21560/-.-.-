using System;

namespace Interfaces
{
    public class EmailValidator : IValidator
    {
        public string Email { get; set; } = null!;

        public EmailValidator(string email)
        {
            Email = email;
        }

        public bool Validate()
        {
            // simple rules: contain '@', contain '.', length > 5, and '.' after '@'
            if (string.IsNullOrWhiteSpace(Email))
            {
                Console.WriteLine("EmailValidator: email is empty");
                return false;
            }

            if (Email.Length <= 5)
            {
                Console.WriteLine("EmailValidator: email too short");
                return false;
            }

            int at = Email.IndexOf('@');
            int dot = Email.LastIndexOf('.');

            if (at <= 0 || dot <= at + 1 || dot == Email.Length - 1)
            {
                Console.WriteLine("EmailValidator: email not in good format");
                return false;
            }

            Console.WriteLine("EmailValidator: looks ok");
            return true;
        }
    }
}