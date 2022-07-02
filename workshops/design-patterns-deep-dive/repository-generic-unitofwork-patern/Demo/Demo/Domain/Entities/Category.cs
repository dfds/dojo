
using Demo.Domain.Common;

namespace Demo.Domain.Entities
{
    public class Category:BaseEntity
    {
        public string Title { get; set; }
        public string MetaTitle { get; set; }
        public virtual ICollection<Post> Posts { get; set; }
    }
}