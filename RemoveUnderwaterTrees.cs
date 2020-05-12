// Converted from UnityScript to C# at http://www.M2H.nl/files/js_to_c.php - by Mike Hergaarden

using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;

public class RemoveUnderwaterTrees : MonoBehaviour {

public Terrain terrain;

public float waterLevel = 0.0f;

private TreeInstance[] backupTreeInstances;
private List<TreeInstance> newTreeInstances;

// enum : backup tree data, remove trees below water level, restore tree backup data
public enum allTreeActions 
{
    BackupCurrentTrees,
    RestoreBackupTrees,
    RemoveUnderwaterTrees
}

public allTreeActions performAction;


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
            backupTreeInstances = terrain.terrainData.treeInstances;
            Debug.Log( "Current trees have been Stored in backup data" );
        break;
        
        // restore the backed up trees
        case allTreeActions.RestoreBackupTrees :
            if (backupTreeInstances != null)
            {
                terrain.terrainData.treeInstances = backupTreeInstances;
                Debug.Log( "Trees have been Restored from the backup data" );
            }
            else
            {
                Debug.Log( "NO backup data FOUND ...." );
            }
        break;
                
        // remove trees below water level
        case allTreeActions.RemoveUnderwaterTrees :
        
            Debug.Log( "Removing trees below water level ...." );
            
            // get the width and depth of the terrain
            Vector3 terrainSize = terrain.terrainData.size;
            //Debug.Log( "terrainSize : " + terrainSize );
            
            // get the tree data from the terrain data
            TreeInstance[] treeInstances = terrain.terrainData.treeInstances;
            Debug.Log( "Old : Total Trees = " + treeInstances.Length );
            
            // create a list to store the modified information
            newTreeInstances = new List<TreeInstance>();
            
            // calculate the normalized Water Level
            
            // cycle through each tree
            for ( int t = 0; t < treeInstances.Length; t ++ )
            {
                // check if the tree Y is lower than the water level
                if ( treeInstances[t].position.y * terrain.terrainData.size.y > waterLevel )
                {
                    // if not, add tree to newTreeInstances List
                    newTreeInstances.Add( treeInstances[t] );
                }
            }
            
            // apply newTreeInstances List to terrain data
            terrain.terrainData.treeInstances = new TreeInstance[ newTreeInstances.Count ];
            terrain.terrainData.treeInstances = newTreeInstances.ToArray();
            Debug.Log( "New : Total Trees = " + terrain.terrainData.treeInstances.Length );
            
        break;
    }
}

#endif
}