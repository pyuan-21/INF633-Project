using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShapeInstanceBrush : InstanceBrush
{
    public enum ListOfShape { BASIC, CIRCLE, SQUARE, GRID };
    public ListOfShape shapeType;
    public float max_height = 20f;
    public  float max_angle = 1f;
     

    public GameObject[] treesobjects = new GameObject[3];



  
    public override void draw(float x, float z)
    {

        

          // altitude
        float height = terrain.get(x, z);
        if (height <= max_height/4) terrain.object_prefab = treesobjects[0];
        if (height <= max_height/2 && height > max_height/4) terrain.object_prefab = treesobjects[1];
        if (height <= max_height && height > max_height/2) terrain.object_prefab = treesobjects[2];


        print(height+"|"+terrain.object_prefab);

        // steepness
        if (terrain.getSteepness(x, z) >max_angle) return;

        
        if (shapeType == ListOfShape.BASIC) spawnObject(x, z);


        if (shapeType == ListOfShape.CIRCLE)
        {
            float r = Random.Range(0f, radius);
            float angle = Random.Range(0f, 2 * Mathf.PI);
            spawnObject(x + r * Mathf.Cos(angle), z + r * Mathf.Sin(angle));
        }

        if (shapeType == ListOfShape.SQUARE)
        {
            float x1 = Random.Range(-radius, radius);
            float z1 = Random.Range(-radius, radius);
            spawnObject(x + x1, z + z1);
        }

        if (shapeType == ListOfShape.GRID)
        {
            for (float dz = -radius;  dz<= radius; dz++)
            {
                for (float dx = -radius; dx <= radius;  dx++)
                {
                        spawnObject(x + dx, z + dx);

                }
            }
        }






    }
}

