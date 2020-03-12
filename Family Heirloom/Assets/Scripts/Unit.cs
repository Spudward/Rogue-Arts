using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Unit : MonoBehaviour
{
    public Transform target;
    public float speed = 5f;
    public float coneAngle;
    Vector3 lastPlayerPos;
    public bool canSeePlayer;
    Vector3[] path;
    int targetIndex = 0;

    public enum Conditions
    {
        PATROL,
        FOLLOWPLAYER,
        MOVETOLASTSEENLOC
    }
    public Conditions myCondition;

    void Start()
    {
        myCondition = Conditions.PATROL;

    }

    void Update()
    {
        ConeOfVision();
        switch (myCondition) {
            case Conditions.PATROL:
                //move between posible hiding spots, starting at the closest one.
                break;
            case Conditions.FOLLOWPLAYER:
                //follow the player if they can be seen
                PathRequestManager.RequestPath(transform.position, target.position, OnPathFound);
                if (!canSeePlayer)
                {
                    myCondition = Conditions.MOVETOLASTSEENLOC;
                }
                break;
            case Conditions.MOVETOLASTSEENLOC:
                //move to the last known position of the player, if they cannot be seen then we continue patroling.
                PathRequestManager.RequestPath(transform.position, lastPlayerPos, OnPathFound);
                if (transform.position == lastPlayerPos)
                {
                    myCondition = Conditions.PATROL;
                }
                break;
        }    
    }

    void ConeOfVision ()
    {
        if (Vector3.Angle(target.position-transform.position, transform.forward) < coneAngle)
        {
            canSeePlayer = true;
            lastPlayerPos = target.position;
            if (myCondition != Conditions.FOLLOWPLAYER)
            {
                myCondition = Conditions.FOLLOWPLAYER;
            }   
        }
        else
        {
            canSeePlayer = false;
        }
    }

    public void OnPathFound (Vector3[] newPath, bool pathSuccessful)
    {
        if (pathSuccessful)
        {
            path = newPath;
            StopCoroutine("FollowPath");
            StartCoroutine("FollowPath");
        }
    }
    IEnumerator FollowPath ()
    {
        Vector3 currentWaypoint = path[0];
        print(currentWaypoint);
        
        while (true)
        {
            if (transform.position == currentWaypoint)
            {
                targetIndex++;
                if (targetIndex >= path.Length)
                {
                    yield break;
                }
                currentWaypoint = path[targetIndex];
                print(currentWaypoint);
            }
            transform.position = Vector3.MoveTowards(transform.position, currentWaypoint, speed);
            yield return null;
        }
           
    }
}
