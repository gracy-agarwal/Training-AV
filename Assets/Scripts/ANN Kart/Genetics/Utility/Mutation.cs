using UnityEngine;

namespace ANN_Kart.Genetics.Utility
{
    public static class Mutation
    {
        public static void Mutate(Chromosome chromosome)
        {
            for (int i = 0; i < chromosome.Length; i++)
                chromosome.SetGeneAtPosition(i, Random.Range(-1f, 1f));
        }
    }
}