namespace Checkout
{
    public class Basket : IBasket
    {
        private List<ICatalogueItem> _items;
        public Basket()
        {
            _items = new List<ICatalogueItem>();
        }

        public void Add(ICatalogueItem item)
        {
            _items.Add(item);
        }

        public void Remove(ICatalogueItem item)
        {
            _items.Remove(item);
        }

        public decimal GetAllItemsPrice()
        {
            return _items.Sum(x => x.SalesPrice);
        }
    }
}