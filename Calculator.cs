using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Test.Discounter;

namespace Test
{
    //I would suggest we make this Calculator class an implementation of an interface, so we can de-couple a lot of these classes from each other.
    //I would also suggest that we separate this out, so that we have a "Calculate service" which lives in a separate project.
    //The Calculate service "CalcService" would implement "ICalcService" both of which live in their own separate class library.
    //That way your CalcService, can be used in multiple places. And if you want to, you can have different implementations of it to suit your needs.

    //It would also be good to make these methods asynchronous. My guess is the UI has buttons which call them?
    //So the users experience is that the app freezes whenever one of these is called. I would be looking to make these methods async. For example..

    //public async Task<bool> RunRollForwardAsync()
    //{
    //    return await Task.Run(() =>
    //    {
    //        foreach (Inputs input in _listOfAllInputs)
    //        {
    //            Projector projector = new Projector(input);
    //            RollForward rollForward = new RollForward(projector);

    //            double[] yearlyInflatedCashflowAtBaseDate = projector.GetInflatedCostWuthDecrement();

    //            double[] rollForwardProjections = rollForward.GetRollForwardProjections(projector.Inputs.Time, yearlyInflatedCashflowAtBaseDate);
    //            ProjectedRollForwardCashflowsForEachOutgoingType.Add(rollForwardProjections);
    //        }

    //        double[] aggregatedRollForwardCashflow = Aggregator.AggregateYearlyProjections(ProjectedRollForwardCashflowsForEachOutgoingType, _listOfAllInputs);
    //        AggregatedRollForwardCashflows = aggregatedRollForwardCashflow;

    //        return true;
    //    });
    //}

    //There also doesn't appear to be any error handling going on, or logging of those errors.  Only the happy path is catered for here.
    //If one of these methods throws an error what happens?

    //Async methods do tend to spread like a zombie virus, so it could be a big re-factor. But it would probably be worth it long term, especially if there are
    //calculation methods that use a database connection to grab data.  

    public class Calculator
    {
        public Calculator(List<Inputs> listOfAllInputs)
        {
            _listOfAllInputs = listOfAllInputs;
            TermForAllProjections = MaximumTermOfProjection.SetTermOfProjection(listOfAllInputs);

            ProjectedCashflowsForEachOutgoingType = new List<double[]>();
            ProjectedRollForwardCashflowsForEachOutgoingType = new List<double[]>();

            RunProjector();
            RunRollForward();
        }
        
        private readonly List<Inputs> _listOfAllInputs;

        public int TermForAllProjections { get; }
        public List<double[]> ProjectedCashflowsForEachOutgoingType { get; }
        public List<double[]> ProjectedRollForwardCashflowsForEachOutgoingType { get; }
        public double[] AggregatedCashflows { get; private set; }
        public double[] AggregatedRollForwardCashflows { get; private set; }



        //Most of these methods return "void" which doesn't any control.
        //Generally, if we don't know how to deal with a problem in the code, we should throw an exception.
        //If we do know how to deal with a problem, we could return a "Result" class.
        //Suggest you look at "FunctionalExtensions" https://enterprisecraftsmanship.com/posts/c-functional-extensions-nuget-library/
        //This supports the "Railway Orientated" approach, so you can return a "Result" which is either a success or a failure.

        public void RunProjector()
        {
            foreach (Inputs input in _listOfAllInputs)
            {
                Projector projector = new Projector(input);
                DiscounterContinuous discounter = new DiscounterContinuous(projector);

                double[] arrayOfDiscountedCashflows = discounter.GetDiscountedValue(projector.GetInflatedCostWuthDecrement());
                ProjectedCashflowsForEachOutgoingType.Add(arrayOfDiscountedCashflows);
            }

            double[] aggregatedCashflow = Aggregator.AggregateYearlyProjections(ProjectedCashflowsForEachOutgoingType, _listOfAllInputs);
            AggregatedCashflows = aggregatedCashflow;
        }

        public void RunRollForward()
        {
            foreach (Inputs input in _listOfAllInputs)
            {
                Projector projector = new Projector(input);
                RollForward rollForward = new RollForward(projector);

                double[] yearlyInflatedCashflowAtBaseDate = projector.GetInflatedCostWuthDecrement();

                double[] rollForwardProjections = rollForward.GetRollForwardProjections(projector.Inputs.Time, yearlyInflatedCashflowAtBaseDate);
                ProjectedRollForwardCashflowsForEachOutgoingType.Add(rollForwardProjections);
            }

            double[] aggregatedRollForwardCashflow = Aggregator.AggregateYearlyProjections(ProjectedRollForwardCashflowsForEachOutgoingType, _listOfAllInputs);
            AggregatedRollForwardCashflows = aggregatedRollForwardCashflow;
        }

        public double[] CalculateTotalInfWithDec()
        {
            //Usually it's better to use var.  This could be simplified to "var ListOfSums = new List<double>()""
            List<double> sums = new List<double>();  

            double[] cashFlow = new Projector(_listOfAllInputs.First()).GetInflatedCostWuthDecrement();

            int currentSumsLength = sums.Count;

            for (int i =0; i < cashFlow.Length; i++)
            {
                if (i > currentSumsLength-1)
                {
                    sums.Add(cashFlow[i]);
                }
                else
                {
                    sums[i] += cashFlow[i];
                }
            }

            double[] totalArray = sums.ToArray();

            return totalArray;
        }

        public double[] CalculateTotalInfCashflows()
        {
            List<double> sums = new List<double>();

            double[] cashFlow = new Projector(_listOfAllInputs.First()).GetInflatedCost();

            int currentSumsLength = sums.Count;

            for (int i = 0; i < cashFlow.Length; i++)
            {
                if (i > currentSumsLength - 1)
                {
                    sums.Add(cashFlow[i]);
                }
                else
                {
                    sums[i] += cashFlow[i];
                }
            }

            double[] totalArray = sums.ToArray();

            return totalArray;
        }
        
        public double[] CalculateTotalInfCashflows(bool withDecrement)
        {
            if (withDecrement)
            {
                List<double> sums = new List<double>();

                double[] cashFlow = new Projector(_listOfAllInputs.First()).GetInflatedCostWuthDecrement();

                int currentSumsLength = sums.Count;

                for (int i = 0; i < cashFlow.Length; i++)
                {
                    if (i > currentSumsLength - 1)
                    {
                        sums.Add(cashFlow[i]);
                    }
                    else
                    {
                        sums[i] += cashFlow[i];
                    }
                }

                double[] totalArray = sums.ToArray();

                return totalArray;
            }

            List<double> sumsNoDec = new List<double>();

            double[] cashFlowNoDec = new Projector(_listOfAllInputs.First()).GetInflatedCost();

            int currentSumsLengthNoDec = sumsNoDec.Count;

            for (int i = 0; i < cashFlowNoDec.Length; i++)
            {
                if (i > currentSumsLengthNoDec - 1)
                {
                    sumsNoDec.Add(cashFlowNoDec[i]);
                }
                else
                {
                    sumsNoDec[i] += cashFlowNoDec[i];
                }
            }

            double[] totalArrayNoDec = sumsNoDec.ToArray();

            return totalArrayNoDec;
        }
    }
}
