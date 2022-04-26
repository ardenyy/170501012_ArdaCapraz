using System.Collections.Generic;
using Reisebuero.Utilities;

namespace Reisebuero.Models
{
    public class Employee : BaseModel
    {
        public string Name { get; set; }
        public string Surname { get; set; }
        public Constants.Role Role { get; set; }
        public List<TourSale> SoldTours { get; set; }
    }
}
