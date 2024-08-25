using System.Collections.Generic;

public class Chromosome
{
    private List<float> genes;
    public int Length => genes.Count;
    public List<float> Genes => genes;

    public Chromosome(int length) => InitializeGenes(length);

    private void InitializeGenes(int length) => genes = new List<float>(length);

    public void SetGenes(List<float> genesToSet) => genes = genesToSet;
    
    public void SetGeneAtPosition(int position, float value) => genes[position] = value;

    public float GetGeneAtPosition(int position) => genes[position];
}