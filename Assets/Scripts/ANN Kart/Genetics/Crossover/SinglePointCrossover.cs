using System.Collections.Generic;

namespace ANN_Kart.Genetics.Crossover
{
    public class SinglePointCrossover: ICrossover
    {
        public List<Chromosome> Crossover(Chromosome parent1, Chromosome parent2)
        {
            Chromosome offspring1 = new Chromosome();
            Chromosome offspring2 = new Chromosome();

            int crossoverPoint = UnityEngine.Random.Range(0, parent1.Genes.Count);

            for (int i = 0; i < parent1.Genes.Count; i++)
            {
                if (i < crossoverPoint)
                {
                    offspring1.Genes[i] = parent1.Genes[i];
                    offspring2.Genes[i] = parent2.Genes[i];
                }
                else
                {
                    offspring1.Genes[i] = parent2.Genes[i];
                    offspring2.Genes[i] = parent1.Genes[i];
                }
            }

            List<Chromosome> offspring = new List<Chromosome> { offspring1, offspring2 };
            return offspring;
        }
    }
}