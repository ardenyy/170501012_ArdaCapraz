using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Reisebuero.Models
{
    public class LoginForm : BaseModel
    {
        [NotMapped]
        public override int ID
        {
            get { return EmployeeID; }
            set { EmployeeID = SetProperty(value, EmployeeID); }
        }
        [Key]
        [ForeignKey("Employee")]
        protected int EmployeeID
        {
            get { return Employee.ID; }
            set { Employee.ID = value; }
        }
        public Employee Employee { get; set; }
        public string Password { get; set; }
    }
}
