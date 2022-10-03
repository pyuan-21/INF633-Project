using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GaussianBrush : TerrainBrush {

    public bool isIncreasing = true;
    public float maxHeight = 5;
    public float varianceX = 10;
    public float varianceY = 10;
    private float Gaussian2D(float x, float y)
    {
        return maxHeight * Mathf.Exp(-x * x * 0.5f / (varianceX * varianceX) - y * y * 0.5f / (varianceY * varianceY));
    }

    public override void draw(int x, int z) {
        for (int zi = -radius; zi <= radius; zi++) {
            for (int xi = -radius; xi <= radius; xi++) {
                float height = terrain.get(x + xi, z + zi);
                if (isIncreasing)
                    height += Mathf.Max(Gaussian2D(xi, zi), 0);
                else
                    height -= Mathf.Max(Gaussian2D(xi, zi), 0);
                terrain.set(x + xi, z + zi, height);
            }
        }
    }
}
