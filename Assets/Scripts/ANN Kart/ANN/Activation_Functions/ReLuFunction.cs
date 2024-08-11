namespace ANN_Kart.ANN.Activation_Functions
{
    public class ReLuFunction : IActivationFunction
    {
        public float CalculateOutput(float weightedSum)
        {
            if (weightedSum > 0) 
                return weightedSum;
            return 0;
        }
    }
}