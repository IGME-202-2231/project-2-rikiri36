using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PhysicsObject : MonoBehaviour
{
    [SerializeField] private Vector3 position;
    [SerializeField] private Vector3 direction;
    [SerializeField] private Vector3 velocity;

    public Vector3 Direction
    {
        get { return position; }
    }

    public Vector3 Position
    {
        get { return position; }
        set { position = value; }
    }

    public Vector3 Velocity
    {
        get { return velocity; }
    }

    // Sum of all forces in a frame - New
    [SerializeField] private Vector3 acceleration = Vector3.zero;

    // Mass of object - New
    [SerializeField] private float mass = 1;

    [SerializeField] private float maxSpeed = 10;


    [SerializeField] private float radius;
    public float Radius
    {
        get { return radius; }
    }


    //camera
    Camera cam;
    float camHeight;
    float camWidth;

    public float CamHeight
    {
        get { return camHeight; }
    }

    public float CamWidth
    {
        get { return camWidth; }
    }


    // Start is called before the first frame update
    void Start()
    {
        cam = Camera.main;
        camHeight = cam.orthographicSize;
        camWidth = camHeight * cam.aspect;

        position = transform.position;

        direction = Random.insideUnitCircle.normalized;
    }

    // Update is called once per frame
    void Update()
    {
        //apply ALL forces first



        // Calculate the velocity for this frame - New
        velocity += acceleration * Time.deltaTime;
        velocity = Vector3.ClampMagnitude(velocity, maxSpeed);

        gameObject.transform.rotation = Quaternion.LookRotation(Vector3.forward, velocity);

        position += velocity * Time.deltaTime;

        //Bounce();

        // Grab current direction from velocity  - New
        direction = velocity.normalized;

        transform.position = position;

        // Zero out acceleration - New
        acceleration = Vector3.zero;



    }


    public void ApplyForce(Vector3 force)
    {
        acceleration += force / mass;
    }

    void ApplyGravity(Vector3 force)
    {
        acceleration += force;
    }

    void Bounce()
    {
        if (position.x > camWidth)
        {
            velocity.x *= -1;
            position.x = camWidth;
        }
        else if (position.x < -(camWidth))
        {
            velocity.x *= -1;
            position.x = -camWidth;
        }

        if (position.y > camHeight)
        {
            velocity.y *= -1;
            position.y = camHeight;
        }
        else if (position.y < -camHeight)
        {
            velocity.y *= -1;
            position.y = -camHeight;
        }
    }


    private void OnDrawGizmos()
    {
        //Gizmos.DrawLine(transform.position, transform.position + (velocity.normalized * 2));
        Gizmos.color = Color.cyan;
        Gizmos.DrawLine(transform.position, transform.position + Velocity);
    }
}
