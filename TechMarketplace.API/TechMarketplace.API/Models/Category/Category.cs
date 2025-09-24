namespace TechMarketplace.API.Models.Category
{
    public class Category
    {
        public int Id { get; set; } 
        public string Name { get; set; }
        public string Description { get; set; }

        public string CateogryImgUrl { get; set; }  

        public List<SubCategory> SubCategories { get; set; } =new List<SubCategory>();


    }
}
