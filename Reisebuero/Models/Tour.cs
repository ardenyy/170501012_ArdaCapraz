using System;
using System.Collections.Generic;

namespace Reisebuero.Models
{
    public class Tour : BaseModel
    {
        public string Name { get; set; }
        public int Capacity { get; set; }
        public double Price { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public List<Customer> Customer { get; set; }
        public List<TourSale> TourSales { get; set; }
    }
}
