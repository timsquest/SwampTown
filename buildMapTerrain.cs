using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class buildMapTerrain : MonoBehaviour
{
    private Collider m_Collider;
    Vector3 m_Center;
    float heightOrig;
    public float heightScale = 10f;
    public float widthScale = 10f;
    // Start is called before the first frame update
    void Start()
    {
        //ShapeMesh()
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    /*void ShapeMesh (Tile tileObject)
    {
        Mesh mesh = tileObject.theTile.GetComponent<MeshFilter>().mesh;
        Vector3[] vertices = mesh.vertices;
        for(int v = 0; v < vertices.Length; v++)
        {
            vertices[v].y = Mathf.PerlinNoise((vertices[v].x + tileObject.theTile.transform.position.x)/detailScale,(vertices[v].z + tileObject.theTile.transform.position.z)/detailScale)*heightScale;

            if(vertices[v].y > 4.25)
            {
                GameObject newTree = tree_pool.getTree();
                if(newTree != null)
                {
                    Vector3 treePos = new Vector3(vertices[v].x + tileObject.theTile.transform.position.x, vertices[v].y - .7f, vertices[v].z + tileObject.theTile.transform.position.z);
                    if (Mathf.Abs(treePos.x) > halfTilesX*planeSize/2 || Mathf.Abs(treePos.z) > halfTilesZ*planeSize/2)
                    {
                        newTree.transform.position = treePos;
                        newTree.transform.localScale = new Vector3 (2.5f,1.5f,2.5f);
                        newTree.SetActive(true);
                        tileObject.theTrees.Add(newTree);
                    }
                }
            }
        }

        mesh.vertices = vertices;
        mesh.RecalculateBounds();
        mesh.RecalculateNormals();
        MeshCollider meshCollider = tileObject.theTile.gameObject.AddComponent<MeshCollider>();
        meshCollider.material = new PhysicMaterial("Wood");
    }*/
}
