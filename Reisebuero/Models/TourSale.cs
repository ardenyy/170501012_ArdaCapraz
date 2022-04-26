using System;

namespace Reisebuero.Models
{
    public class TourSale : BaseModel
    {
        public Tour Tour { get; set; }
        public Customer Customer { get; set; }
        public Employee Employee { get; set; }
        public DateTime Date { get; set; }
    }
}
