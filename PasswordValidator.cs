using System;
using System.Linq;

namespace Interfaces
{
    public class PasswordValidator : IValidator
    {
        public string Pass { get; set; } = null!;

        public PasswordValidator(string pass)
        {
            Pass = pass;
        }

        public bool Validate()
        {
            // simple rules:
            // at least 6 chars, contains digit, contains letter
            if (string.IsNullOrEmpty(Pass) || Pass.Length < 6)
            {
                Console.WriteLine("PasswordValidator: password too short (min 6)");
                return false;
            }

            bool hasDigit = Pass.Any(c => char.IsDigit(c));
            bool hasLetter = Pass.Any(c => char.IsLetter(c));

            if (!hasDigit)
            {
                Console.WriteLine("PasswordValidator: should contain at least one digit");
                return false;
            }

            if (!hasLetter)
            {
                Console.WriteLine("PasswordValidator: should contain at least one letter");
                return false;
            }

            Console.WriteLine("PasswordValidator: ok");
            return true;
        }
    }
}