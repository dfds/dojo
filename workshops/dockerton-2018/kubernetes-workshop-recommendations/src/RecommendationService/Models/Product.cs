using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DFDS.RecommendationService.Models
{
    //"products": [
    //{
    //  "id": "string",
    //  "description": "string",
    //  "imageUrl": "string",
    //  "price": 0
    //}
    public class ProductsDto
    {
        public IEnumerable<Product> Products { get; set; }
    }

    public class Product
    {
        public string Id { get; set; }
        public string Description { get; set; }
        public string ImageUrl { get; set; }
        public decimal Price { get; set; }
    }
}
