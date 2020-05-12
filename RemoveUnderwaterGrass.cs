// Converted from UnityScript to C# at http://www.M2H.nl/files/js_to_c.php - by Mike Hergaarden

using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;

public class RemoveUnderwaterGrass : MonoBehaviour {

public Terrain terrain;

public float waterLevel = 0.0f;

// enum : backup tree data, remove trees below water level, restore tree backup data
public enum allTreeActions 
{
    BackupCurrentTrees,
    RestoreBackupTrees,
    RemoveUnderwaterTrees
}

public allTreeActions performAction;
private int[,] detailMap;

#if UNITY_EDITOR

[ContextMenu("Modify Tree Data")]

void  ModifyTreeData (){
    // check if there is no terrain in the inspector, then use the current active terrain
    if ( !terrain )
    {
        terrain = Terrain.activeTerrain;
    }
    
    switch( performAction )
    {
        // backup the current trees
        case allTreeActions.BackupCurrentTrees :
            // Get all of layer zero of detailed objects
            detailMap = terrain.terrainData.GetDetailLayer(0, 0, terrain.terrainData.detailWidth, terrain.terrainData.detailHeight, 0);
            Debug.Log( "Current grass have been stored in backup data" );
        break;
        
        // restore the backed up trees
        case allTreeActions.RestoreBackupTrees :
            if (detailMap != null)
            {
                terrain.terrainData.SetDetailLayer(0, 0, 0, detailMap);
                Debug.Log( "Trees have been Restored from the backup data" );
            }
            else
            {
                Debug.Log( "NO backup data FOUND ...." );
            }
        break;
                
        // remove trees below water level
        case allTreeActions.RemoveUnderwaterTrees :
            // For each pixel in the detail map...
            int[,] newDetailMap = detailMap;
            for (var y = 0; y < terrain.terrainData.detailHeight; y++)
            {
                for (var x = 0; x < terrain.terrainData.detailWidth; x++)
                {
                   newDetailMap[x, y] = 0;
                }
            }

            // Assign the modified map back.
            terrain.terrainData.SetDetailLayer(0, 0, 0, newDetailMap);
        break;
    }
}

#endif
}