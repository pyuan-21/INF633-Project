using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GaussianBrush : TerrainBrush
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
                float firstPart = (float)Mathf.Exp(-((xi * xi) + (zi * zi)) / (2 * sigma * sigma));
                float secondPart = (float)1 / Mathf.Sqrt(2 * Mathf.PI * sigma * sigma);
                float gaussian = firstPart * secondPart;
                float terrainheight = terrain.get(x + xi, z + zi);

                terrain.set(x + xi, z + zi,terrainheight+ gaussian);

                //System.Console.WriteLine(height * gaussian);

            }
        }


    }
}
