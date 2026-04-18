namespace CarSales.Web.Models
{
    public class Favorite
    {
        public int FavoriteId { get; set; }
        public int UserId { get; set; }
        public int CarId { get; set; }
        public DateTime CreatedAt { get; set; } = DateTime.Now;

        public virtual User? User { get; set; }
        public virtual Car? Car { get; set; }
    }
}