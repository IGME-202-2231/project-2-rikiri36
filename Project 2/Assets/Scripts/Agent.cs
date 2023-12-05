using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.UIElements;

public abstract class Agent : MonoBehaviour
{
    [SerializeField] protected PhysicsObject physicsObject;
    [SerializeField] float maxSpeed;
    [SerializeField] float maxForce;

    [SerializeField] private List<Obstacle> obstacles;

    protected Vector3 totalForce;

    protected Vector3 wanderForce;

    protected List<Vector3> foundObstacles = new List<Vector3>();

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
        Vector3 futurePos = CalcFuturePosition(time);

        ////Get a random angle by adding on a bit to whatever we used last time!
        //currentWanderAngle += Random.Range(-wanderRange, wanderRange);

        //// And stay inside a given max range!
        //if (currentWanderAngle > maxWanderAngle)
        //{
        //    currentWanderAngle = maxWanderAngle;
        //}
        //else if (currentWanderAngle < -maxWanderAngle)
        //{
        //    currentWanderAngle = -maxWanderAngle;
        //}

        ////Where would that displacement vector end?
        //Vector3 targetOffset = futurePosOffset;
        //targetOffset.x += Mathf.Cos(currentWanderAngle * Mathf.Deg2Rad) * radius;
        //targetOffset.y += Mathf.Sin(currentWanderAngle * Mathf.Deg2Rad) * radius;

        //// Need to return a force - seek the target position
        //// (current position + target offset)
        //return Seek(targetOffset + physicsObject.Position);


        //"Project a circle" into that space
        //What radius works best? Do radii have an effect on agent's movement?
        //Get a random angle to determine displacement vector
        float randAngle = Random.Range(0, Mathf.PI * 2);

        //Where would that displacement vector end?  Go there.
        Vector3 targetPos = futurePos;
        //targetPos.x / y += Mathf.Sin / Cos(randAngle) * radius
        targetPos.x += Mathf.Cos(randAngle) * radius;
        targetPos.y += Mathf.Sin(randAngle) * radius;

        // Need to return a force - how do I get that?
        return Seek(targetPos);
    }

    protected Vector3 StayInBounds(float time)
    {
        Vector3 futurePos = CalcFuturePosition(time);

        if (futurePos.x > (physicsObject.CamWidth- physicsObject.Radius) ||
            futurePos.x < -(physicsObject.CamWidth- physicsObject.Radius) ||
            futurePos.y > (physicsObject.CamHeight- physicsObject.Radius) ||
            futurePos.y < -(physicsObject.CamHeight- physicsObject.Radius) )
        {
            return Seek(Vector3.zero);

        }
        return Vector3.zero;
    }

    protected Vector3 AvoidObstacles(float time)
    {
        Vector3 totalAvoidForce = Vector3.zero;
        foundObstacles.Clear();

        foreach(Obstacle obstacle in obstacles)
        {
            Vector3 agentToObstacle = obstacle.transform.position - transform.position;
            float rightDot = 0, forwardDot = 0;

            //find whether if the obstacle is in front or behind agent.
            //positive if in front, negative if behind
            forwardDot = Vector3.Dot(physicsObject.Direction, agentToObstacle);

            //because it's wandering, future position is needed
            Vector3 futurePos = CalcFuturePosition(time);

            //?
            float dist = Vector3.Distance(transform.position, futurePos) + physicsObject.Radius;

            //if in front of me
            //if the two vectors are perpendicular to each other, dot product is zero
            if (forwardDot >= 0)
            {
                //within the box in front of us
                if (forwardDot <= dist + obstacle.Radius)
                {
                    
                    // how far left/right?
                    rightDot = Vector3.Dot(transform.right, agentToObstacle);

                    //Vector3 steeringForce = transform.right / Mathf.Abs(forwardDot/dist);
                    Vector3 steeringForce = transform.right * (1 - forwardDot / dist) * physicsObject.MaxForce;

                    // is the Obstacle withint the safe box width?
                    if (Mathf.Abs(rightDot) <= (physicsObject.Radius + obstacle.Radius) ) // 
                    {
                        foundObstacles.Add(obstacle.transform.position);

                        // if left, sterr right
                        if (rightDot < 0)
                        {
                            totalAvoidForce += steeringForce;
                        }

                        //if right, steer left.
                        else if (rightDot > 0)
                        {
                            totalAvoidForce += -steeringForce;
                        }

                    }

                }

                
            }
            //otherwise it's behind me and I don't care



        }

        return totalAvoidForce;
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawLine(transform.position, CalcFuturePosition(2) );
    }
}
