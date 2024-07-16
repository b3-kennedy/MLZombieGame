using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;

public class CircleDrawer : MonoBehaviour
{

    LineRenderer lr;
    public int steps;
    public float radius;
    public List<Collider> collidersList;

    // Start is called before the first frame update
    void Start()
    {
        lr = GetComponent<LineRenderer>();
    }

    // Update is called once per frame
    void Update()
    {
        DrawCircle();
    }

    void DrawCircle()
    {
        lr.positionCount = steps;
        for (int i = 0; i < steps; i++)
        {
            float circumferenceProgress = (float)i / steps;
            float currentRadian = circumferenceProgress * 2 * Mathf.PI;
            float xScaled = Mathf.Cos(currentRadian);
            float yScaled = Mathf.Sin(currentRadian);

            float x = xScaled * radius;
            float y = yScaled * radius;

            Vector3 currentPos = transform.position + new Vector3(x, 0, y);

            lr.SetPosition(i, currentPos);
        }
    }

}
