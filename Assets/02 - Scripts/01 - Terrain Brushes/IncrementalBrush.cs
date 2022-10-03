using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IncrementalBrush : TerrainBrush {
    public bool isIncreasing = true;
    public float incrementalHeight = 5;
    public override void draw(int x, int z) {
        for (int zi = -radius; zi <= radius; zi++) {
            for (int xi = -radius; xi <= radius; xi++) {
                float height = terrain.get(x + xi, z + zi);
                height += incrementalHeight;
                terrain.set(x + xi, z + zi, height);
            }
        }
    }
}
