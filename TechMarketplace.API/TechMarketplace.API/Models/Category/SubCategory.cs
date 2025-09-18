﻿using TechMarketplace.API.Models.Products;

namespace TechMarketplace.API.Models.Category
{
    public class SubCategory
    {
        public int Id { get; set; }
        public string Name { get; set; }

        public string Description { get; set; }

        public int CategoryId { get; set; }
            
        public Category Category { get; set; }  

        public List<Product> Products { get; set; } = new List<Product>();  
    }
}
