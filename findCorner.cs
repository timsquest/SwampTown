using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class findCorner : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        Terrain terrain = Terrain.activeTerrain;
        float[,] heights = terrain.terrainData.GetHeights(0,0,50,100);
        for (int x = 0; x < 50; x++)
        {
            for (int y = 0; y < 100; y++)
            {
                heights[y,x] = 1;
            }
        }
        terrain.terrainData.SetHeights(0,0,heights);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
