using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GridBrush : InstanceBrush
{
    float grid_size = 10f;

    public override void draw(float x, float z)
    {
        for (float zi = -radius; zi <= radius; zi += grid_size)
        {
            for (float xi = -radius; xi <= radius; xi += grid_size)
            {
           
                    spawnObject(x + xi, z + zi);
                

            }
        }
    }
}