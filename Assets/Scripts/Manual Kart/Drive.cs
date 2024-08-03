using System.Collections.Generic;
using UnityEngine;
using System.IO;
using UnityEngine.Serialization;

public class Drive : MonoBehaviour
{
    // Car Movement Data:
    [SerializeField] private float translationSpeed = 50.0F;
    [SerializeField] private float rotationSpeed = 100.0F;
    [SerializeField] private float visibleDistance = 200.0F;
    
    // Car Training Data:
    const string TrainingDataPath = "/trainingData.txt";
    List<string> collectedTrainingData = new List<string>();
    StreamWriter traininDataFile;

    void Start() => CreateTrainingDataFile();

    private void CreateTrainingDataFile()
    {
        string fullPath = Application.dataPath + TrainingDataPath;
        traininDataFile = File.CreateText(fullPath);
    }
    
    void Update()
    {
        float cachedTranslationalInput = HandleTranslationalMovement();
        float cachedRotationalInput = HandleRotationalMovement();
        DrawRaycasts(cachedTranslationalInput, cachedRotationalInput);
    }
    
    private float HandleTranslationalMovement()
    {
        float translationInput = Input.GetAxis("Vertical");
        float distanceToCover = Time.deltaTime * translationSpeed * translationInput;
        transform.Translate(0, 0, distanceToCover);
        return translationInput;
    }

    private float HandleRotationalMovement()
    {
        float rotationInput = Input.GetAxis("Horizontal");
        float angleToRotate = Time.deltaTime * rotationSpeed * rotationInput;
        transform.Rotate(0, angleToRotate, 0);
        return rotationInput;
    }
    
    private void DrawRaycasts(float translationInput, float rotationInput)
    {
        RecordedData recordedData;
        recordedData.forwardHitProximity = RaycastInDirection(transform.forward);
        recordedData.rightHitProximity = RaycastInDirection(transform.right);
        recordedData.leftHitProximity = RaycastInDirection(-transform.right);
        recordedData.forwardRightHitProximity = RaycastInDirection(Quaternion.AngleAxis(45, Vector3.up) * transform.forward);
        recordedData.forwardLeftHitProximity = RaycastInDirection(Quaternion.AngleAxis(-45, Vector3.up) * transform.forward);

        UpdateTrainingData(recordedData, translationInput, rotationInput);
    }

    private void UpdateTrainingData(RecordedData recordedData, float translationInput, float rotationInput)
    {
        string trainingData = 
            recordedData.forwardHitProximity + "," + 
            recordedData.rightHitProximity + "," + 
            recordedData.leftHitProximity + "," + 
            recordedData.forwardRightHitProximity + "," + 
            recordedData.forwardLeftHitProximity + "," + 
            Round(translationInput) + "," + 
            Round(rotationInput);

        if(!collectedTrainingData.Contains(trainingData))
            collectedTrainingData.Add(trainingData);
    }

    /// <summary>
    /// Shoots a raycast in the provided direction.
    /// It returns a value from 0 to 1 specifying the intensity of how close the collision occured is to the car.
    /// If no Collision occurs, 0 is returned
    /// </summary>
    /// <param name="direction"></param>
    /// <returns></returns>
    private float RaycastInDirection(Vector3 direction)
    {
        RaycastHit hit;
        if (Physics.Raycast(transform.position, direction, out hit, visibleDistance))
            return 1 - Round(hit.distance / visibleDistance);
        return 0;
    }

    /// <summary>
    /// This function rounds a floating-point number x to the nearest 0.5 increment.
    /// </summary>
    /// <param name="x"></param>
    /// <returns></returns>
    float Round(float x) => (float)System.Math.Round(x * 2, System.MidpointRounding.AwayFromZero) / 2.0f;
    
    void OnApplicationQuit() => SaveTrainingData();

    private void SaveTrainingData()
    {
        foreach(string data in collectedTrainingData)
            traininDataFile.WriteLine(data);
        traininDataFile.Close();
    }
}

public struct RecordedData
{
    public float forwardHitProximity;
    public float rightHitProximity;
    public float leftHitProximity;
    public float forwardRightHitProximity;
    public float forwardLeftHitProximity;
}
