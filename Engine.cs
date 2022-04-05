using System.Collections.Generic;
using Test.Discounter;

namespace Test
{
    public class Engine
    {
        #region Public Methods
        public Result GetResultProjections(List<Inputs> listOfInputs)
        {
            List<double[]> listOfProjections = new List<double[]>(4);
            List<double[]> listOfDiscountedProjections = new List<double[]>(4);


            List<Projector> listOfProjectors = new List<Projector>(4);

            foreach (Inputs inputType in listOfInputs)
            {
                //could use var here. "var projector = new Projector(inputType)"
                Projector projector = new Projector(inputType);

                //These two both hold the same value. So you could just store the returned result from "projector.GetInflatedCost" and use it wherever you need it.
                //Rather than store the same value twice. IE- var cost = projector.GetInflatedCost();
                double[] inflatedCost = projector.GetInflatedCost();
                double[] inflatedCostWithDec = projector.GetInflatedCost();

                //In fact the above 3 lines could be replaced with just this couldn't it?
                //var cost = new Projector(inputType).GetInflatedCost();
                //and then just reference "cost" below where you call UpdateInflatedCostWithDecrement and GetDiscountedValue


                projector.UpdateInflatedCostWithDecrement(inflatedCost);

                listOfProjections.Add(inflatedCost);
                listOfProjectors.Add(projector);

                if (projector.Inputs.IsContinuous)
                {
                    DiscounterContinuous discounterContinuous = new DiscounterContinuous(projector);
                    double[] discountedProjections = discounterContinuous.GetDiscountedValue(inflatedCostWithDec);
                    listOfDiscountedProjections.Add(discountedProjections);
                }
                else
                {
                    DiscountEndOfYear discountEndOfYear = new DiscountEndOfYear(projector);
                    double[] discountedProjections = discountEndOfYear.GetDiscountedValue(inflatedCostWithDec);
                    listOfDiscountedProjections.Add(discountedProjections);
                }
            }
            return new Result(listOfProjections, listOfDiscountedProjections);
        }

        public Result GetRollForwardProjections(List<Inputs> listOfInputs, int rollForwardYears)
        {
            List<double[]> rollForwardProjectionsList = new List<double[]>(4);

            List<double[]> rollForwardDiscountedProjectionsList = new List<double[]>(4);

            foreach (var input in listOfInputs)
            {
                Projector projector = new Projector(input);

                double[] preRollForwardProjection = projector.GetInflatedCost();
                projector.UpdateInflatedCostWithDecrement(preRollForwardProjection);

                RollForward rollForward = new RollForward(projector);

                double[] rollForwardProjection = rollForward.GetRollForwardProjections(rollForwardYears, preRollForwardProjection);
                double[] rollForwardProjection2 = rollForward.GetRollForwardProjections(rollForwardYears, preRollForwardProjection);
                rollForwardProjectionsList.Add(rollForwardProjection);

                double[] rollForwardDiscountedProjections = rollForward.GetRollForwardDiscountedProjections(rollForwardYears, rollForwardProjection2);
                rollForwardDiscountedProjectionsList.Add(rollForwardDiscountedProjections);
            }
            return new Result(rollForwardProjectionsList, rollForwardDiscountedProjectionsList);
        }
        #endregion
    }
}