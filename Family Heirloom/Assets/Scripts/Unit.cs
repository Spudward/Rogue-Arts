using System.Collections;
using System.Collections.Generic;
using System.Net.Mime;
using System.Runtime.InteropServices;
using System.Security.Cryptography;
using UnityEngine;

public class Unit : MonoBehaviour
{
    public Transform target;
    public float speed = 5f;
    public float visionRange = 5f;
    public float coneAngle;
    Vector3 lastPlayerPos;
    public bool canSeePlayer;
    Vector3[] path;
    int targetIndex = 0;
    public List<GameObject> openPatrolPoints = new List<GameObject>();
    public List<GameObject> closedPatrolPoints = new List<GameObject>();
    GameObject currentPoint;

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
        openPatrolPoints.AddRange (GameObject.FindGameObjectsWithTag("Patrol"));
        currentPoint = null;
    }

    void Update()
    {
        ConeOfVision();
        switch (myCondition) {
            case Conditions.PATROL:
                //move between posible hiding spots, starting at the closest one.

                if (openPatrolPoints.Count > 0)
                {
                    if (currentPoint == null)
                    {
                        float distance = 10000;
                        foreach (GameObject point in openPatrolPoints)
                        {
                            float myDistance = (transform.position - point.transform.position).magnitude;
                            if (myDistance <= distance)
                            {
                                currentPoint = point;
                                distance = myDistance;
                                print(currentPoint);
                            }
                            
                        }
                        print("Pathing");

                    } else
                    {

                        PathRequestManager.RequestPath(transform.position, currentPoint.transform.position, OnPathFound);
                        if ((transform.position - currentPoint.transform.position).magnitude <= 5)
                        {
                            openPatrolPoints.Remove(currentPoint);
                            closedPatrolPoints.Add(currentPoint);
                            currentPoint = null;
                        }
                    }
                } else
                {
                    openPatrolPoints = closedPatrolPoints;
                    closedPatrolPoints = new List<GameObject>();
                }
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

    void ConeOfVision()
    {
        
        if (Vector3.Angle(target.position-transform.position, transform.forward) < coneAngle)
        {

            RaycastHit hit;
            if (Physics.Raycast(transform.position, (target.position - transform.position).normalized, out hit, visionRange))
            {


                if (hit.collider.gameObject.CompareTag("Player"))
                {
                    canSeePlayer = true;
                    lastPlayerPos = target.position;
                    myCondition = Conditions.FOLLOWPLAYER;
                    
                }
                else
                {
                    canSeePlayer = false;
                }
            }
            else
            {
                canSeePlayer = false;
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
                Vector3 targetDirection = currentWaypoint - transform.position;
                float singleStep = speed * Time.deltaTime;
                Vector3 newDirection = Vector3.RotateTowards(transform.forward, targetDirection, singleStep, 0.0f);
                print(currentWaypoint);
            }
            transform.position = Vector3.MoveTowards(transform.position, currentWaypoint, speed);
            yield return null;
        }
           
    }
}
