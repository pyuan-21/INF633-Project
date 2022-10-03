using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class VolumePreservingBrush : TerrainBrush {
    public bool isRecord = true;
    private Dictionary<System.ValueTuple<int, int>, float> recordHeightData = new Dictionary<(int, int), float>();
    private int lastRadius = 0;
    private List<System.ValueTuple<int, int>> lastAreaInfo = new List<(int, int)>();
    public override void draw(int x, int z) {
        if (isRecord)
        {
            recordHeightData.Clear();
            lastAreaInfo.Clear();

            for (int zi = -radius; zi <= radius; zi++)
            {
                for (int xi = -radius; xi <= radius; xi++)
                {
                    recordHeightData.Add(new(xi, zi), terrain.get(x + xi, z + zi));
                    lastAreaInfo.Add(new(x + xi, z + zi));
                }
            }
            lastRadius = radius;
        }
        else
        {
            // first clear the last area, then move it to new area
            foreach(var e in lastAreaInfo)
            {
                terrain.set(e.Item1, e.Item2, 0);
            }
            lastAreaInfo.Clear();

            // copy the data to the destination area
            for (int zi = -radius; zi <= radius; zi++)
            {
                if (zi < -lastRadius || zi > lastRadius)
                    continue; // out of previous data's range
                for (int xi = -radius; xi <= radius; xi++)
                {
                    if (xi < -lastRadius || xi > lastRadius)
                        continue; // out of previous data's range

                    terrain.set(x + xi, z + zi, recordHeightData[new(xi, zi)]);
                    lastAreaInfo.Add(new(x + xi, z + zi));
                }
            }
        }
    }
}
