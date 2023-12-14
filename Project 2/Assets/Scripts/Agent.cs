using System.Collections;
using System.Collections.Generic;
using System.Data.Common;
using UnityEditor;
using UnityEngine;


public abstract class Agent : MonoBehaviour
{
    //agent manager
    private GameObject manager;

    public GameObject Manager
    {
        get { return  manager; }
        set { manager = value; }
    }

    [SerializeField] protected PhysicsObject physicsObject;
    [SerializeField] protected float maxSpeed = 2f;
    [SerializeField] float maxForce = 1f;


    /// <summary>
    /// How many pixels lifted from the camera area to check for stay in bounds
    /// </summary>
    [SerializeField] float wallBoundaryOffSet = 0.6f;

    private List<Obstacle> obstacles;
    protected List<Uninfected> uninfectedList;
    protected List<Agent> infectedList;

    public List<Obstacle> ObstaclesList
    {
        set
        {
            obstacles = value;
        }
    }

    protected Vector3 totalForce;

    protected Vector3 wanderForce;

    private float borderTimer = 1;

    protected List<Vector3> foundObstacles = new List<Vector3>();

    protected Status currentStatus;
    protected InfectionStatus currentInfectionState;
    protected Agent target;


    public Status CurrentStatus
    {
        get { return currentStatus; }
        set { currentStatus = value; }
    }

    protected virtual void Init()
    {

    }

