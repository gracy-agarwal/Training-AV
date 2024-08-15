using UnityEngine;
using UnityEngine.Serialization;

namespace ANN_Kart.Genetics
{
    [CreateAssetMenu(menuName = "ScriptableObjects/Car_Data", fileName = "Car_Data")]
    public class CarData : ScriptableObject
    {
        public float visibleDistance = 50;
        [FormerlySerializedAs("Speed")] [FormerlySerializedAs("speed")] public float TranslationSpeed = 50.0F;
        [FormerlySerializedAs("rotationSpeed")] public float RotationSpeed = 100.0F;
    }
}