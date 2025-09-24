namespace TechMarketplace.API.Dtos.Filters
{
    public class SearchByPrice
    {
        public int Id { get; set; } 
        public string Name { get; set; }  

        public decimal Price { get; set; }

        public string img {  get; set; }    
    }
}
