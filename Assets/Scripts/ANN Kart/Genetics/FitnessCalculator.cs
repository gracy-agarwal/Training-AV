namespace ANN_Kart.Genetics.Genetics
{
    public static class FitnessCalculator
    {
        private static float distanceTravelledMultiplier = 0.8f;
        private static float lifetimeMultiplier = 0.5f;

        public static float CalculateFitness(float lifetime, float distanceTravelled)
        {
            return (lifetime * lifetimeMultiplier) + (distanceTravelled * distanceTravelledMultiplier);
        }
    }
}