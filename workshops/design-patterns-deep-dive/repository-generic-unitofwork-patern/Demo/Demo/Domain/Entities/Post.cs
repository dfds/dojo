using Demo.Domain.Common;

namespace Demo.Domain.Entities
{
    public class Post:BaseEntity
    {
        public string Title { get; set; }
        public string MetaTitle { get; set; }
        public string Summary { get; set; }
        public string Body { get; set; }
    }
}