using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ThrowAttack : MonoBehaviour
{

    public AnimationCurve trajectory;
    public float duration;
    public float heightY;

    public float impactRadius;

    public Animator anim;
    public Transform throwPoint;
    public GameObject rock;
    public GameObject aoe;
    GameObject throwAoe;
    GameObject spawnedRock;
    BossAI boss;
    public float damage;



    // Start is called before the first frame update
    void Start()
    {
        boss = GetComponent<BossAI>();
    }

    public void Execute()
    {
        anim.SetBool("charge", false);
        anim.SetBool("wave", false);
        anim.SetBool("stomp", false);
        spawnedRock = Instantiate(rock, throwPoint.transform.position, Quaternion.identity);
        spawnedRock.transform.SetParent(throwPoint);
        spawnedRock.transform.localPosition = Vector3.zero;
        spawnedRock.GetComponent<Rigidbody>().isKinematic = true;
        spawnedRock.GetComponent<Collider>().enabled = false;
        anim.SetBool("throw", true);
    }

    public void Launch()
    {
        if(spawnedRock != null)
        {
            spawnedRock.transform.SetParent(null);
            StartCoroutine(FollowCurve(throwPoint.transform.position, boss.target.position));
            throwAoe = Instantiate(aoe, new Vector3(boss.target.position.x, 0.1f, boss.target.position.z), Quaternion.identity);
            CircleDrawer circle = throwAoe.GetComponent<CircleDrawer>();
            circle.radius = impactRadius;
            
        }
    }

    public void EndAttack()
    {
        anim.SetBool("throw", false);
        boss.canLookAt = true;
        boss.OnEndAttack();
    }

    public IEnumerator FollowCurve(Vector3 start, Vector3 target)
    {
        float timePassed = 0f;

        Vector3 end = target;

        while(timePassed < duration)
        {
            timePassed += Time.deltaTime;
            float linearTime = timePassed / duration;
            float heightTime = trajectory.Evaluate(linearTime);

            float height = Mathf.Lerp(0f, heightY, heightTime);

            spawnedRock.transform.position = Vector3.Lerp(start, end, linearTime) + new Vector3(0, height, 0);

            yield return null;
        }

        if(Vector3.Distance(boss.target.position, spawnedRock.transform.position) < impactRadius)
        {
            boss.target.GetComponent<PlayerHealth>().TakeDamage(damage);
        }
        EndAttack();
        Destroy(throwAoe);
        Destroy(spawnedRock);

    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
