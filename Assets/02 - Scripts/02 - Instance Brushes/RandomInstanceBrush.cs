using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum shape
{
    SQUARE,
    CIRCLE,
}

public class RandomInstanceBrush : InstanceBrush
{
   
    public override void draw(float x, float z)
    {
 
            float x1 = Random.Range(-radius, radius);
            float z1 = Random.Range(-radius, radius);
            spawnObject(x + x1, z + z1);
        
    }
}