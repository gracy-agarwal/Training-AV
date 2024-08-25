using System.Collections.Generic;
using UnityEngine;

namespace ANN_Kart.ANN
{
    [CreateAssetMenu(menuName = "ScriptableObjects/ANN_Data", fileName = "ANN_Data")]
    public class ANN_Data : ScriptableObject
    {
        public List<LayerData> Ann_Structure;
        public int NumberOfLayers => Ann_Structure.Count;

        public LayerType GetLayerType(int layerIndex) => Ann_Structure[layerIndex].layerType;
        public int GetNumberOfNeuronAtLayer(int layerIndex) => Ann_Structure[layerIndex].numberOfNeurons;
        
        public int GetNumberOfInputsPerNeuronAtLayer(int layerIndex)
        {
            if (layerIndex == 0)
                return 1;
            
            return Ann_Structure[layerIndex - 1].numberOfNeurons;
        }
    }

    [System.Serializable]
    public struct LayerData
    {
        public LayerType layerType;
        public int numberOfNeurons;
    }
}