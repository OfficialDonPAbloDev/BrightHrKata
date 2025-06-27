namespace Checkout.Tests
{
    [TestClass]
    public class CheckoutTests
    {
        private Checkout? _cut;

        [TestInitialize]
        public void SetUp()
        {
            _cut = new Checkout();
        }

        [TestMethod]
        public void Empty_basket_get_total_returns_no_cost()
        {
            var result = _cut?.GetTotalCost();
            Assert.AreEqual(0, result);
        }
    }
}