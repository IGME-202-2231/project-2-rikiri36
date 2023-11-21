using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Agent : MonoBehaviour
{
    [SerializeField] protected PhysicsObject physicsObject;
    [SerializeField] float maxSpeed;
    [SerializeField] float maxForce;

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

    protected Vector3 Seek(Vector3 targetPos, float weight = 1f)
    {
        // Calculate desired velocity
        Vector3 desiredVelocity = targetPos - gameObject.transform.position;

        // Set desired = max speed
        desiredVelocity = desiredVelocity.normalized * maxSpeed;

        // Calculate seek steering force
        Vector3 seekingForce = desiredVelocity - physicsObject.Velocity;

        // Return seek steering force
        return seekingForce * weight;
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
}
