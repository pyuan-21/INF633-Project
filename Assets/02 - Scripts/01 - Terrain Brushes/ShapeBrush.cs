using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum BrushShape
{
    SQUARE,
    CIRCLE,
}
public class ShapeBrush : TerrainBrush {
    public bool fixedHeight = true;
    public float incrementalHeight = 5;
    public BrushShape shapeType = BrushShape.SQUARE;
    public override void draw(int x, int z) {
        float radiusSquare = radius * radius;

        for (int zi = -radius; zi <= radius; zi++) {
            for (int xi = -radius; xi <= radius; xi++) {
                if(shapeType == BrushShape.CIRCLE)
                {
                    if ((xi * xi + zi * zi) > radiusSquare)
                        continue; // not inside the circle
                }
                if (fixedHeight)
                {
                    terrain.set(x + xi, z + zi, incrementalHeight);
                }
                else
                {
                    float height = terrain.get(x + xi, z + zi);
                    height += incrementalHeight;
                    terrain.set(x + xi, z + zi, height);
                }
            }
        }
    }
}
