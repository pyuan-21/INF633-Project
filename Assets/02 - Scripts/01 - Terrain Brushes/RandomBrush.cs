using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RandomBrush : TerrainBrush {
    public float minHeight = 2;
    public float maxHeight = 5;
    public override void draw(int x, int z) {
        for (int zi = -radius; zi <= radius; zi++) {
            for (int xi = -radius; xi <= radius; xi++) {
                float height = Random.Range(minHeight, maxHeight);
                height += terrain.get(x + xi, z + zi);
                terrain.set(x + xi, z + zi, height);
            }
        }
    }
}
