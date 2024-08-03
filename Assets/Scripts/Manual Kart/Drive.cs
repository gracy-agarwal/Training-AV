using System.Collections.Generic;
using UnityEngine;
using System.IO;

public class Drive : MonoBehaviour
{
    // Car Movement Data:
    [SerializeField] private float speed = 50.0F;
    [SerializeField] private float rotationSpeed = 100.0F;
    [SerializeField] private float visibleDistance = 200.0F;
    
    // Car Training Data:
    const string trainingDataPath = "/trainingData.txt";
    List<string> collectedTrainingData = new List<string>();
    StreamWriter traininDataFile;

    void Start() => CreateTrainingDataFile();

    private void CreateTrainingDataFile()
    {
        string fullPath = Application.dataPath + trainingDataPath;
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
        float distanceToCover = Time.deltaTime * speed * translationInput;
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
        recordedData.fDist = RaycastInDirection(transform.forward);
        recordedData.rDist = RaycastInDirection(transform.right);
        recordedData.lDist = RaycastInDirection(-transform.right);
        recordedData.r45Dist = RaycastInDirection(Quaternion.AngleAxis(45, Vector3.up) * transform.forward);
        recordedData.l45Dist = RaycastInDirection(Quaternion.AngleAxis(-45, Vector3.up) * transform.forward);

        UpdateTrainingData(recordedData, translationInput, rotationInput);
    }

    private void UpdateTrainingData(RecordedData recordedData, float translationInput, float rotationInput)
    {
        string trainingData = 
            recordedData.fDist + "," + 
            recordedData.rDist + "," + 
            recordedData.lDist + "," + 
            recordedData.r45Dist + "," + 
            recordedData.l45Dist + "," + 
            Round(translationInput) + "," + 
            Round(rotationInput);

        if(!collectedTrainingData.Contains(trainingData))
            collectedTrainingData.Add(trainingData);
    }

    private float RaycastInDirection(Vector3 direction)
    {
        RaycastHit hit;
        if (Physics.Raycast(transform.position, direction, out hit, visibleDistance))
            return 1 - Round(hit.distance / visibleDistance);
        return 0;
    }

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
    public float fDist;
    public float rDist;
    public float lDist;
    public float r45Dist;
    public float l45Dist;
}
