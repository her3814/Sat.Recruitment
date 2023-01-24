using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sat.Recruitment.Domain.Models
{
    public static class NewUserGiftPercentages
    {
        //I guess that, as a mistake, the original code sets an 80% GIFT instead of a 0.08/8% GIFT for those NormalUsers that are created with money between 10 and 100USD
        public const decimal NormalOver10 = 0.8m;
        public const decimal NormalOver100 = 0.12m;
        public const decimal SuperUser = 0.2m;
        public const decimal PremiumUser = 2m;

        public static decimal GetNewUserGiftPercentage(User user)
        {
            switch (user.UserType)
            {
                case "Normal":
                    if (user.Money > 100)
                        return NormalOver100;
                    else if (user.Money > 10)
                        return NormalOver10;
                    break;
                case "SuperUser":
                    if (user.Money > 100)
                        return SuperUser;
                    break;
                case "PremiumUser":
                    if (user.Money > 100)
                        return PremiumUser;
                    break;
                default:
                    break;
            }
            return 0;
        }
    }
}
