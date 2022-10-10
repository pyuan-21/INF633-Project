using System.Collections;
using System.Collections.Generic;
using UnityEngine;



public class BaseInstanceBrush :InstanceBrush
{


  
    public GameObject[] treesobjects = new GameObject[2];

    public override void draw(float x, float z)
{
        float height = terrain.get(x, z);

       

        // steepness
        if (1f < terrain.getSteepness(x, z)){
            return;
        }

        // altitude
        if (height<=20f && height>10f){
            terrain.object_prefab = treesobjects[0];
            


        }
        if (height <=10f && height>5f){
            terrain.object_prefab = treesobjects[1];
            
        }
        if (height <= 5f) {
            terrain.object_prefab = treesobjects[1];
            
        }
       

        //Instantiate(terrain.object_prefab);
        spawnObject(x, z);
    }
}

