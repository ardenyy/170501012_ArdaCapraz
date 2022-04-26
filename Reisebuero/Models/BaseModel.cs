using Reisebuero.Utilities;

namespace Reisebuero.Models
{
    public abstract class BaseModel : ObservableObject
    {
        public virtual int ID { get; set; }
    }
}
