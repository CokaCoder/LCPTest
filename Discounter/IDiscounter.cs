namespace Test.Discounter
{
    public interface IDiscounter
    {
        #region Methods
        double[] GetDiscountedValue(double[] inflatedCostWithoutDecrement);
        #endregion
    }
}