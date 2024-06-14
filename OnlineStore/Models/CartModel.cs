using System.Collections.Generic;

namespace OnlineStore.Models
{
    public class CartModel
    {
        public int Id { get; set; }
        public string UserId { get; set; }
        public ICollection<CartItemModel> Items { get; set; }
    }
}