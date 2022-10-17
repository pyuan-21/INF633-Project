using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Responder : MonoBehaviour
{
    public float maxRotatingAngle = 10.0f;
    public Transform referGoal;
    public float maxMovingStraightDis = 10;

    private Transform currentGoal;
    private float keepRotatingAngle = 0;
    private float keepMovingDistance = 0;
    private bool isStopMoving;
    private bool isWaiting;
    private float waitingTime;

    private Animal animal;
    private Terrain terrain;
    protected CustomTerrain customTerrain;
    private float mapWidth;
    private float mapHeight;

    public float mapRange = 10f;

    public float maxSteepAngle = 20f;
    [Range(0f, 1f)]
    public float searchFoodProbability = 0.3f;
    [Range(0f, 1f)]
    public float waitingProbability = 0.3f;
    [Range(0f, 5f)]
    public float waitingTimeMin = 0.5f;
    [Range(0f, 5f)]
    public float waitingTimeMax = 1.0f;
    [Range(0f, 360f)]
    public float casualRotatingAngle = 60f;

    private bool isObserver = false;

    private void Test()
    {
        var goal = gameObject.GetComponent<QuadrupedProceduralMotion>().goal;
        if (goal == null)
            return;
        if(customTerrain==null)
        {
            GameObject terrainObj = GameObject.Find("Terrain");
            customTerrain = terrainObj.GetComponent<CustomTerrain>();
        }
        Vector3 worldPos = goal.position;
        Vector3 gridPos = customTerrain.world2grid(worldPos.x, worldPos.z);
        Debug.Log("Steepness: " + customTerrain.getSteepness(gridPos.x, gridPos.z));
        Debug.Log("maxSteepAngle: " + maxSteepAngle);
    }

    private void Update()
    {
        if (isWaiting)
        {
            waitingTime -= Time.deltaTime;
            if (waitingTime <= 0)
                isWaiting = false;
        }

        //Test(); // pyuan
    }

    public void Init()
    {
        isStopMoving = false;
        isWaiting = false;

        animal = transform.GetComponent<Animal>();

        GameObject goalParent = GameObject.Find("Goals");
        GameObject obj = new GameObject(gameObject.name + "_CurrentGoal_" + animal.id);
        obj.transform.SetParent(goalParent.transform);
        currentGoal = obj.transform;
        currentGoal.position = referGoal.position;
        GetComponent<QuadrupedProceduralMotion>().goal = currentGoal;

        // create four foot targets
        CreateFootTargets();


        var testCam = GameObject.Find("TestCamera");
        // just test codes:
        if (testCam != null && testCam.activeSelf && testCam.transform.parent == null)
        {
            // we only bind TestCamera to one creatures
            Vector3 pos = testCam.transform.position;
            Quaternion rot = testCam.transform.rotation;
            testCam.transform.SetParent(transform);
            testCam.transform.localPosition = pos;
            testCam.transform.localRotation = rot;
            isObserver = true;
        }

        // Retrieve terrain.
        terrain = Terrain.activeTerrain;
        GameObject terrainObj = GameObject.Find("Terrain");
        customTerrain = terrainObj.GetComponent<CustomTerrain>();
        mapWidth = terrain.terrainData.size.x;
        mapHeight = terrain.terrainData.size.z;

        mapRange = MathF.Max(mapRange, 1);
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
            obj.name = foots[i].gameObject.name + "_animal_" + animal.id;
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
        while (objs.Count > 0)
        {
            GameObject current = objs[objs.Count - 1];
            objs.RemoveAt(objs.Count - 1);

            if (current.name == name)
                return current;
            else
            {
                if (current.transform.childCount > 0)
                {
                    for (int i = 0; i < current.transform.childCount; i++)
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

    public void ResetRotatingAngle()
    {
        keepRotatingAngle = 0;// first to give a chance to rotate
    }

    public void StopMoving()
    {
        isStopMoving = true;
    }

    public bool IsMoving()
    {
        var qpm = GetComponent<QuadrupedProceduralMotion>();
        return qpm.frontLeftFoot.Moving || qpm.frontRightFoot.Moving || qpm.backLeftFoot.Moving || qpm.backRightFoot.Moving;
    }

    public bool IsWaiting()
    {
        return isWaiting;
    }


    private void SearchFoodReaction(float result)
    {
        Debug.Log("Result: " + result);
        // vision is almost 0 if food is near engouh to animal, then its value into the network and output is almost 0.5
        // then result is 0.5*2-1=0. almost 0
        bool condition = MathF.Abs(result) <= 0.1f || keepMovingDistance < maxMovingStraightDis || Mathf.Abs(keepRotatingAngle) >= 360; // it tells animal should keep moving straight?
        bool forceRotate = false;
        if (condition)
        {
            if (MoveStraight())
            {
                if (Mathf.Abs(keepRotatingAngle) >= 360)
                    keepRotatingAngle = 0;
            }
            else
                forceRotate = true;
        }

        if(!condition || forceRotate)
        {
            if (keepMovingDistance >= maxMovingStraightDis)
                keepMovingDistance = 0; // give the chane next time to go

            // turn around
            // it seems food is not in animal's visual area
            // we have two options, rotate or keep moving straight
            // we can simple rotate
            float angle = result * maxRotatingAngle;
            TurnAround(angle);
        }
    }

    private float GetGround()
    {
        float ground = 0f;
        Vector3 raycastOrigin = transform.position + Vector3.up * 99999;
        RaycastHit[] hitInfos = Physics.RaycastAll(raycastOrigin, Vector3.down, Mathf.Infinity);
        if (hitInfos.Length > 0)
        {
            for (int i = 0; i < hitInfos.Length; i++)
            {
                if (hitInfos[i].transform.CompareTag("Ground") || hitInfos[i].transform.gameObject.layer == LayerMask.NameToLayer("Ground"))
                {
                    ground = hitInfos[i].point.y;
                    break;
                }
            }
        }
        return ground;
    }

    private bool MoveStraight()
    {
        // go straight 
        int iterCount = 20;
        float maxOffset = maxMovingStraightDis;
        float delataOffset = maxOffset / iterCount;
        for(float currentOffset = maxOffset; currentOffset > 0; currentOffset -= delataOffset)
        {
            Vector3 offset = (transform.rotation * Vector3.forward).normalized * currentOffset;
            Vector3 newPos = currentGoal.position + offset;
            if (IsValidPos(newPos))
            {
                // update Height
                float ground = GetGround();
                currentGoal.position = new Vector3(newPos.x, ground, newPos.z);
                keepMovingDistance += currentOffset;
                Debug.Log("MoveStraight: " + currentGoal.position);
                return true;
            }
        }
        return false;
    }

    private void TurnAround(float angle)
    {
        Vector3 dir = currentGoal.position - transform.position;
        float distance = dir.magnitude;
        dir = dir.normalized;
        Vector3 newDir = Quaternion.AngleAxis(angle, Vector3.up) * dir;
        Vector3 newPos = transform.position + newDir * distance;
        float ground = GetGround();
        currentGoal.position = new Vector3(newPos.x, ground, newPos.z);
        keepRotatingAngle += Mathf.Abs(angle);
        Debug.Log("TurnAround: " + currentGoal.position);
    }

    private bool IsValidPos(Vector3 worldPos)
    {
        // check whether it's inside the terrain
        if (worldPos.x <= mapRange || worldPos.x >= mapWidth - mapRange)
            return false;
        if (worldPos.z <= mapRange || worldPos.z >= mapHeight - mapRange)
            return false;

        // check steepness
        Vector3 gridPos = customTerrain.world2grid(worldPos.x, worldPos.z);
        if (customTerrain.getSteepness(gridPos.x,gridPos.z) >= maxSteepAngle)
            return false;

        // Maybe it should to check height difference between current position and next position? --> ignore it, I do sth. in the QuadraupedProceduralMotion instead
        return true;
    }

    public void Reaction(float result)
    {
        if (isStopMoving)
            return;
        // make decision here:
        // 1. if health is lower than 30%, it should concentrate on searching food (or 30% probability to find food when the health is higgher then 30%)
        // 2. else, if can wander casually
        if (animal.GetHealth() < 0.3f || UnityEngine.Random.value <= searchFoodProbability)
        {
            SearchFoodReaction(result);
        }
        else
        {
            // randomly choose to wait or turan around
            if(UnityEngine.Random.value <= waitingProbability)
            {
                // wait
                isWaiting = true;
                if (waitingTimeMax < waitingTimeMin)
                {
                    float temp = waitingTimeMax;
                    waitingTimeMax = waitingTimeMin;
                    waitingTimeMin = temp;
                }
                waitingTime = UnityEngine.Random.Range(waitingTimeMin, waitingTimeMax);
                Debug.Log("Waiting: " + waitingTime);
            }
            else
            {
                // turn around
                float angle = UnityEngine.Random.Range(-casualRotatingAngle, casualRotatingAngle);
                TurnAround(angle);
            }
        }
    }

    public bool IsObserver()
    {
        return isObserver;
    }
}
