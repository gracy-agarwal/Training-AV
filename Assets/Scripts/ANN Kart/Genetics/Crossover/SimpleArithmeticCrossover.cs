using System.Collections.Generic;

namespace ANN_Kart.Genetics.Crossover
{
    public class SimpleArithmeticCrossover : ICrossover
    {
        public List<Chromosome> Crossover(Chromosome parent1, Chromosome parent2)
        {
            Chromosome offspring1 = new Chromosome();
            Chromosome offspring2 = new Chromosome();

            int crossoverPoint = UnityEngine.Random.Range(0, parent1.Genes.Count);
            float alpha = 0.5f; // Weight factor can be adjusted

            for (int i = 0; i < parent1.Genes.Count; i++)
            {
                if (i < crossoverPoint)
                {
                    offspring1.Genes[i] = parent1.Genes[i];
                    offspring2.Genes[i] = parent2.Genes[i];
                }
                else
                {
                    offspring1.Genes[i] = alpha * parent1.Genes[i] + (1 - alpha) * parent2.Genes[i];
                    offspring2.Genes[i] = alpha * parent2.Genes[i] + (1 - alpha) * parent1.Genes[i];
                }
            }

            return new List<Chromosome> { offspring1, offspring2 };
        }
    }
}