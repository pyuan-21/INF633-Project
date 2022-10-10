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
    public shape shapeType;
    public override void draw(float x, float z)
    {
        if (shapeType == shape.CIRCLE)
        {

            float r = Random.Range(0f, radius);
            float angle = Random.Range(0f, 2 * Mathf.PI);
            Debug.Log(terrain.getSteepness(x,z));
            spawnObject(x + r * Mathf.Cos(angle), z + r * Mathf.Sin(angle));
        }
        else
        {
            float x1 = Random.Range(-radius, radius);
            float z1 = Random.Range(-radius, radius);
            spawnObject(x + x1, z + z1);
        }
    }
}