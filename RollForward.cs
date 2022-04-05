using System;
using Test.Discounter;

namespace Test
{
    public class RollForward
    {
        #region Constructor
        public RollForward(Projector projector)
        {
            Projector = projector;
        }
        #endregion

        #region Properties

        public Projector Projector;
        #endregion

        #region Public Methods
        public double[] GetRollForwardProjections(int rollForwardYear, double[] inflatedArrayWithDecrement)
        {
            double assumedInflationFactor = 1 + Projector.Inputs.Inflation / 100;
            double knownInflationFactor = 1 + Projector.Inputs.RollForwardInflationRate / 100;

            double[] rollForwardCashflows = new double[inflatedArrayWithDecrement.Length - rollForwardYear];

            for (int i = 0; i < rollForwardCashflows.Length; i++)
            {
                double discountFactor = Math.Pow(1 + Projector.Inputs.DiscountRate, -1);

                rollForwardCashflows[i] = inflatedArrayWithDecrement[i + rollForwardYear] *
                                          Math.Pow(knownInflationFactor, rollForwardYear) /
                                          Math.Pow(assumedInflationFactor, rollForwardYear) * discountFactor;
            }

            return rollForwardCashflows;
        }

        public double[] GetRollForwardDiscountedProjections(int rollForwardYears, double[] rollForwardProjections)
        {
            if (Projector.Inputs.IsContinuous)
            {
                DiscounterContinuous discounterContinuous = new DiscounterContinuous(Projector);
                return discounterContinuous.GetDiscountedValue(rollForwardProjections);
            }

            else
            {
                DiscountEndOfYear discountEndOfYear = new DiscountEndOfYear(Projector);
                return discountEndOfYear.GetDiscountedValue(rollForwardProjections);
            }
        }
        #endregion
    }
}
