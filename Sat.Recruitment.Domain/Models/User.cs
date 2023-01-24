namespace Sat.Recruitment.Domain.Models
{
    public class User
    {
        public string Name { get; set; }
        public string Email { get; set; }
        public string Address { get; set; }
        public string Phone { get; set; }
        public string UserType { get; set; }
        public decimal Money { get; set; }


        public override bool Equals(object objToCompare)
        {
            if (objToCompare is null)
            {
                return false;
            }
            if (objToCompare is not User)
            {
                return false;
            }
            User objAsUser = (User)objToCompare;
            return Email == objAsUser.Email || Phone == objAsUser.Phone || (Name == objAsUser.Name && Address == objAsUser.Address);
        }
    }
}