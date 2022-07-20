using System;

namespace Reisebuero.Models
{
    public class TourSale : BaseModel
    {
        public int TourID { get; set; }
        public Tour Tour { get; set; }

        public int CustomerID { get; set; }
        public Customer Customer { get; set; }

        public int EmployeeID { get; set; }
        public Employee Employee { get; set; }

        public DateTime Date { get; set; }
    }
}
