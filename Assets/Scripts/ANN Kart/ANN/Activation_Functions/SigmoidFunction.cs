namespace ANN_Kart.ANN.Activation_Functions
{
    public class SigmoidFunction : IActivationFunction
    {
        public double CalculateOutput(float weightedSum)
        {
            double k = System.Math.Exp(-1 * weightedSum);
            return 1 / (1.0f + k);
        }
    }
}