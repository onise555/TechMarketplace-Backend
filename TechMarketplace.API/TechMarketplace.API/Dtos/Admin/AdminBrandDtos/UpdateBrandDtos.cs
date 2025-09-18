﻿namespace TechMarketplace.API.Dtos.Admin.AdminBrandDtos
{
    public class UpdateBrandDtos
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public string Description { get; set; }

        public string? ImageUrl { get; set; }

        public DateTime CreatedAt { get; set; }
    }
}
