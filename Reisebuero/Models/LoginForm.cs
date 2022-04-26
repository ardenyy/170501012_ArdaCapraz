using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Reisebuero.Models
{
    public class LoginForm : BaseModel
    {
        [NotMapped]
        public int ID
        {
            get { return Employee.ID; }
            set { Employee.ID = value; }
        }
        [Key]
        [ForeignKey("Employee")]
        public int EmployeeID
        {
            get { return ID; }
            set { ID = value; }
        }
        public Employee Employee { get; set; }
        public string Password { get; set; }
    }
}
