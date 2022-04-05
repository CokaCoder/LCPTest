using System;

namespace Test.Discounter
{
    public class DiscountEndOfYear:Discounter,IDiscounter
    {
        #region Constructor
        public DiscountEndOfYear(Projector projector) 
            : base(projector)
        {
        }
        #endregion

        #region Public Methods
        public double[] GetDiscountedValue(double[] inflatedCostWithDecrement)
        {
            double[] value = inflatedCostWithDecrement;

            for (int i = 0; i < value.Length; i++)
            {
                value[i] = value[i] * GetDiscountFactor(i);
            }

            return value;
        }
        #endregion

        #region Private Methods
        private double GetDiscountFactor(int refNumber)
        {
            return Math.Pow(((100+Projector.Inputs.DiscountRate)/100),-(refNumber+1));
        }
        #endregion
    }
}