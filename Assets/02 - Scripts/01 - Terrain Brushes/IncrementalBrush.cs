using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IncrementalBrush : TerrainBrush
{
    public float sigma = 1f;
    public float height = 5;

    // incremental gaussian
    public override void draw(int x, int z)
    {
        for (int zi = -radius; zi <= radius; zi++)
        {
            for (int xi = -radius; xi <= radius; xi++)
            {
               
                float terrainheight = terrain.get(x + xi, z + zi);
                terrain.set(x + xi, z + zi, terrainheight + 1);

                //System.Console.WriteLine(height * gaussian);

            }
        }


    }
}
