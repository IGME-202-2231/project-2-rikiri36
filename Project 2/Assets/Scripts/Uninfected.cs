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
    [SerializeField] private float avoidWeight;

    protected override void CalcSteeringForces()
    {
        //protected Vector3 Wander(ref float currentWanderAngle, float wanderRange, float maxWanderAngle, float time, float radius)
        totalForce += Wander(ref currentWanderAngle, wanderRange, maxWanderAngle, timeAhead, wanderRadius);
        boundsForce = StayInBounds(boundsTime);
        boundsForce *= boundsScalar;
        totalForce += boundsForce;
        totalForce += AvoidObstacles(timeAhead);
    }

    private void OnDrawGizmos()
    {
        //
        //  Draw safe space box
        //
        Vector3 futurePos = CalcFuturePosition(timeAhead);

        float dist = Vector3.Distance(transform.position, futurePos) + physicsObject.Radius;

        Vector3 boxSize = new Vector3(physicsObject.Radius * 2f,
            dist
            , physicsObject.Radius * 2f);

        Vector3 boxCenter = Vector3.zero;
        boxCenter.y += dist / 2f;

        Gizmos.color = Color.green;

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
}
