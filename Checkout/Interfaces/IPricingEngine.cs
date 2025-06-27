using Checkout.Interfaces;

namespace Checkout
{
    public interface IPricingEngine
    {
        decimal GetSumOfAllNormalItemsPrices(IBasket basket);
        decimal GetSumOfAllSpecialPriceItemsPrices(IBasket basket);
    }
}