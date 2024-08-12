using System.Collections;
using System.Collections.Generic;
using Unity.AI.Navigation;
using UnityEngine;

[ExecuteInEditMode]
public class TreePlanter : MonoBehaviour
{
    BoxCollider col;
    public GameObject[] trees;
    public int numberOfTrees;
    public int density;
    public LayerMask mask;
    public Transform treeParent;
    public float minDistanceBetweenTrees;
    List<Vector3> placedTreePositions = new List<Vector3>();
    public float raycastYHeight;


    // Start is called before the first frame update
    void Start()
    {



    }

    public void ClearTrees()
    {
        for (int i = treeParent.childCount - 1; i >= 0; i--)
        {
            DestroyImmediate(treeParent.GetChild(i).gameObject);
        }

        placedTreePositions.Clear();
    }


    public void GenerateTrees()
    {
        if (treeParent.childCount > 0)
        {
            ClearTrees();
        }

        col = GetComponent<BoxCollider>();
        for (int i = 0; i < numberOfTrees; i++)
        {
            bool validPosition = false;
            Vector3 point = Vector3.zero;

            while (!validPosition)
            {
                int xVal = Mathf.RoundToInt(Random.Range(col.bounds.min.x, col.bounds.max.x));
                int zVal = Mathf.RoundToInt(Random.Range(col.bounds.min.z, col.bounds.max.z));
                point = new Vector3(RoundToMultiple(xVal), raycastYHeight, RoundToMultiple(zVal));

                if (Physics.Raycast(point, -Vector3.up, out RaycastHit hit, raycastYHeight, mask))
                {
                    if (hit.collider.GetComponent<NavMeshSurface>())
                    {
                        point = hit.point;

                        if (IsPositionValid(point))
                        {
                            validPosition = true;
                        }
                    }
                }
            }

            // Instantiate the tree once a valid position is found
            int randomTree = Random.Range(0, trees.Length);
            GameObject tree = Instantiate(trees[randomTree], point, Quaternion.identity);
            tree.transform.SetParent(treeParent);
            placedTreePositions.Add(point); // Add the position to the list
        }
    }

    private bool IsPositionValid(Vector3 position)
    {
        foreach (Vector3 treePosition in placedTreePositions)
        {
            if (Vector3.Distance(position, treePosition) < minDistanceBetweenTrees)
            {
                return false;
            }
        }
        return true;
    }

    int RoundToMultiple(int value)
    {
        return (value / density) * density;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
