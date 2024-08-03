/*using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;

public class Brain : MonoBehaviour
{
    public int numberOfInputsOfNN = 5;
    public int numberOfOutputsOfNN = 2;
    public int numberOfHiddenLayers = 2;
    public int numberOfNeuronPerHiddenLayer = 5;
    public double learningRate = 0.5;
    public ANN ann;
    public Chromosome chromosome;
    public int chromosomeLength = 0;
    public bool alive;
    public float visibleDistance = 50;
    public float speed = 50.0F;
    public float rotationSpeed = 100.0F;
    public float translation;
    public float rotation;
    public float distanceTravelled = 0;
    public float timeAlive = 0;
    Vector3 startPosition;
    public GameObject endPosition;
    public float roadDistance = 0;
    public float averageSpeed = 0;
    public float distanceTravelledMultiplier = 0.8f;
    public float averageSpeedMultiplier = 0.4f;
    public float timeAliveMultiplier = 0.8f;
    public float overallFitness = 0;
    // float sumofTranslation = 0;
    // public float averageTranslationSpeed = 0;
    // float sumofRotation = 0;
    // public float averageRotationalSpeed = 0;
    public PopulationManager pm;

    public void CalChromosomeLength()
    {
        chromosomeLength = ann.getLen();
    }

    public void InitChromosome()
    {
        CalChromosomeLength();
        chromosome = new Chromosome(chromosomeLength);
        List<double> g = ann.getChromosome();
        for (int i = 0; i < chromosomeLength; i++)
        {
            chromosome.genes.Add(g[i]);
        }
    }

    public void Init()
    {
        alive = false;
        ann = new ANN(numberOfInputsOfNN, numberOfOutputsOfNN, numberOfHiddenLayers, numberOfNeuronPerHiddenLayer, learningRate);
        InitChromosome();
        startPosition = transform.position;
        roadDistance = (endPosition.transform.position - startPosition).magnitude;
    }

    public void InitOffspring()
    {
        alive = true;
        ann = new ANN(numberOfInputsOfNN, numberOfOutputsOfNN, numberOfHiddenLayers, numberOfNeuronPerHiddenLayer, learningRate);
        CalChromosomeLength();
        chromosome = new Chromosome(chromosomeLength);
        startPosition = transform.position;
        roadDistance = (endPosition.transform.position - startPosition).magnitude;
    }

    void OnCollisionEnter(Collision col)
    {
        if (col.gameObject.tag == "wall")
        {
            alive = false;
            GetComponent<Rigidbody>().isKinematic = true;
        }
    }

    void Update()
    {
        // if (!alive) return;

        timeAlive += Time.deltaTime;

        distanceTravelled += (transform.position - startPosition).magnitude;
        startPosition = transform.position;

        averageSpeed = distanceTravelled / timeAlive;

        overallFitness = (distanceTravelled * distanceTravelledMultiplier) + (averageSpeed * averageSpeedMultiplier) + (timeAlive * timeAliveMultiplier);

        List<double> calcOutputs = new List<double>();
        List<double> inputs = new List<double>();

        float forwardRay = RaycastDirection(transform.forward);
        float rightRay = RaycastDirection(transform.right);
        float leftRay = RaycastDirection(-transform.right);
        float right45Ray = RaycastDirection(Quaternion.AngleAxis(-45, Vector3.up) * transform.right);
        float left45Ray = RaycastDirection(Quaternion.AngleAxis(45, Vector3.up) * -transform.right);

        inputs.Add(forwardRay);
        inputs.Add(rightRay);
        inputs.Add(leftRay);
        inputs.Add(right45Ray);
        inputs.Add(left45Ray);

        // calcOutputs = ann.CalcOutput(inputs);

        // float translationInput = Map(-1, 1, 0, 1, (float)calcOutputs[0]);
        // float rotationInput = Map(-1, 1, 0, 1, (float)calcOutputs[1]);

        // translation = (translationInput * speed * Time.deltaTime);
        // rotation = (rotationInput * rotationSpeed * Time.deltaTime);

        // transform.Translate(0, 0, translation);
        // transform.Rotate(0, rotation, 0);

        // sumofTranslation += translation;
        // sumofRotation += rotation;

        // averageTranslationSpeed = sumofTranslation / timeAlive;
        // averageRotationalSpeed = sumofRotation / timeAlive;

        // if (averageTranslationSpeed < 0.2 || Mathf.Abs(averageRotationalSpeed) > 1.4)
        // {
        //     overallFitness = 0;
        // }
    }

    float RaycastDirection(Vector3 direction)
    {
        RaycastHit hit;
        float distance = 0;

        if (Physics.Raycast(transform.position, direction, out hit, visibleDistance))
        {
            distance = 1 - Round(hit.distance / visibleDistance);
            Debug.DrawRay(transform.position, direction * visibleDistance, Color.red);
        }
        else
        {
            Debug.DrawRay(transform.position, direction * visibleDistance, Color.green);
        }

        return distance;
    }
    float Round(float x)
    {
        return (float)System.Math.Round(x, 2, System.MidpointRounding.AwayFromZero);
    }

    float Map(float newfrom, float newto, float origfrom, float origto, float value)
    {
        if (value <= origfrom)
            return newfrom;
        else if (value >= origto)
            return newto;
        return (newto - newfrom) * ((value - origfrom) / (origto - origfrom)) + newfrom;
    }
}*/