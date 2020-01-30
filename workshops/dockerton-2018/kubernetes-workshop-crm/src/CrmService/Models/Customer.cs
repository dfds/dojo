namespace DFDS.CrmService.Models
{
    public class Customer
    {
        public string Name { get; set; }

        public string Email { get; set; }

        public string Id { get; set; }

        public string ImageUrl { get; set; }

        public Address Address { get; set; }
    }
}
