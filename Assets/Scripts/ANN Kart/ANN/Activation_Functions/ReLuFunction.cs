namespace ANN_Kart.ANN.Activation_Functions
{
    public class ReLuFunction : IActivationFunction
    {
        public double CalculateOutput(float weightedSum)
        {
            if (weightedSum > 0) 
                return weightedSum;
            return 0;
        }
    }
}