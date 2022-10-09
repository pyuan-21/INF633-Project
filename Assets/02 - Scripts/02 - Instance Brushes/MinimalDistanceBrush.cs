using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MinimalDistanceBrush : InstanceBrush
{
    float distance = 5f;

    public override void draw(float x, float z)
    {
        for (float zi = -radius; zi <= radius; zi++)
        {
            for (float xi = -radius; xi <= radius; xi++)
            {

               spawnObject(x + xi+distance, z + zi+distance);


            }
        }
    }
}
