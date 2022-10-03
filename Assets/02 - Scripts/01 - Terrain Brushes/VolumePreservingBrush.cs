using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class VolumePreservingBrush : TerrainBrush {
    private bool prePressed = false; // previous state of pressing the mouse left button
    private bool isPressing = false; // current state of pressing the mouse left button
    private List<float> heightData = new List<float>();
    public override void onMouseLeftBtnPressed()
    {
        isPressing = true;
    }

    public override void onMouseLeftBtnReleased()
    {
        isPressing = false;
        prePressed = false;
        heightData.Clear();
    }


    public override void draw(int x, int z) {
        if (!prePressed)
        {
            // first pressed
            for (int zi = -radius; zi <= radius; zi++)
            {
                for (int xi = -radius; xi <= radius; xi++)
                {
                    heightData.Add(terrain.get(x + xi, z + zi));
                }
            }
            prePressed = true;
        }
        else
        {
            //TODO: not using the "isPressing", and move should remove the original area, and the data will be applied to the new area.
            //TODO: therefore we need to restore the area as well.
            int i = 0;
            for (int zi = -radius; zi <= radius; zi++)
            {
                for (int xi = -radius; xi <= radius; xi++)
                {
                    terrain.set(x + xi, z + zi, heightData[i]);
                    i++;
                }
            }
        }
    }
}
