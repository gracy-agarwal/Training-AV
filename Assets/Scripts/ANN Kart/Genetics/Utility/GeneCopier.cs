namespace ANN_Kart.Genetics.Utility
{
    public static class GeneCopier
    {
        public static void CopyGenes(Chromosome target, Chromosome source)
        {
            for (int i = 0; i < source.Length; i++)
                target.SetGeneAtPosition(i, source.GetGeneAtPosition(i));
        }
    }
}