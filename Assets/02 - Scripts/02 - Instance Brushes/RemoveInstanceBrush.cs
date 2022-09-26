using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RemoveInstanceBrush : InstanceBrush {

    public override void draw(float x, float z) {
        terrain.RemoveTreeInstance(x, z, radius);
    }
}
