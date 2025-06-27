using Checkout.Interfaces;
using System.Runtime.InteropServices;

namespace Checkout
{
    public class Checkout : ICheckout
    {
        private readonly IStockCatalogue _stockCatalogue;
        private IBasket _basket;
        public Checkout(IStockCatalogue stockCatalogue)
        {
            _stockCatalogue = stockCatalogue;
            _basket = new Basket();
        }
        public decimal GetTotalCost()
        {
            return _basket.GetAllItemsPrice();
        }

        public void Scan(char sku)
        {
            var item = _stockCatalogue.Items.FirstOrDefault(x => x.Sku == sku);
            if (item != null)
            {
                _basket.Add(item);
            }
        }
    }

    public class StockCatalogue : IStockCatalogue
    {
        public StockCatalogue()
        {
            Items = [new CatalogueItem('A', "A great thing", 50.0M),
                new CatalogueItem('B', "A good useful thing", 30.0M),
                new CatalogueItem('C', "An Ok thing", 20.0M),
                new CatalogueItem('D', "A cheapo thing", 15.0M)];
        }
        public List<CatalogueItem> Items { get; set; }
    }
}
