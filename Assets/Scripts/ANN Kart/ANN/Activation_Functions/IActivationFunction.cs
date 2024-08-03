namespace ANN_Kart.ANN.Activation_Functions
{
    public interface IActivationFunction
    {
        public double CalculateOutput(float weightedSum);
    }
}