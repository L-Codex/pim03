namespace api.Utilities
{
    public class Validators
    {
        public static bool IsValidCPF(string cpf)
        {
            // Source: https://www.geradorcpf.com/algoritmo_do_cpf.htm
            if (cpf.Length != 11 || !cpf.All(char.IsDigit))
            {
                return false;
            }

            // Verificar digitos iguais.
            if (new string(cpf[0], 11) == cpf)
            {
                return false;
            }

            int soma = 0;
            int mod = 0;

            // Calcular primeiro digito.
            for (int i = 0; i < 9; i++)
            {
                soma += int.Parse(cpf[i].ToString()) * (10 - i);
            }
            mod = soma % 11;
            int digito1 = mod < 2 ? 0 : 11 - mod;

            // Sair se o primeiro dígito verificador não for válido.
            if (digito1 != int.Parse(cpf[9].ToString()))
            {
                return false;
            }

            // Calcular segundo dígito.
            soma = 0;
            for (int i = 0; i < 10; i++)
            {
                soma += int.Parse(cpf[i].ToString()) * (11 - i);
            }
            mod = soma % 11;
            int digito2 = mod < 2 ? 0 : 11 - mod;

            return digito2 == int.Parse(cpf[10].ToString());
        }

        public static bool IsValidUUID(string uuid)
        {
            return Guid.TryParse(uuid, out _);
        }
    }
}
