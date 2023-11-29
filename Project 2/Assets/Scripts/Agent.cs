using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public abstract class Agent : MonoBehaviour
{
    [SerializeField] protected PhysicsObject physicsObject;
    [SerializeField] float maxSpeed;
    [SerializeField] float maxForce;

    protected Vector3 totalForce;

    protected Vector3 wanderForce;

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        //CalcSteeringForces();
        totalForce = Vector3.zero;

        CalcSteeringForces();

        Vector3.ClampMagnitude(totalForce, maxForce);
        physicsObject.ApplyForce(totalForce);

    }


    protected abstract void CalcSteeringForces();

    protected Vector3 Seek(Vector3 targetPos)
    {
        // Calculate desired velocity
        Vector3 desiredVelocity = targetPos - gameObject.transform.position;

        // Set desired = max speed
        desiredVelocity = desiredVelocity.normalized * maxSpeed;

        // Calculate seek steering force
        Vector3 seekingForce = desiredVelocity - physicsObject.Velocity;

        // Return seek steering force
        return seekingForce;
    }

    protected Vector3 Seek(GameObject target)
    {
        // Call the other version of Seek 
        //   which returns the seeking steering force
        //  and then return that returned vector. 
        return Seek(target.transform.position);
    }


    protected Vector3 Flee(Vector3 targetPos)
    {
        // Calculate desired velocity
        Vector3 desiredVelocity = gameObject.transform.position - targetPos;

        // Set desired = max speed
        desiredVelocity = desiredVelocity.normalized * maxSpeed;

        // Calculate seek steering force
        Vector3 fleeingForce = desiredVelocity - physicsObject.Velocity;

        // Return seek steering force
        return fleeingForce;
    }

    protected Vector3 Flee(GameObject target)
    {
        // Call the other version of Seek 
        //   which returns the seeking steering force
        //  and then return that returned vector. 
        return Flee(target.transform.position);
    }



    public Vector3 CalcFuturePosition(float time)
    {
        return physicsObject.Velocity * time + transform.position;

    }

    /// <summary>
    /// Determine the force to wander ahead a bit
    /// </summary>
    /// <param name="currentWanderAngle">A reference param to the current wander angle (in degrees). 
    /// Reference params allow us to change the value here and whatever 
    /// method called this will get that change!</param>
    /// <param name="wanderRange">How big an angle delta to allow (in degrees)</param>
    /// <param name="maxWanderAngle">How maximum angle to allow (in degrees)</param>
    /// <param name="time">look ahead time when projecting the circle</param>
    /// <param name="radius">radius when projecting the circle</param>
    /// <returns></returns>
    protected Vector3 Wander(ref float currentWanderAngle, float wanderRange,
        float maxWanderAngle, float time, float radius)
    {
        //Choose a distance ahead
        Vector3 futurePosOffset = CalcFuturePosition(time);

        //Get a random angle by adding on a bit to whatever we used last time!
        currentWanderAngle += Random.Range(-wanderRange, wanderRange);

        // And stay inside a given max range!
        if (currentWanderAngle > maxWanderAngle)
        {
            currentWanderAngle = maxWanderAngle;
        }
        else if (currentWanderAngle < -maxWanderAngle)
        {
            currentWanderAngle = -maxWanderAngle;
        }

        //Where would that displacement vector end?
        Vector3 targetOffset = futurePosOffset;
        targetOffset.x += Mathf.Cos(currentWanderAngle * Mathf.Deg2Rad) * radius;
        targetOffset.y += Mathf.Sin(currentWanderAngle * Mathf.Deg2Rad) * radius;

        // Need to return a force - seek the target position
        // (current position + target offset)
        return Seek(targetOffset + physicsObject.Position);
    }

    //protected Vector3 StayInBounds(float time)
    //{
    //    Vector3 futurePos = CalcFuturePosition(time);

    //    //right
    //    if (futurePos.x > physicsObject.CamWidth)
    //    {
    //        Vector3 desired = new Vector3(maxSpeed, physicsObject.Velocity.y);
    //        Vector3 steer = desired - physicsObject.Velocity;
    //        Vector3.ClampMagnitude(steer, maxForce);
    //        return steer;
    //    }
    //    //left
    //    else if (futurePos.x < -physicsObject.CamWidth)
    //    {
    //        Vector3 desired = new Vector3(-maxSpeed, physicsObject.Velocity.y);
    //        Vector3 steer = desired - physicsObject.Velocity;
    //        Vector3.ClampMagnitude(steer, maxForce);
    //        return steer;
    //    }

    //    //top
    //    if (futurePos.y > physicsObject.CamHeight)
    //    {
    //        Vector3 desired = new Vector3(physicsObject.Velocity.x, maxSpeed);
    //        Vector3 steer = desired - physicsObject.Velocity;
    //        Vector3.ClampMagnitude(steer, maxForce);
    //        return steer;
    //    }

    //    //bottom
    //    else if (futurePos.y < -physicsObject.CamHeight)
    //    {
    //        Vector3 desired = new Vector3(physicsObject.Velocity.x, -maxSpeed);
    //        Vector3 steer = desired - physicsObject.Velocity;
    //        Vector3.ClampMagnitude(steer, maxForce);
    //        return steer;
    //    }

    //    return Vector3.zero;
    //}

    protected Vector3 StayInBounds(float time)
    {
        Vector3 futurePos = CalcFuturePosition(time);

        if (futurePos.x > physicsObject.CamWidth ||
            futurePos.x < -physicsObject.CamWidth ||
            futurePos.y > physicsObject.CamHeight ||
            futurePos.y < -physicsObject.CamHeight)
        {
            return Seek(Vector3.zero);

        }
        return Vector3.zero;
    }

}
