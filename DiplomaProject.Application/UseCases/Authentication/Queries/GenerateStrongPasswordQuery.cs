using DiplomaProject.Domain.Services.External;
using System.Security.Cryptography;
using System.Text;

namespace DiplomaProject.Application.UseCases.Authentication.Queries;

public class GenerateStrongPasswordQuery : BaseQuery<string>
{
    public class GenerateStrongPasswordQueryHandler(IMapper mapper, ICurrentUser currentUser)
        : BaseQueryHandler<GenerateStrongPasswordQuery>(mapper, currentUser)
    {
        public override Task<string> Handle(GenerateStrongPasswordQuery request, CancellationToken cancellationToken)
        {
            var isStrongPassword = false;
            var password = string.Empty;
            while (!isStrongPassword)
            {
                password = GenerateStrongPassword();
                isStrongPassword = IsStrongPassword(password);
            }

            return Task.FromResult(password);
        }

        private static bool IsStrongPassword(string password)
        {
            var hasUpperCase = false;
            var hasLowerCase = false;
            var hasDigit = false;
            var hasSpecialCharacter = false;

            foreach (var character in password)
            {
                if (char.IsUpper(character))
                {
                    hasUpperCase = true;
                }
                else if (char.IsLower(character))
                {
                    hasLowerCase = true;
                }
                else if (char.IsDigit(character))
                {
                    hasDigit = true;
                }
                else if (char.IsPunctuation(character) || char.IsSymbol(character))
                {
                    hasSpecialCharacter = true;
                }
            }

            return hasUpperCase && hasLowerCase && hasDigit && hasSpecialCharacter;
        }

        private static string GenerateStrongPassword()
        {
            var builder = new StringBuilder();

            for (var i = 0; i < 10; i++)
            {
                var character = (char)RandomNumberGenerator.GetInt32(33, 126);
                builder.Append(character);
            }

            return builder.ToString();
        }
    }
}