using System.Collections.Generic;
using Reisebuero.Utilities;

namespace Reisebuero.Models
{
    public class Employee : BaseModel
    {
        public string Name { get; set; }
        public string Surname { get; set; }
        public Constants.Role Role { get; set; }
        public List<TourSale> TourSales { get; set; } = new List<TourSale>();

        public bool CheckObjectData()
        {
            if (Name == null
                || Surname == null
                || (Role != Constants.Role.Administrator
                    && Role != Constants.Role.Employee))
            {
                return false;
            }
            return true;
        }
    }
}
