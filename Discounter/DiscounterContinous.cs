using System;
using System.Linq;

namespace Test.Discounter
{
    public class DiscounterContinuous : Discounter, IDiscounter

    {
        #region Constructor
        public DiscounterContinuous(Projector projector) 
            : base(projector)
        {
        }
        #endregion

        #region Public Methods
        public double[] GetDiscountedValue(double[] inflatedCostWithDecrement)
        {


            //You might want to consider using LINQ to do things like this.
            //LINQ basically uses loops, and although the performance gain is small,
            //here it can turn 5 lines of code into just 1, and is more concise.
            //eg ....return inflatedCostWithDecrement.Select((value, index) => value * GetDiscountFactor(index)).ToArray();



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
            return Math.Pow(Math.Exp((-Projector.Inputs.DiscountRate)/100), refNumber + 0.5);
        }
        #endregion
    }
}
