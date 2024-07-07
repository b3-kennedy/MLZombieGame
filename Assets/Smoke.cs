using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Smoke : MonoBehaviour
{

    public AnimationCurve smokeSize;
    float smokeTime = 0;
    public float smokeDuration;

    // Start is called before the first frame update
    void Start()
    {
        smokeTime = 0;
        Destroy(gameObject, smokeDuration);
    }

    private void OnDestroy()
    {
        GameManager.Instance.DisableSmokeCamera();
    }

    // Update is called once per frame
    void Update()
    {
        smokeTime += Time.deltaTime;
        GetComponent<Transform>().localScale = new Vector3(smokeSize.Evaluate(smokeTime), smokeSize.Evaluate(smokeTime), smokeSize.Evaluate(smokeTime));
    }

    void OnTriggerEnter(Collider other)
    {
        Debug.Log(other.gameObject.name);
        if (other.CompareTag("MainCamera"))
        {
            GameManager.Instance.EnableSmokeCamera();
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("MainCamera"))
        {
            GameManager.Instance.DisableSmokeCamera();
        }
    }
}
