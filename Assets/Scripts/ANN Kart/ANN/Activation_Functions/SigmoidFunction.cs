namespace ANN_Kart.ANN.Activation_Functions
{
    public class SigmoidFunction : IActivationFunction
    {
        public double CalculateOutput(float weightedSum)
        {
            double k = System.Math.Exp(weightedSum);
            return k / (1.0f + k);
        }
    }
}