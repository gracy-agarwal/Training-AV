namespace ANN_Kart.ANN.Activation_Functions
{
    public interface IActivationFunction
    {
        public float CalculateOutput(float weightedSum);
    }
}