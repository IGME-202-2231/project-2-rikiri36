using System;
using UnityEditor.ShaderKeywordFilter;
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

        totalForce += Wander(ref currentWanderAngle, maxWanderAngle, maxWanderChangePerSecond, timeAhead, wanderRadius);
        //totalForce += Separate(timeAhead, wanderRadius);
        boundsForce = StayInBounds(boundsTime);
        boundsForce *= boundsScalar;
        totalForce += boundsForce;
        totalForce += AvoidObstacles(timeAhead, wanderRadius) * avoidWeight ;

    }
}