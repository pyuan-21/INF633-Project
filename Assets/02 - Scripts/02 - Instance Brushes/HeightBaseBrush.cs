using System.Collections;
using System.Collections.Generic;
using UnityEngine;



public class HeightBaseBrush : InstanceBrush
{



    public GameObject[] treesobjects = new GameObject[3];
    public float max_height = 20f;
    public float max_angle = 1f;

    public override void draw(float x, float z)
    {
        
        // altitude
        float height = terrain.get(x, z);
        if (height <= max_height / 4 && treesobjects[0]!=null) terrain.object_prefab = treesobjects[0];
        if (height <= max_height / 2 && height > max_height / 4 && treesobjects[0] != null) terrain.object_prefab = treesobjects[1];
        if (height <= max_height && height > max_height / 2 && treesobjects[0] != null) terrain.object_prefab = treesobjects[2];


        print(height + "|" + terrain.object_prefab);

        // steepness
        if (terrain.getSteepness(x, z) < max_angle)  spawnObject(x, z);
    }
}

