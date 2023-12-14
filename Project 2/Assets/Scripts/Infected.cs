using System;
using UnityEngine;

public class Infected : Agent
{
    [SerializeField] private float currentWanderAngle;

    [SerializeField] private float maxWanderAngle = 45f;
    [SerializeField] private float maxWanderChangePerSecond = 10f;

    [SerializeField] private float timeAhead = 2f;
    [SerializeField] private float wanderRadius = 1f;
    [SerializeField] private float boundsTime = 1f;
    [SerializeField] private float boundsScalar = 3f;

    /// <summary>
    /// 34:45 timestamp
    /// </summary>
    public float TimeAhead
    {
        get
        {
            return timeAhead;
        }
    }


    private Vector3 boundsForce;
    [SerializeField] private float avoidWeight = 2f;

    protected override void CalcSteeringForces()
    {

        switch (currentStatus)
        {
            case Status.Alive:
                
                if (target == null  )
                {
                    target = FindCloset(InfectionStatus.Uninfected);
                }
                else if (target != null && target.CurrentStatus == Status.Dead)
                {
                    target = FindCloset(InfectionStatus.Uninfected);
                }


                if (target != null )
                {
                    totalForce += Seek(target);

                    //checking for contact to kill
                    if (Vector3.Distance(transform.position, target.transform.position) <= physicsObject.Radius)
                    {
                        Kill(target);
                    }
                }
                else
                {
                    totalForce += Wander(ref currentWanderAngle, maxWanderAngle, maxWanderChangePerSecond, timeAhead, wanderRadius);

                }


                break;

            case Status.Dead:
                physicsObject.Velocity = Vector3.zero;

                break;

        }

        //totalForce += Separate(timeAhead, wanderRadius);
        boundsForce = StayInBounds(boundsTime);
        boundsForce *= boundsScalar;
        totalForce += boundsForce;
        totalForce += AvoidObstacles(timeAhead, wanderRadius) * avoidWeight ;

    }
}