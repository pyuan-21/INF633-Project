using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class SmoothBrush : TerrainBrush {
    private float[] gaussianKernel = {
        1.0f/16, 2.0f/16, 1.0f/16,
        2.0f/16, 4.0f/16, 2.0f/16,
        1.0f/16, 2.0f/16, 1.0f/16,
    };
    public override void draw(int x, int z) {
        int minX = x - radius;
        int maxX = x + radius;
        int minZ = z - radius;
        int maxZ = z + radius;
        for (int zi = -radius; zi <= radius; zi++) {
            for (int xi = -radius; xi <= radius; xi++) {
                // convolution
                int centerX = x + xi;
                int centerZ = z + zi;
                float height = 0.0f;
                for(int i = -1; i <= 1; i++)
                {
                    for(int j = -1; j <= 1; j++)
                    {
                        int curX = Mathf.Clamp(centerX+i, minX, maxX);
                        int curY = Mathf.Clamp(centerZ+j, minZ, maxZ);
                        height += gaussianKernel[i + 1 + (j + 1) * 3] * terrain.get(curX, curY);
                    }
                }
                terrain.set(x + xi, z + zi, height);

            }
        }
    }
}
