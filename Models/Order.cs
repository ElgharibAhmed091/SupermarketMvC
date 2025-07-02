namespace WebApplication1.Models
{
    public class Order
    {
        public int Id { get; set; }
        //  public string UserId { get; set; } // المستخدم اللي اشترى المنتجات
        public DateTime OrderDate { get; set; } = DateTime.Now;
        public List<orderitem> OrderItems { get; set; } = new List<orderitem>();
        public string UserIdentifier { get; internal set; }
        // public string UserIdentifier { get; internal set; }
    }
}
 