using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using Panda;

public class chaser : MonoBehaviour
{
    public float Mrange;
    public float radius;
    public float angle;
    public GameObject Player;
    public float MoveSpeed = 4;
    public float MaxDist = 10;
    public float MinDist = 1;
    public float volume = 10;
    public float OriginalPosition;
    public int currentL;
    public Transform[] points;
    public Transform centrePoint;
    public AudioSource audiosource;
    private Collider hitbox;
    private GameObject targetGo = null;
    private WaypointManager waypointManager;
    private NavMeshAgent navMeshAgent;
    private bool iscolliding_player;
    private Vector3 location;
    private Vector3 dejavu;

    public bool playerspotted;

    // start is called before the first frame update
    
    void Start()
    {
        currentL = 0;
        hitbox = GetComponent<SphereCollider>();
        navMeshAgent = GetComponent<NavMeshAgent>();
        waypointManager = GetComponent<WaypointManager>();
        audiosource = GetComponentInChildren<AudioSource>();
       
        
    }

    // update is called once per frame
    void Update()
    {      
        
    }

    bool RandomPoint(Vector3 center, float range, out Vector3 result)
    {
        Vector3 randomPoint = center + Random.insideUnitSphere * range;
        NavMeshHit hit;
        if (NavMesh.SamplePosition(randomPoint,out hit, 1.0f, NavMesh.AllAreas))
        {
            result = hit.position;
            return true;
        }
        result = Vector3.zero;
        return false;
    }

    private IEnumerator fovroutine()
    {
        float delay = 0.2f;

        while (true)
        {
            yield return delay;
            CanSeePlayer();
        }
    }

    [Task]
     bool CanSeePlayer()
    {
        Collider[] rangechecks = Physics.OverlapSphere(transform.position, radius);
        
        if (rangechecks.Length != 0)
        {
            Transform target = rangechecks[0].transform;
            Vector3 directiontotarget = (target.position - transform.position).normalized;
            if (Vector3.Angle(transform.position, directiontotarget) < angle / 2)
            {
                float distancetotarget = Vector3.Distance(transform.position, target.position);
                if (!Physics.Raycast(transform.position, directiontotarget))
                {
                    playerspotted = true;
                    location = target.position;
                    return playerspotted;
                }
                else
                {
                    playerspotted = false;
                    return playerspotted;
                }

            }
        }

        return false;
    }

   //[Task]
   //bool Patrol()
   // {
       // if (transform.position != points[currentL].position)
       // {
       //     transform.position = Vector3.MoveTowards(transform.position, points[currentL].position, MoveSpeed * Time.deltaTime);
       // }
       // else
       // {
       //     currentL = (currentL + 1) % points.Length;
       //     return false;
       // }
       // return true;
    //}

//[Task]
//    bool ChasePlayer()
//    {
//            transform.LookAt(Player);

//            if (Vector3.Distance(transform.position, Player.position) >= MinDist)
//            {

//                transform.position += transform.forward * MoveSpeed * Time.deltaTime;
//                OnTriggerEnter(hitbox);
//                return true;
//            }
//        return false;
//    }

   // [Task]
    //bool AwareOfPlayer()
    //{

    //}

    //[Task]
    //bool LastKnownLocation()
    //{
    //    NavMeshAgent.SetDestination(location);
    //}

    [Task]
    bool ForgetPlayer()
    {       
        location = dejavu;
        return true;
    }

    [Task]
    bool SetDestinationRandom()
    {
        if (navMeshAgent.remainingDistance <= navMeshAgent.stoppingDistance)
        {
            Vector3 point;
            if (RandomPoint(centrePoint.position, Mrange, out point))
            {
                navMeshAgent.SetDestination(point);
                
            }
        }
        return true;
    }

    //[Task]
    //bool SetDestinationOrigin()
    //{


    //}

    [Task]
    void wait(float time)
    {
         StartCoroutine(waittime());
         IEnumerator waittime()
        {
            yield return new WaitForSeconds(time);
        }
    }

    void OnTriggerEnter(Collider enemy)
    {
        if (enemy.gameObject == Player.gameObject)
        {
            
            audiosource.Play();
            iscolliding_player = true;
        }
    }

    void OnTriggerExit(Collider enemy)
    {
        if (enemy.gameObject == Player.gameObject)
        {
            iscolliding_player = false;
        }
    }

}
