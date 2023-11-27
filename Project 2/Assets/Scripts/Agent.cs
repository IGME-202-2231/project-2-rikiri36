using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Agent : MonoBehaviour
{
    [SerializeField] protected PhysicsObject physicsObject;
    [SerializeField] float maxSpeed;
    [SerializeField] float maxForce;

    [SerializeField] float lookAheadTime;
    [SerializeField] float wanderRadius;
    

    protected Vector3 totalForce;

    private Vector3 wanderForce;

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        //CalcSteeringForces();

        CalcSteeringForces();

        Vector3.ClampMagnitude(totalForce, maxForce);
        physicsObject.ApplyForce(totalForce);

        totalForce = Vector3.zero;
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

    protected Vector3 Wander(float time)
    {
        //Choose a distance ahead
        Vector3 futurePos = CalcFuturePosition(time);

        //"Project a circle" into that space
        //What radius works best? Do radii have an effect on agent's movement?
        //Get a random angle to determine displacement vector
        float randAngle = Random.Range(0, Mathf.PI * 2);

        //Where would that displacement vector end?  Go there.
        Vector3 targetPos = futurePos;
        //targetPos.x / y += Mathf.Sin / Cos(randAngle) * radius

        // Need to return a force - how do I get that?
        return Vector3.zero;
    }
}
