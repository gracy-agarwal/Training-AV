using UnityEngine;

namespace ANN_Kart.Genetics.Genetics
{
    [CreateAssetMenu(menuName = "ScriptableObjects/Population_Data", fileName = "Population_Data")]
    public class PopulationData : ScriptableObject
    {
        [SerializeField] public CarController carPrefab;
        public int PopulationSize = 50;
        public int IterationSize = 5;
        public int MaximumGeneration = 20;
        // maximum time for a population to be alive
        public float TrialTime = 10.0f;
        public Vector3 startingPosition;
        public Vector3 endPosition;
    }
}