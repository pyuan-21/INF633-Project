using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestUpdateGrass : MonoBehaviour
{
    public CustomTerrain terrain;
    private Transform tfm;
    private int[,] details = null;
    private Vector2 detailSize;
    private Vector2 terrainSize;
    // Start is called before the first frame update
    void Start()
    {
        tfm = transform;

        detailSize = terrain.detailSize();
        Vector3 gsz = terrain.terrainSize();
        terrainSize = new Vector2(gsz.x, gsz.z);
    }

    // Update is called once per frame
    void Update()
    {
        detailSize = terrain.detailSize();
        Vector3 gsz = terrain.terrainSize();
        terrainSize = new Vector2(gsz.x, gsz.z);
        details = terrain.getDetails();

        int dx = (int)((tfm.position.x / terrainSize.x) * detailSize.x);
        int dy = (int)((tfm.position.z / terrainSize.y) * detailSize.y);
        Debug.Log("dx: " + dx + ", dy: " + dy);
        if ((dx >= 0) && dx < (details.GetLength(1)) && (dy >= 0) && (dy < details.GetLength(0)) && details[dy, dx] > 0)
        {
            // Eat (remove) the grass and gain energy.
            details[dy, dx] = 0;
            terrain.UpdateDetail(dy, dx, 0); // update grass of terrain
            Debug.Log("Eat grass");
        }
    }
}
