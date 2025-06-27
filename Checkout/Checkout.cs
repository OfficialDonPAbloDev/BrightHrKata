using Checkout.Interfaces;

namespace Checkout
{
    public class Checkout(IStockCatalogue stockCatalogue, ISpecialPrices specialPrices) : ICheckout
    {
        private readonly IStockCatalogue _stockCatalogue = stockCatalogue;
        private readonly ISpecialPrices _specialPrices = specialPrices;
        private IBasket _basket = new Basket();
        private decimal _runningTotal = 0M;

        public decimal GetTotalCost()
        {
            return GetSumOfAllNormalItemsPrices() + GetSumOfAllSpecialPriceItemsPrices();
        }

        public void Scan(char sku)
        {
            var item = _stockCatalogue.Items.FirstOrDefault(x => x.Sku == sku);
            if (item != null)
            {
                _basket.Add(item);
            }
        }

        public decimal GetSumOfAllNormalItemsPrices()
        {
            var localSum = 0M;
            var specialPrices = GetCurrentSpecialPrices();

            foreach (var item in _basket.Items
                        .Where(x=> !specialPrices
                                    .Select(y=> y.Sku)
                                    .Distinct()
                                    .Contains(x.Sku)))
            {
                localSum += item.SalesPrice;
            }

            return localSum;
        }

        public decimal GetSumOfAllSpecialPriceItemsPrices()
        {
            var localSum = 0M;
            var specialPrices = GetCurrentSpecialPrices()
                .ToList();

            var applicableBasketItems = _basket
                .Items
                .Where(x => specialPrices.Select(y=> y.Sku)
                .Distinct()
                .Contains(x.Sku))
                .ToList();


            var query = (from specialPrice in specialPrices
                        join applicableItem in applicableBasketItems on specialPrice.Sku equals applicableItem.Sku
                        select new QualifyingItemsSummary
                        (
                            applicableItem.Sku,
                            applicableBasketItems.Count(x => x.Sku == applicableItem.Sku),
                            specialPrice.NumberOfItemsToApply,
                            specialPrice.PromotionPrice,
                            applicableItem.SalesPrice

                        ))
                        .Distinct()
                        .ToList();

            foreach ( var item in query )
            {
                if(item.BasketAmount >= item.NumberToQualifyForPricing)
                {
                    localSum += item.NumberToQualifyForPricing / item.BasketAmount * item.SpecialPrice;
                    if (item.NumberToQualifyForPricing % item.BasketAmount > 0)
                    {
                        localSum += item.NumberToQualifyForPricing % item.BasketAmount * item.individualPrice;
                    }
                }
                else
                {
                    localSum += item.BasketAmount * item.individualPrice;
                }
            }

            return localSum;
        }

        private List<SpecialPrice> GetCurrentSpecialPrices()
        {
            return _specialPrices
                .Prices
                .Where(x => x.ValidDateRangeFrom < DateTime.Now && x.ValidDateRangeTo > DateTime.Now)
                .ToList();
        }
    }
}
