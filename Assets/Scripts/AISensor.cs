using JetBrains.Annotations;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[ExecuteInEditMode]
public class AISensor : MonoBehaviour
{
    public float distance;
    public float angle;
    public float height;
    public float rayStartHeight;
    float normalRayStartHeight;
    public Color meshColour = Color.red;

    public int scanFrequency;
    public LayerMask layers;
    public LayerMask occlusionLayers;
    bool isInSmoke;

    public List<GameObject> objects = new List<GameObject>();

    Collider[] colliders = new Collider[50];

    int count;
    float scanInterval;
    float scanTimer;

    Mesh mesh;
    // Start is called before the first frame update
    void Start()
    {
        normalRayStartHeight = rayStartHeight;
        scanInterval = 1.0f / scanFrequency;

    }

    // Update is called once per frame
    void Update()
    {
        scanTimer -= Time.deltaTime;
        if(scanTimer < 0)
        {
            scanTimer += scanInterval;
            Scan();
        }
    }

    void Scan()
    {
        if (!isInSmoke)
        {
            count = Physics.OverlapSphereNonAlloc(transform.position, distance, colliders, layers, QueryTriggerInteraction.Collide);

            objects.Clear();
            for (int i = 0; i < count; i++)
            {
                GameObject obj = colliders[i].gameObject;
                if (IsInSight(obj))
                {
                    objects.Add(obj);
                    if (GetComponent<ZombiePatrolAI>())
                    {
                        GetComponent<ZombiePatrolAI>().AlertBrain(obj);
                        //if(MLPatrol2.Instance != null)
                        //{
                        //    MLPatrol2.Instance.TakeAction();
                        //}
                    }
                    else if (GetComponent<EnforcerZombieAI>())
                    {
                        GetComponent<EnforcerZombieAI>().SpottedPlayer(obj.transform);
                    }

                }

                //if (!IsInSight(obj))
                //{
                //    if (GetComponent<ZombiePatrolAI>())
                //    {
                //        GetComponent<ZombiePatrolAI>().playerSpotted = false;
                //        if (obj.GetComponent<RigidbodyMovement>() && obj.GetComponent<RigidbodyMovement>().mlIdentifier != null)
                //        {
                //            obj.GetComponent<RigidbodyMovement>().mlIdentifier.SetActive(false);
                //        }
                //    }
                //}
            }
        }




    }

    private void OnTriggerStay(Collider other)
    {
        if (other.CompareTag("smoke"))
        {
            isInSmoke = true;
        }
    }


    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("smoke"))
        {
            isInSmoke = false;
        }
    }

    public bool IsInSight(GameObject obj)
    {

        if (isInSmoke)
        {
            return false;
        }

        Vector3 origin = transform.position;
        Vector3 dest = obj.transform.position;
        Vector3 dir = dest - origin;
        if(dir.y < 0 || dir.y > height)
        {
            return false;
        }

        dir.y = 0;
        float deltaAngle = Vector3.Angle(dir, transform.forward);
        if(deltaAngle > angle)
        {
            return false;
        }
        if(RigidbodyMovement.Instance != null)
        {
            if (RigidbodyMovement.Instance.isCrouched)
            {
                rayStartHeight = normalRayStartHeight * 2;
            }
            else
            {
                rayStartHeight = normalRayStartHeight;
            }
        }


        origin.y += height / rayStartHeight;
        dest.y = origin.y;
        

        if(Physics.Linecast(origin, dest, occlusionLayers))
        {
            return false;
        }

        //if(Physics.Raycast(origin, dir, out RaycastHit hit ,Vector3.Distance(dest, origin)))
        //{
        //    if(hit.collider.gameObject != obj)
        //    {
        //        Debug.Log("???");
        //        return false;
        //    }
        //    else
        //    {
        //        return true;
        //    }
        //}

        return true;
    }

    Mesh CreateWedgeMesh()
    {

        int segments = 10;

        Mesh mesh = new Mesh();
        int numTriangles =(segments * 4) + 2 + 2;
        int numVertices = numTriangles * 3;

        Vector3[] vertices = new Vector3[numVertices];
        int[] triangles = new int[numVertices];

        Vector3 bottomCenter = Vector3.zero;
        Vector3 bottomLeft = Quaternion.Euler(0, -angle, 0) * Vector3.forward * distance;
        Vector3 bottomRight = Quaternion.Euler(0, angle, 0) * Vector3.forward * distance;

        Vector3 topCenter = bottomCenter + Vector3.up * height;
        Vector3 topRight = bottomRight + Vector3.up * height;
        Vector3 topLeft = bottomLeft + Vector3.up * height;

        int vert = 0;

        //left
        vertices[vert++] = bottomCenter;
        vertices[vert++] = bottomLeft;
        vertices[vert++] = topLeft;

        vertices[vert++] = topLeft;
        vertices[vert++] = topCenter;
        vertices[vert++] = bottomCenter;

        //right
        vertices[vert++] = bottomCenter;
        vertices[vert++] = topCenter;
        vertices[vert++] = topRight;

        vertices[vert++] = topRight;
        vertices[vert++] = bottomRight;
        vertices[vert++] = bottomCenter;

        float currentAngle = -angle;
        float deltaAngle = (angle * 2) / segments;

        for (int i = 0; i < segments; i++)
        {

             bottomLeft = Quaternion.Euler(0, currentAngle, 0) * Vector3.forward * distance;
             bottomRight = Quaternion.Euler(0, currentAngle + deltaAngle, 0) * Vector3.forward * distance;

             topRight = bottomRight + Vector3.up * height;
             topLeft = bottomLeft + Vector3.up * height;

            //far
            vertices[vert++] = bottomLeft;
            vertices[vert++] = bottomRight;
            vertices[vert++] = topRight;

            vertices[vert++] = topRight;
            vertices[vert++] = topLeft;
            vertices[vert++] = bottomLeft;

            //top
            vertices[vert++] = topCenter;
            vertices[vert++] = topLeft;
            vertices[vert++] = topRight;

            //bottom
            vertices[vert++] = bottomCenter;
            vertices[vert++] = bottomRight;
            vertices[vert++] = bottomLeft;

            currentAngle += deltaAngle;
        }

        for (int i = 0; i < numVertices; i++)
        {
            triangles[i] = i;
        }

        mesh.vertices = vertices;
        mesh.triangles = triangles;
        mesh.RecalculateNormals();


        return mesh;
    }

    private void OnValidate()
    {
        mesh = CreateWedgeMesh();
        scanInterval = 1.0f / scanFrequency;
    }

    private void OnDrawGizmos()
    {
        if (mesh)
        {
            Gizmos.color = meshColour;
            Gizmos.DrawMesh(mesh, transform.position, transform.rotation);
        }

        Gizmos.DrawWireSphere(transform.position, distance);
        for (int i = 0; i < count; i++)
        {
            Gizmos.DrawSphere(colliders[i].transform.position, 0.2f);
        }

        Gizmos.color = Color.green;
        foreach (var obj in objects)
        {
            Gizmos.DrawSphere(obj.transform.position, 0.2f);
        }
    }
}
