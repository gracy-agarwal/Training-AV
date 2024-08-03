namespace ANN_Kart.ANN.Activation_Functions
{
    public class TangentFunction : IActivationFunction
    {
        public double CalculateOutput(float weightedSum)
        {
            double k = System.Math.Exp(-2 * weightedSum);
            return 2 / (1.0f + k) - 1;
        }
    }
}