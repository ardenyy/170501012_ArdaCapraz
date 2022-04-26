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
        public List<Tour> Tours { get; set; }
        public List<TourSale> PurchasedTours { get; set; }
    }
}
