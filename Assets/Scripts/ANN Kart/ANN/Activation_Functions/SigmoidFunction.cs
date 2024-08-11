namespace ANN_Kart.ANN.Activation_Functions
{
    public class SigmoidFunction : IActivationFunction
    {
        public float CalculateOutput(float weightedSum)
        {
            double k = System.Math.Exp(-1 * weightedSum);
            return 1.0f / (float) (1.0f + k);
        }
    }
}