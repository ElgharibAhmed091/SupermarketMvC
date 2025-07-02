namespace WebApplication1.Models
{
    public class cartitem
    {
        public Product Product { get; set; }  // المنتج الموجود في العربة
        public VEGETABLES VEGETABLES { get; set; }  // المنتج الموجود في العربة
        public int Quantity { get; set; }     // الكمية التي تم إضافتها من هذا المنتج

    }
} 
