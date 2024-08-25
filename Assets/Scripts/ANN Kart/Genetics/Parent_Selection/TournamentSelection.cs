using System.Collections.Generic;
using System.Linq;

namespace ANN_Kart.Genetics.Genetics.Parent_Selection
{
    public class TournamentSelection : ISelection
    {
        public CarPerformanceData SelectParents(List<CarPerformanceData> currentPopulationData)
        {
            int tournamentSize = 3; // This can be adjusted depending on how selective you want the tournament to be.
            List<CarPerformanceData> tournament = new List<CarPerformanceData>();

            for (int i = 0; i < tournamentSize; i++)
            {
                int randomIndex = UnityEngine.Random.Range(0, currentPopulationData.Count);
                tournament.Add(currentPopulationData[randomIndex]);
            }

            // Return the best car from the tournament
            return tournament.OrderByDescending(data => data.FitnessValue).First();
        }
    }
}