using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Animal : MonoBehaviour
{

    [Header("Animal parameters")]
    public float swapRate = 0.01f;
    public float mutateRate = 0.01f;
    public float swapStrength = 10.0f;
    public float mutateStrength = 0.5f;
    public float maxAngle = 10.0f;

    [Header("Energy parameters")]
    public float maxEnergy = 10.0f;
    public float lossEnergy = 0.1f;
    public float gainEnergy = 10.0f;
    private float energy;

    [Header("Sensor - Vision")]
    public float maxVision = 20.0f;
    public float stepAngle = 10.0f;
    public int nEyes = 5;

    private int[] networkStruct;
    private SimpleNeuralNet brain = null;

    // Terrain.
    private CustomTerrain terrain = null;
    private int[,] details = null;
    private Vector2 detailSize;
    private Vector2 terrainSize;

    // Animal.
    private Transform tfm;
    private float[] vision;

    // Genetic alg.
    private GeneticAlgo genetic_algo = null;

    // Renderer.
    private Material mat = null;


    // pyuan
    [HideInInspector]
    public long id;
    public float maxSpeed = 0.5f;
    public Transform referGoal;
    public float maxMovingStraightDis = 10;

    private Transform currentGoal;
    private float keepRotatingAngle = 0;
    private float keepMovingDistance = 0;
    private bool isStopMoving;

    void Start()
    {
        // Network: 1 input per receptor, 1 output per actuator.
        vision = new float[nEyes];
        networkStruct = new int[] { nEyes, 5, 1 };
        energy = maxEnergy;
        tfm = transform;

        // Renderer used to update animal color.
        // It needs to be updated for more complex models.
        MeshRenderer renderer = GetComponentInChildren<MeshRenderer>();
        if (renderer != null)
            mat = renderer.material;

        isStopMoving = false;

        // pyuan Look at here, currentGoal is not correct

        GameObject goalParent = GameObject.Find("Goals");
        GameObject obj = new GameObject(gameObject.name + "_CurrentGoal" + id);
        obj.transform.SetParent(goalParent.transform);
        currentGoal = obj.transform;
        currentGoal.position = referGoal.position;
        GetComponent<QuadrupedProceduralMotion>().goal = currentGoal;

        // create four foot targets
        CreateFootTargets();
    }

    private void CreateFootTargets()
    {
        var qpm = GetComponent<QuadrupedProceduralMotion>();
        FootStepper[] foots = { qpm.frontLeftFoot, qpm.frontRightFoot, qpm.backLeftFoot, qpm.backRightFoot };
        string[] names = { "Front Left Leg", "Front Right Leg", "Back Left Leg", "Back Right Leg" };

        for (int i = 0; i < foots.Length; i++)
        {
            GameObject footParent = GameObject.Find("FootTargets");
            GameObject obj = Instantiate(foots[i].gameObject);
            obj.name = foots[i].gameObject.name + "_animal_" + id;
            obj.transform.SetParent(footParent.transform);
            obj.transform.position = foots[i].transform.position;
            obj.transform.localScale = foots[i].transform.localScale;
            obj.transform.GetComponent<MeshRenderer>().enabled = false;
            foots[i].gameObject.SetActive(false); // foots[i] is the old reference!

            if (i == 0)
                qpm.frontLeftFoot = obj.GetComponent<FootStepper>();
            if (i == 1)
                qpm.frontRightFoot = obj.GetComponent<FootStepper>();
            if (i == 2)
                qpm.backLeftFoot = obj.GetComponent<FootStepper>();
            if (i == 3)
                qpm.backRightFoot = obj.GetComponent<FootStepper>();

            qpm.frontLeftFoot.gameObject.SetActive(true);
            qpm.frontRightFoot.gameObject.SetActive(true);
            qpm.backLeftFoot.gameObject.SetActive(true);
            qpm.backRightFoot.gameObject.SetActive(true);

            // bind to correct target
            GameObject leg = FindGameObject(names[i], gameObject);
            leg.GetComponent<FabricIKQuadruped>().target = obj.transform;
        }

    }

    private GameObject FindGameObject(string name, GameObject parent)
    {
        List<GameObject> objs = new List<GameObject>();
        objs.Add(parent);
        while (objs.Count>0)
        {
            GameObject current = objs[objs.Count - 1];
            objs.RemoveAt(objs.Count - 1);

            if (current.name == name)
                return current;
            else
            {
                if (current.transform.childCount > 0)
                {
                    for(int i = 0; i < current.transform.childCount; i++)
                    {
                        objs.Add(current.transform.GetChild(i).gameObject);
                    }
                }
            }
        }
        return null;
    }

    private void OnDestroy()
    {
        if (currentGoal != null)
            Destroy(currentGoal.gameObject);
    }

    void Update()
    {
        // In case something is not initialized...
        if (brain == null)
            brain = new SimpleNeuralNet(networkStruct);
        if (terrain == null)
            return;
        if (details == null)
        {
            UpdateSetup();
            return;
        }

        // Retrieve animal location in the heighmap
        int dx = (int)((tfm.position.x / terrainSize.x) * detailSize.x);
        int dy = (int)((tfm.position.z / terrainSize.y) * detailSize.y);

        // For each frame, we lose lossEnergy
        energy -= lossEnergy;

        // If the animal is located in the dimensions of the terrain and over a grass position (details[dy, dx] > 0), it eats it, gain energy and spawn an offspring.
        if ((dx >= 0) && dx < (details.GetLength(1)) && (dy >= 0) && (dy < details.GetLength(0)) && details[dy, dx] > 0)
        {
            // Eat (remove) the grass and gain energy.
            details[dy, dx] = 0;
            energy += gainEnergy;
            if (energy > maxEnergy)
                energy = maxEnergy;

            //pyuan uncomment below code, cause I don;t want to more than one creature when testing
            //genetic_algo.addOffspring(this);
            keepRotatingAngle = 0; // first to give a chance to rotate
        }

        // If the energy is below 0, the animal dies.
        if (energy < 0)
        {
            //pyuan uncomment below code, cause I want to know what issues are
            //energy = 0.0f;
            //genetic_algo.removeAnimal(this);
            //isStopMoving = true;
        }

        // Update the color of the animal as a function of the energy that it contains.
        if (mat != null)
            mat.color = Color.white * (energy / maxEnergy);

        // pyuan
        if (!isStopMoving)
        {
            // find next food position

            // 1. Update receptor.
            UpdateVision();

            // 2. Use brain.
            float[] output = brain.getOutput(vision);

            // 3. Act using actuators.
            // output[0] is [0,1] -> [-1,1]
            float result = (output[0] * 2.0f - 1.0f);

            bool condition = MathF.Abs(result) <= 0.1f || keepMovingDistance < 20 || Mathf.Abs(keepRotatingAngle) >= 360; // it tells animal should keep moving straight?

            if (condition)
            {
                // go straight 
                Vector3 v = tfm.rotation * Vector3.forward * maxSpeed;
                currentGoal.position += v;

                keepMovingDistance += maxSpeed;

                if (Mathf.Abs(keepRotatingAngle) >= 360)
                    keepRotatingAngle = 0;
            }
            else
            {
                if (keepMovingDistance >= 20)
                    keepMovingDistance = 0; // give the chane next time to go

                // turn around
                // it seems food is not in animal's visual area
                // we have two options, rotate or keep moving straight
                // we can simple rotate
                float angle = result * maxAngle;
                keepRotatingAngle += angle;
                Vector3 dir = currentGoal.position - transform.position;
                float distance = dir.magnitude;
                dir = dir.normalized;
                Vector3 newDir = Quaternion.AngleAxis(angle, Vector3.up) * dir;
                Vector3 newPos = transform.position + newDir * distance;
                currentGoal.position = newPos;
            }

        }
    }

    /// <summary>
    /// Calculate distance to the nearest food resource, if there is any.
    /// </summary>
    private void UpdateVision()
    {
        float startingAngle = -((float)nEyes / 2.0f) * stepAngle;
        Vector2 ratio = detailSize / terrainSize;

        for (int i = 0; i < nEyes; i++)
        {
            Quaternion rotAnimal = tfm.rotation * Quaternion.Euler(0.0f, startingAngle + (stepAngle * i), 0.0f);
            Vector3 forwardAnimal = rotAnimal * Vector3.forward;
            float sx = tfm.position.x * ratio.x;
            float sy = tfm.position.z * ratio.y;
            vision[i] = 1.0f;

            // Interate over vision length.
            for (float distance = 1.0f; distance < maxVision; distance += 0.5f)
            {
                // Position where we are looking at.
                float px = (sx + (distance * forwardAnimal.x * ratio.x));
                float py = (sy + (distance * forwardAnimal.z * ratio.y));

                if (px < 0)
                    px += detailSize.x;
                else if (px >= detailSize.x)
                    px -= detailSize.x;
                if (py < 0)
                    py += detailSize.y;
                else if (py >= detailSize.y)
                    py -= detailSize.y;

                if ((int)px >= 0 && (int)px < details.GetLength(1) && (int)py >= 0 && (int)py < details.GetLength(0) && details[(int)py, (int)px] > 0)
                {
                    vision[i] = distance / maxVision;
                    break;
                }
            }
        }
    }

    public void Setup(CustomTerrain ct, GeneticAlgo ga)
    {
        terrain = ct;
        genetic_algo = ga;
        UpdateSetup();
    }

    private void UpdateSetup()
    {
        detailSize = terrain.detailSize();
        Vector3 gsz = terrain.terrainSize();
        terrainSize = new Vector2(gsz.x, gsz.z);
        details = terrain.getDetails();
    }

    public void InheritBrain(SimpleNeuralNet other, bool mutate)
    {
        brain = new SimpleNeuralNet(other);
        if (mutate)
            brain.mutate(swapRate, mutateRate, swapStrength, mutateStrength);
    }
    public SimpleNeuralNet GetBrain()
    {
        return brain;
    }
    public float GetHealth()
    {
        return energy / maxEnergy;
    }

}
