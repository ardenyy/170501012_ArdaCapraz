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
        public List<TourSale> TourSales { get; set; } = new List<TourSale>();

        public Tour() : base() { }

        public bool CheckObjectData()
        {
            if (Name == null
                || Capacity < 0
                || Price < 0.0
                || StartDate >= EndDate
                || EndDate < DateTime.Now)
            {
                return false;
            }
            return true;
        }
    }
}
