using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShapeInstanceBrush : InstanceBrush
{
    public enum ListOfShape { BASIC, CIRCLE, SQUARE, GRID };
    public ListOfShape shapeType;
    public float shape_size = 10f;
    public float max_height = 20f;
    public  float max_angle = 1f;
     

    public GameObject[] treesobjects = new GameObject[3];



  
    public override void draw(float x, float z)
    {



      
        // height constraint
        float height = terrain.get(x, z);
        if (height <= max_height / 4 && treesobjects[0] != null) terrain.object_prefab = treesobjects[0];
        if (height <= max_height / 2 && height > max_height / 4 && treesobjects[1] != null) terrain.object_prefab = treesobjects[1];
        if (height <= max_height && height > max_height / 2 && treesobjects[2] != null) terrain.object_prefab = treesobjects[2];



        print(height+"|"+terrain.object_prefab);

        // steepness
        if (terrain.getSteepness(x, z) >max_angle) return;

        
        if (shapeType == ListOfShape.BASIC) spawnObject(x, z);


        if (shapeType == ListOfShape.CIRCLE)
        {
           
            float angle = Random.Range(0f, 2 * Mathf.PI);
            for (float dz = -radius; dz <= radius; dz += shape_size)
            {
                for (float dx = -radius; dx <= radius; dx += 10)
                {
                    spawnObject(x + radius* Mathf.Sin(angle), z + radius* Mathf.Cos(angle));

                }
            }
        }

        if (shapeType == ListOfShape.SQUARE)
        {

            spawnObject(x , z );
            spawnObject(x + radius, z );
            spawnObject(x , z + radius);
            spawnObject(x + radius, z + radius);
        }

        if (shapeType == ListOfShape.GRID)
        {
         for (float dz = -radius; dz <= radius; dz += shape_size)
        {
            for (float dx = -radius; dx <= radius; dx += shape_size)
            {
                spawnObject(x + dx, z + dz);

            }
        }
    }
}






}


