namespace Checkout
{
    public interface IBasket
    {
        void Add(ICatalogueItem item);
        void Remove(ICatalogueItem item);
        decimal GetAllItemsPrice();
    }
}