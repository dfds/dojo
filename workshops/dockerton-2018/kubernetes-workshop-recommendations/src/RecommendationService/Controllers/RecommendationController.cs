using Microsoft.AspNetCore.Mvc;
using DFDS.RecommendationService.Models;
using System.Collections.Generic;
namespace DFDS.RecommendationService.Controllers
{
    public class RecommendationController : ControllerBase
    {
        [Route("/api/customers/{customerId}/recommendations")]
        public ProductsDto Get(int customerId)
        {
            var dto = new ProductsDto();
            var list = new List<Product>()
            {
                new Product() {Description = "Test", Id = "WHATUP", ImageUrl = "https://farm8.staticflickr.com/7436/15886869494_8359a24e48.jpg", Price = 1337.37M },
                new Product() {Description = "Test2", Id = "WHATUP2", ImageUrl = "https://farm4.staticflickr.com/3919/14383534532_02471bbed9.jpg", Price = 345 },
                new Product() {Description = "Test3", Id = "WHATUP3", ImageUrl = "https://farm6.staticflickr.com/5711/31374837845_e76bb54f97_b.jpg", Price = 136453337.37M }
            };

            dto.Products = list;
            return dto;
        }
    }
}