using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NoiseBrush : TerrainBrush {

    public float maxHeight = 5;
    public override void draw(int x, int z) {
        for (int zi = -radius; zi <= radius; zi++) {
            for (int xi = -radius; xi <= radius; xi++) {
                float height = PerlinNoise(x + xi, z + zi) * maxHeight + terrain.get(x + xi, z + zi);
                //Debug.Log("(x:" + (x + xi) + ", z:" + (z + zi) + "), heigh:" + height);
                terrain.set(x + xi, z + zi, height);
            }
        }
    }

    public int N = 9;
    public float alpha = 0.4f;
    public float h = 0.3f;
    public float s = 1;
    public float o = 0;
    private float SmoothRandomFunction(float u, float v)
    {
        return Random.Range(0, u) * Random.Range(0, v) / (u * v);
    }

    private float PerlinNoise(float x, float z)
    {
        float sum = 0.0f;
        for(int k = 0; k < N; k++)
        {
            float ratio = Mathf.Pow(2, k);
            sum += Mathf.Pow(alpha, k) * SmoothRandomFunction(ratio * x, ratio * z);
        }
        return sum;
    }
}
