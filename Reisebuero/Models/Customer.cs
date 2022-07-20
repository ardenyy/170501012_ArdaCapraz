using System;
using System.Collections.Generic;

namespace Reisebuero.Models
{
    public class Customer : BaseModel
    {
        public string Name { get; set; }
        public string Surname { get; set; }
        public string Email { get; set; }
        public DateTime BirthDate { get; set; }
        public List<TourSale> TourSales { get; set; } = new List<TourSale>();

        public bool CheckObjectData()
        {
            if (Name == null
                || Surname == null
                || Email == null
                || BirthDate >= DateTime.Now)
            {
                return false;
            }
            return true;
        }
    }
}
