namespace Repository.ModelView
{
    public class InteriorView
    {
        public required string InteriorName { get; set; }
        public required string Size { get; set; }
        public required string MaterialId { get; set; }
        public required string Description { get; set; }
        public required string UrlImage { get; set; }
        public required int Quantity { get; set; }
        public required int Price { get; set; }
    }
}
