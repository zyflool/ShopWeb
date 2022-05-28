namespace ShopWeb.Models
{
    public class Goods
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public double Price { get; set; }

        public int Inventory { get; set; }

        public byte[] Image { get; set; }

        public bool IsDeleted { get; set; }
    }
}
