using Reisebuero.Utilities;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Reisebuero.Models
{
    public abstract class BaseModel : ObservableObject
    {
        [Key, DatabaseGenerated(DatabaseGeneratedOption.None)]
        [Required]
        public virtual int ID { get; set; }
    }
}
