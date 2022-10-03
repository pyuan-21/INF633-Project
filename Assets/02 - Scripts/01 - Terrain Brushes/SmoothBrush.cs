using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class SmoothBrush : TerrainBrush {
    public int kernelType = 0; // TODO: create two gaussian kernel to do the convolution
    public override void draw(int x, int z) {

        for (int zi = -radius; zi <= radius; zi++) {
            for (int xi = -radius; xi <= radius; xi++) {
                //float height = // TODO: get this grid height by using the convolution result
                //terrain.set(x + xi, z + zi, height);
                
            }
        }
    }
}
