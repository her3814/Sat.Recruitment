using System.Net.Mail;

namespace Sat.Recruitment.Helpers
{
    public static class EmailHelper
    {
        public static bool IsValidEmail(this string email)
        {
            if (email.EndsWith("."))
            {
                return false;
            }
            MailAddress addr;
            try
            {
                addr = new MailAddress(email);
            }
            catch(FormatException)
            {
                return false;
            };
            return addr.Address == email;
        }

        public static string NormalizeEmail(this string email)
        {
            var aux = email.Trim().Split(new char[] { '@' }, StringSplitOptions.RemoveEmptyEntries);

            var atIndex = aux[0].IndexOf("+", StringComparison.Ordinal);

            aux[0] = atIndex < 0 ? aux[0].Replace("+", "") : aux[0].Replace("+", "").Remove(atIndex);

            return string.Join("@", new string[] { aux[0], aux[1] });
        }
    }
}