using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Kiwify.Core.Data.Entities
{
    public class BaseEntity
    {
        [Column("id")]
        [Key]
        public long Id { get; set; }
    }
}
