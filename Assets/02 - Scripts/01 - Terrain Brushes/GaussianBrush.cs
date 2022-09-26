using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GaussianBrush : TerrainBrush {

    public float maxHeight = 5;
    public float varianceX = 1;
    public float varianceY = 1;
    private float Gaussian2D(float x, float y)
    {
        return maxHeight * Mathf.Exp(-x * x * 0.5f / (varianceX * varianceX) - y * y * 0.5f / (varianceY * varianceY));
    }

    public override void draw(int x, int z) {
        for (int zi = -radius; zi <= radius; zi++) {
            for (int xi = -radius; xi <= radius; xi++) {
                terrain.set(x + xi, z + zi, Gaussian2D(xi, zi));
            }
        }
    }
}
