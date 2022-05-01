using System.ComponentModel.DataAnnotations.Schema;

namespace Reisebuero.Models
{
    public class LoginForm : BaseModel
    {       
        [ForeignKey("ID")]
        public Employee Employee { get; set; }
        public string Password { get; set; }
    }
}
