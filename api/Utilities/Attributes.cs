using System.ComponentModel.DataAnnotations;
using System.Reflection.Metadata.Ecma335;

namespace api.Utilities
{
    [AttributeUsage(AttributeTargets.Property | AttributeTargets.Field, AllowMultiple = false)]
    public class CPFAttribute : ValidationAttribute
    {
        public bool AllowPunctuation { get; set; } = true;

        protected override ValidationResult? IsValid(
            object? value,
            ValidationContext validationContext
        )
        {
            if (value == null)
            {
                return ValidationResult.Success;
            }

            if (value is not string)
            {
                return new ValidationResult(
                    $"{validationContext.DisplayName} deve ser uma string!"
                );
            }

            string cpf = value.ToString() ?? "";

            // Remover pontuação
            if (AllowPunctuation)
            {
                cpf = cpf.Replace(".", "").Replace("-", "");
            }
            else if (cpf.Contains('.') || cpf.Contains('-'))
            {
                return new ValidationResult(
                    $"{validationContext.DisplayName} não deve conter pontuação!"
                );
            }

            if (!Validators.IsValidCPF(cpf))
            {
                return new ValidationResult($"{validationContext.DisplayName} inválido!");
            }

            return ValidationResult.Success;
        }
    }

    [AttributeUsage(AttributeTargets.Property | AttributeTargets.Field, AllowMultiple = false)]
    public class GuidAttribute : ValidationAttribute
    {
        protected override ValidationResult? IsValid(
            object? value,
            ValidationContext validationContext
        )
        {
            if (value == null)
            {
                return ValidationResult.Success;
            }

            if (value is string str && Guid.TryParse(str, out _))
            {
                return ValidationResult.Success;
            }

            if (value is Guid)
            {
                return ValidationResult.Success;
            }

            return new ValidationResult(
                $"O campo {validationContext.DisplayName} deve ser um GUID válido!"
            );
        }
    }

    [AttributeUsage(AttributeTargets.Property | AttributeTargets.Field, AllowMultiple = false)]
    public class BirthDateAttribute : ValidationAttribute
    {
        public int MinimumAge { get; set; } = 0;
        public int MaximumAge { get; set; } = -1;

        protected override ValidationResult? IsValid(
            object? value,
            ValidationContext validationContext
        )
        {
            if (value == null)
            {
                return ValidationResult.Success;
            }

            if (value is DateOnly date)
            {
                var today = DateOnly.FromDateTime(DateTime.Now);
                var age = today.Year - date.Year;

                if (date > today.AddYears(-age))
                {
                    age--;
                }

                if (age < MinimumAge)
                {
                    return new ValidationResult(
                        $"A idade mínima para {validationContext.DisplayName} é {MinimumAge} anos."
                    );
                }

                if (MaximumAge > 0 && age > MaximumAge)
                {
                    return new ValidationResult(
                        $"A idade máxima para {validationContext.DisplayName} é {MaximumAge} anos."
                    );
                }

                return ValidationResult.Success;
            }

            return new ValidationResult(
                $"{validationContext.DisplayName} deve ser do tipo DateOnly."
            );
        }
    }

    [AttributeUsage(AttributeTargets.Property | AttributeTargets.Field, AllowMultiple = false)]
    public class DateTimeAttribute : ValidationAttribute
    {
        public bool AllowPast { get; set; } = true;
        public bool AllowFuture { get; set; } = true;

        protected override ValidationResult? IsValid(
            object? value,
            ValidationContext validationContext
        )
        {
            if (value == null)
            {
                return ValidationResult.Success;
            }
            if (value is DateTime date)
            {
                var today = DateTime.Now;
                if (!AllowPast && date < today)
                {
                    return new ValidationResult(
                        $"{validationContext.DisplayName} não pode ser uma data passada."
                    );
                }
                if (!AllowFuture && date > today)
                {
                    return new ValidationResult(
                        $"{validationContext.DisplayName} não pode ser uma data futura."
                    );
                }
                return ValidationResult.Success;
            }

            return new ValidationResult(
                $"{validationContext.DisplayName} deve ser do tipo DateTime."
            );
        }
    }
}