    // Start is called before the first frame update
    void Start()
    {
        Init();

        obstacles = manager.GetComponent<AgentManager>().obstacles;
        uninfectedList = manager.GetComponent<AgentManager>().uninfectedList;
        infectedList = manager.GetComponent<AgentManager>().infectedList;

        if (currentInfectionState == InfectionStatus.Infected)
        {
            if (uninfectedList.Count > 0)
            {
                target = uninfectedList[Random.Range(0, uninfectedList.Count - 1)];
            }
            else
            {
                target = null;
            }
            
        }
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

    protected Vector3 Seek(Agent target)
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


    protected Vector3 Flee(Agent target)
    {
        // Call the other version of Seek 
        //   which returns the seeking steering force
        //  and then return that returned vector. 
        return Flee(target.transform.position);
    }


    public Vector3 CalcFuturePosition(float time)
    {
        return (physicsObject.Velocity * time) + transform.position;

    }

    /// <summary>
    /// Determine the force to wander ahead a bit
    /// </summary>
    /// <param name="currentWanderAngle">A reference param to the current wander angle (in degrees). 
    /// Reference params allow us to change the value here and whatever 
    /// method called this will get that change!</param>
    /// <param name="maxWanderAngle">how far left or right to turn can turn (max)</param>
    /// <param name="maxWanderChangePerSec">how big an angle change to allow (in degrees)</param>
    /// <param name="time">look ahead time when projecting the circle</param>
    /// <param name="radius">radius when projecting the circle</param>
    /// <returns></returns>
    protected Vector3 Wander(ref float currentWanderAngle, float maxWanderAngle,
        float maxWanderChangePerSec, float time, float radius)
    {
        Vector3  getAwayFromBorder = Vector3.zero;

        if (transform.position.x > (physicsObject.CamWidth - 2f) ||
            transform.position.x < -(physicsObject.CamWidth - 2f) ||
            transform.position.y > (physicsObject.CamHeight - 2f) ||
            transform.position.x < -(physicsObject.CamHeight - 2f)    )
        {
            if (borderTimer > 0)
            {
                borderTimer -= Time.deltaTime;
            }
            else
            {
                getAwayFromBorder = Seek(Vector3.zero) * 20;
                borderTimer = 3;
            }
        }
        

        //Choose a distance ahead
        Vector3 futurePos = CalcFuturePosition(time);

        // However fast the game is updated, can only change angle by x many degrees at one second.
        float maxWanderChange = maxWanderChangePerSec * Time.deltaTime;
        currentWanderAngle += Random.Range(-(maxWanderChange) , maxWanderChange);

        //clamp it
        //currentWanderAngle = Mathf.Clamp(currentWanderAngle, -maxWanderAngle, maxWanderAngle);
        if (currentWanderAngle > maxWanderAngle)
        {
            currentWanderAngle = maxWanderAngle;
        }
        else if (currentWanderAngle < -maxWanderAngle)
        {
            currentWanderAngle = -maxWanderAngle;
        }

        currentWanderAngle += 90;

        //Vector3 wanderTarget = Quaternion.Euler(0, 0, currentWanderAngle) * physicsObject.Direction.normalized + physicsObject.Position;

        //"Project a circle" into that space
        //What radius works best? Do radii have an effect on agent's movement?
        //Get a random angle to determine displacement vector
        //float randAngle = Random.Range(0, Mathf.PI * 2);

        //Where would that displacement vector end?  Go there.
        Vector3 targetPos = futurePos;
        //targetPos.x / y += Mathf.Sin / Cos(randAngle) * radius
        targetPos.x += Mathf.Cos(currentWanderAngle * Mathf.Deg2Rad) * radius;
        targetPos.y += Mathf.Sin(currentWanderAngle * Mathf.Deg2Rad) * radius;

        // Need to return a force - how do I get that?
        return Seek(targetPos) + getAwayFromBorder;
    }

    protected Vector3 StayInBounds(float time)
    {
        Vector3 futurePos = CalcFuturePosition(time);

        if (futurePos.x > (physicsObject.CamWidth - wallBoundaryOffSet) ||
            futurePos.x < -(physicsObject.CamWidth - wallBoundaryOffSet) ||
            futurePos.y > (physicsObject.CamHeight - wallBoundaryOffSet) ||
            futurePos.y < -(physicsObject.CamHeight - wallBoundaryOffSet))
        {





            return Seek(Vector3.zero) * 2;


        }

        return Vector3.zero;

        //Vector3 targetPos = Vector3.zero;

        //if (futurePos.x > (physicsObject.CamWidth - wallBoundaryOffSet) ||
        //    futurePos.x < -(physicsObject.CamWidth - wallBoundaryOffSet) )
        //{
        //    targetPos = futurePos;
        //    targetPos.x = -futurePos.x;

        //    return Seek(targetPos) + Seek(Vector3.zero);

        //}
        //if (futurePos.y > (physicsObject.CamHeight - wallBoundaryOffSet) ||
        //    futurePos.y < -(physicsObject.CamHeight - wallBoundaryOffSet) )
        //{
        //    targetPos = futurePos;
        //    targetPos.y = -futurePos.y;
        //    return Seek(targetPos) + Seek(Vector3.zero);

        //}

        //return targetPos;
    }

    protected Vector3 AvoidObstacles(float timeAhead, float wanderRadius)
    {
        Vector3 originalVel = physicsObject.Velocity;

        Vector3 totalAvoidForce = Vector3.zero;
        foundObstacles.Clear();

        foreach(Obstacle obstacle in obstacles)
        {
            Vector3 agentToObstacle = obstacle.transform.position - transform.position;
            float rightDot = 0, forwardDot = 0;

            //find whether if the obstacle is in front or behind agent.
            //positive if in front, negative if behind
            forwardDot = Vector3.Dot(physicsObject.Direction.normalized, agentToObstacle );

            Vector3 futurePos = transform.position + (physicsObject.Velocity.normalized * timeAhead);
            float dist = Vector3.Distance(futurePos, transform.position) + wanderRadius;

            //if in front of me
            //if the two vectors are perpendicular to each other, dot product is zero
            if (forwardDot >= 0)
            {
                // if objects are within a radius (# of units)
                if (agentToObstacle.magnitude <= (dist + obstacle.Radius))
                {
                    // how far left/right?
                    rightDot = Vector3.Dot(agentToObstacle,transform.right );

                    //Vector3 steeringForce = transform.right / Mathf.Abs(forwardDot/dist);
                    Vector3 steeringForce = transform.right * (1-forwardDot / dist) * physicsObject.MaxForce;

                    // is intersect right or left with obstacle?
                    //checking if
                    if (Mathf.Abs(rightDot) <= (physicsObject.Radius + obstacle.Radius) ) // 
                    {
                        foundObstacles.Add(obstacle.transform.position);

                        // if left, sterr right
                        if (rightDot < 0)
                        {
                            totalAvoidForce += (steeringForce);
                        }
                        //if right, steer left.
                        else if (rightDot > 0)
                        {
                            totalAvoidForce += -(steeringForce);
                        }
                        else if (rightDot == 0)
                        {
                            totalAvoidForce += (steeringForce );
                        }

                    }

                    
                }

                
            }
            //otherwise it's behind me and I don't care



        }

        totalAvoidForce += originalVel.normalized;
        
        return totalAvoidForce;
    }


    protected Vector3 Separate(InfectionStatus type)
    {
        Vector3 totalSeparateForce = Vector3.zero;
        int count = 0;

        float detectionRange = physicsObject.Radius * 1.5f;
        float dist = 0;

        switch (type)
        {
            case InfectionStatus.Uninfected:

                foreach (Uninfected a in uninfectedList)
                {
                    dist = Vector3.Distance(transform.position, a.transform.position);

                    if (dist < (detectionRange + a.physicsObject.Radius) && dist > 0)
                    {
                        // checking if agent is not on top or itself
                        if (Mathf.Epsilon < dist)
                        {
                            Vector3 desiredVelocity = Flee(a).normalized;

                            totalSeparateForce += (desiredVelocity / dist);
                            count++;

                        }
                    }

                }

                if (count > 0)
                {
                    totalSeparateForce = totalSeparateForce / count;

                    totalSeparateForce *= maxSpeed;

                    totalSeparateForce = totalSeparateForce - physicsObject.Velocity;
                }

                

                break;

            case InfectionStatus.Infected:

                foreach (Infected b in infectedList)
                {
                    dist = Vector3.Distance(transform.position, b.transform.position);

                    if (dist < (detectionRange + b.physicsObject.Radius) && dist > 0)
                    {
                        // checking if agent is not on top or itself
                        if (Mathf.Epsilon < dist)
                        {
                            Vector3 desiredVelocity = Flee(b).normalized;

                            totalSeparateForce += (desiredVelocity / dist);
                            count++;

                        }
                    }

                }

                if (count > 0)
                {
                    totalSeparateForce = totalSeparateForce / count;

                    totalSeparateForce *= maxSpeed;

                    totalSeparateForce = totalSeparateForce - physicsObject.Velocity;
                }

                break;

        }

        return totalSeparateForce;
    }

    public Agent FindCloset(InfectionStatus type)
    {
        float minDist = Mathf.Infinity;
        Agent nearest = null;

        switch (type)
        {
            case InfectionStatus.Uninfected:
                foreach (Uninfected uninfected in uninfectedList)
                {
                    if (uninfected.currentStatus != Status.Dead)
                    {
                        float dist = Vector3.Distance(uninfected.transform.position, transform.position);

                        if (dist < minDist)
                        {
                            nearest = uninfected;
                            
                            minDist = dist;
                        }
                    }
                }

                break;

            case InfectionStatus.Infected:
                foreach (Uninfected infected in uninfectedList)
                {
                    if (infected.currentStatus != Status.Dead)
                    {
                        float dist = Vector3.Distance(infected.transform.position, transform.position);

                        if (dist < minDist)
                        {
                            nearest = infected;
                            minDist = dist;
                        }
                    }
                }
                break;
        }
        return nearest;
    }


    /// <summary>
    /// ONLY for UNINFECTED object
    /// </summary>
    /// <param name="type"></param>
    /// <returns></returns>
    public Vector3 Align()
    {
        float neightborDist = physicsObject.Radius * 2;

        float forwardDot = 0, dist = 0;

        Vector3 steeringForce = Vector3.zero;

        int count = 0;

        Vector3 sum = Vector3.zero;

        Vector3 agentToNeightbor = Vector3.zero;

        foreach (Uninfected a in uninfectedList)
        {
            agentToNeightbor = a.transform.position - transform.position;

            forwardDot = Vector3.Dot(physicsObject.Direction.normalized, agentToNeightbor);

            if (forwardDot > 0)
            {
                dist = Vector3.Distance(transform.position, a.transform.position);

                if (dist < neightborDist && dist > Mathf.Epsilon)
                {
                    sum += a.physicsObject.Velocity;
                    count++;
                }
            }
        }

        if (count > 0)
        {
            sum = (sum / count).normalized;

            sum *= maxSpeed;

            steeringForce = sum - physicsObject.Velocity;
            steeringForce = Vector3.ClampMagnitude(steeringForce, maxForce);

            return steeringForce;
        }
        else
        {
            return steeringForce;
        }
        
    }

    /// <summary>
    /// ONLY for UNINFECTED
    /// </summary>
    /// <returns></returns>
    public Vector3 Cohesion()
    {
        float neightborDist = physicsObject.Radius * 2;

        float forwardDot = 0, dist = 0;

        int count = 0;

        Vector3 sum = Vector3.zero;

        Vector3 agentToNeightbor = Vector3.zero;

        foreach (Uninfected a in uninfectedList)
        {
            agentToNeightbor = a.transform.position - transform.position;

            forwardDot = Vector3.Dot(physicsObject.Direction.normalized, agentToNeightbor);

            if (forwardDot > 0)
            {
                dist = Vector3.Distance(transform.position, a.transform.position);

                if (dist < neightborDist && dist > Mathf.Epsilon)
                {
                    sum += a.transform.position;
                    count++;
                }
            }
        }

        if (count > 0)
        {
            sum = (sum / count);

            sum *= maxSpeed;

            return Seek(sum);
        }
        else
        {
            return Vector3.zero;
        }
    }
    
    /// <summary>
    /// for UNINFECTED ONLY
    /// </summary>
    /// <returns></returns>
    public Vector3 Flock()
    {
        Vector3 sum = Vector3.zero;

        sum += Separate(InfectionStatus.Uninfected);
        sum += Align();
        sum += Cohesion();
        

        return sum;
    }

    public void Kill(Agent target)
    {
        target.currentStatus = Status.Dead;
    }

    private void OnDrawGizmos()
    {
        

        
    }
}
