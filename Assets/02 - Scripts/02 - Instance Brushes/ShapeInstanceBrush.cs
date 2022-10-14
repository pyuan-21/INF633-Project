using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShapeInstanceBrush : InstanceBrush
{
    public float mindistance = 15f;
    public GameObject[] treesobjects = new GameObject[3];

    public bool checkMinDistance(float x,float z)
    {
        float distance = float.MaxValue;
        var n = terrain.getObjectCount();
        for (int i = 0; i < n; i++)
        {
            Vector3 objLoc = terrain.getObjectLoc(i);
            // distance to nearest instances
            distance = Mathf.Min(distance, Mathf.Sqrt((objLoc.x - x) * (objLoc.x - x) + (objLoc.z - z) * (objLoc.z - z)));
        }

        //print(distance);

        if (distance < mindistance) return false;
        else return true;
    }

  
    public override void draw(float x, float z)
    {
    

        // altitude
        float height = terrain.get(x, z);
        if (height <= 20f && height > 10f) terrain.object_prefab = treesobjects[2];
        if (height <= 10f && height > 5f) terrain.object_prefab = treesobjects[1];
        if (height <= 5f) terrain.object_prefab = treesobjects[0];
        print(height + "|" + terrain.object_prefab);
        // steepness
        if (1f < terrain.getSteepness(x, z)) return;


        if (checkMinDistance(x, z)) spawnObject(x, z);
        
    }
}

