using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Uninfected : Agent
{
    [SerializeField] private float currentWanderAngle;
    [SerializeField] private float wanderRange;
    [SerializeField] private float maxWanderAngle;
    [SerializeField] private float timeAhead;
    [SerializeField] private float wanderRadius;
    [SerializeField] private float boundsTime;
    [SerializeField] private float boundsScalar;
    

    private Vector3 boundsForce;

    protected override void CalcSteeringForces()
    {
        //protected Vector3 Wander(ref float currentWanderAngle, float wanderRange, float maxWanderAngle, float time, float radius)
        totalForce += Wander(ref currentWanderAngle, wanderRange, maxWanderAngle, timeAhead, wanderRadius);
        boundsForce = StayInBounds(boundsTime);
        boundsForce *= boundsScalar;
        totalForce += boundsForce;
    }
}
