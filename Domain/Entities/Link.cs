using System.ComponentModel.DataAnnotations.Schema;

namespace Domain.Entities
{
    public class Link
    {
        public string Self { get; set; }

        [ForeignKey("OnlineOrder")]
        public long OrderId { get; set; }

        public virtual OnlineOrder OnlineOrder { get; set; }
    }
}
