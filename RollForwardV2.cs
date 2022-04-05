using System;

namespace Test
{
    public class RollForwardV2
    {
        #region Constructor
        public RollForwardV2(Projector projector)
        {
            Projector = projector;
        }
        #endregion

        #region Properties

        public Projector Projector;
        #endregion

        #region Public Methods

        public double[] GetRollForwardProjections(int rollForwardYear, double[] inflatedArrayWithDecrement, bool isContinuous)
        {
            double assumedInflationFactor = 1 + Projector.Inputs.Inflation / 100;
            
            double[] cashflowsAtRollforwardDate = new double[inflatedArrayWithDecrement.Length - rollForwardYear];

            if (isContinuous)
            {
                for (int i = 0; i < cashflowsAtRollforwardDate.Length; i++)
                {
                    cashflowsAtRollforwardDate[i] = inflatedArrayWithDecrement[i + rollForwardYear] *
                                                    ContinuousMultiplicationFactor(i, assumedInflationFactor, rollForwardYear);
                } 
            }
            else
            {
                for (int i = 0; i < cashflowsAtRollforwardDate.Length; i++)
                {
                    double discountFactor = Math.Pow(1 + Projector.Inputs.DiscountRate, -1);
                    cashflowsAtRollforwardDate[i] = inflatedArrayWithDecrement[i + rollForwardYear] * DiscreteMultiplicationFactor(discountFactor, i, assumedInflationFactor, rollForwardYear);
                }
            }

            return cashflowsAtRollforwardDate;
        }


        private double ContinuousMultiplicationFactor(int refNum, double inflationFactorOLD, int rollForwardYear)
        {
            return Math.Pow(1 + Projector.Inputs.RollForwardInflationRate, rollForwardYear) 
                   * Math.Exp(-((refNum + Projector.Inputs.YearsToRollForwardBy) * Projector.Inputs.DiscountRate))
                   / Math.Pow(inflationFactorOLD, rollForwardYear);
        }
        
        private double DiscreteMultiplicationFactor(double discountFactor, int refNum, double inflationFactorOLD, int rollForwardYear)
        {
            return Math.Pow(1 + Projector.Inputs.RollForwardInflationRate, rollForwardYear)
                   * Math.Pow(discountFactor, refNum + Projector.Inputs.YearsToRollForwardBy)
                   / Math.Pow(inflationFactorOLD, rollForwardYear);
        }

        #endregion
    }
}
