//using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Uninfected : Agent
{
    [SerializeField] private float currentWanderAngle;

    //[SerializeField] private float wanderRange;
    [SerializeField] private float maxWanderAngle = 45f;
    [SerializeField] private float maxWanderChangePerSecond = 10f;

    [SerializeField] private float timeAhead;
    [SerializeField] private float wanderRadius;
    [SerializeField] private float boundsTime;
    [SerializeField] private float boundsScalar;

    //[SerializeField] private float fleeWeight = 1f;

    private Vector3 boundsForce;
    [SerializeField] private float avoidWeight;

    

    

    protected override void CalcSteeringForces()
    {
        switch (currentStatus)
        {
            case Status.Alive:

                //target = FindCloset(InfectionStatus.Infected);

                //float dist = Vector3.Distance(target.transform.position, transform.position);

                //if (target != null && dist <= (physicsObject.Radius*2))
                //{
                //    //totalForce += Flee(target) * fleeWeight;
                //}


                totalForce += Wander(ref currentWanderAngle, maxWanderAngle, maxWanderChangePerSecond, timeAhead, wanderRadius);

                boundsForce = StayInBounds(boundsTime);
                boundsForce *= boundsScalar;
                totalForce += boundsForce;

                totalForce += AvoidObstacles(timeAhead, wanderRadius) * avoidWeight;
                totalForce += Separate(InfectionStatus.Uninfected);
                totalForce += Flock();

                break;

            case Status.Dead:

                physicsObject.Velocity = Vector3.zero;

                break;
        }

        

    }

    private void OnDrawGizmos()
    {
        
        //
        //  Draw safe space box
        //
        Vector3 futurePos = CalcFuturePosition(timeAhead);
        

        float dist = Vector3.Distance(futurePos, transform.position) + physicsObject.Radius;

        Vector3 boxSize = new Vector3(physicsObject.Radius * 2f,
            dist
            , 0);

        Vector3 boxCenter = Vector3.zero;
        boxCenter.y = dist / 2f;

        //Gizmos.DrawLine(transform.position, new Vector3(dist ,dist , 0) );

        Gizmos.color = Color.green;


        Gizmos.DrawWireSphere(futurePos, wanderRadius);
        Gizmos.matrix = transform.localToWorldMatrix;
        Gizmos.DrawWireCube(boxCenter, boxSize);
        Gizmos.matrix = Matrix4x4.identity;


        //
        //  Draw lines to found obstacles
        //
        Gizmos.color = Color.red;

        foreach (Vector3 pos in foundObstacles)
        {
            Gizmos.DrawLine(transform.position, pos);
        }
    }

    protected override void Init()
    {
        currentWanderAngle = UnityEngine.Random.Range(-maxWanderAngle, maxWanderAngle);
    }
}
