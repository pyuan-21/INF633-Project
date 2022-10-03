using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PerlinNoiseBrush : TerrainBrush
{
   
    public float height = 5;

    // perlin noise
    public override void draw(int x, int z)
    {
        for (int zi = -radius; zi <= radius; zi++)
        {
            for (int xi = -radius; xi <= radius; xi++)
            {
                float perlinnoise = Mathf.PerlinNoise(xi, zi);
                float terrainheight = terrain.get(x + xi, z + zi);

                terrain.set(x + xi, z + zi, perlinnoise);

                //System.Console.WriteLine(height * gaussian);

            }
        }


    }
}

