using System.Collections.Generic;
using System.Linq;

namespace ANN_Kart.Genetics.Genetics.Parent_Selection
{
    public class FitnessProportionateSelection : ISelection
    {
        public CarPerformanceData SelectParents(List<CarPerformanceData> currentPopulationData)
        {
            {
                // Calculate the total fitness of the population
                float totalFitness = currentPopulationData.Sum(data => data.FitnessValue);

                // Generate a random value between 0 and the total fitness
                float randomValue = UnityEngine.Random.Range(0f, totalFitness);

                // Iterate over the population and find the individual corresponding to the random value
                float runningSum = 0f;
                foreach (var individual in currentPopulationData)
                {
                    runningSum += individual.FitnessValue;

                    // If the running sum surpasses the random value, select this individual
                    if (runningSum >= randomValue)
                    {
                        return individual;
                    }
                }

                // In case of rounding errors, return the last individual as a fallback
                return currentPopulationData.Last();
            }
        }
    }
}