using Sat.Recruitment.Helpers;
using System.ComponentModel.DataAnnotations;

namespace Sat.Recruitment.Domain.Models
{
    public class User //: IValidatableObject - The idea was to use IValidatable, but couldn't add a filter to handle ModelValidation errors. 
    {
        public string Name { get; set; }
        public string Email { get; set; }
        public string Address { get; set; }
        public string Phone { get; set; }
        public UserTypeEnum UserType { get; set; }
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

        public IEnumerable<ValidationResult> Validate()
        {
            if (string.IsNullOrEmpty(this.Name))
                //Validate if Name is null
                yield return new ValidationResult("The name is required");
            if (string.IsNullOrEmpty(this.Email))
                //Validate if Email is null
                yield return new ValidationResult("The email is required");
            else if (!this.Email.IsValidEmail())
                //Validate if Email is valid
                yield return new ValidationResult("The email is invalid");
            if (string.IsNullOrEmpty(this.Address))
                //Validate if Address is null
                yield return new ValidationResult("The address is required");
            if (string.IsNullOrEmpty(this.Phone))
                //Validate if Phone is null
                yield return new ValidationResult("The phone is required");
        }
    }
}