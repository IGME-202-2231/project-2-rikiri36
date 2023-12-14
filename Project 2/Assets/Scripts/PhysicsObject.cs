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
        get { return direction; }
    }

    public Vector3 Position
    {
        get { return position; }
        set { position = value; }
    }

    public Vector3 Velocity
    {
        get { return velocity; }
        set { velocity = value; }
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

    public float MaxForce { get { return maxSpeed; } }

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
        cam = FindObjectOfType<Camera>();
        camHeight = cam.orthographicSize;
        camWidth = camHeight * cam.aspect;

        position = transform.position;

        direction = Random.insideUnitCircle.normalized;
        //Debug.Log(direction);
        
    }

    // Update is called once per frame
    void Update()
    {
        //apply ALL forces first

        // Calculate the velocity for this frame - New
        velocity += acceleration * Time.deltaTime;

        velocity = Vector3.ClampMagnitude(velocity, maxSpeed);

        position += velocity * Time.deltaTime;

        transform.rotation = Quaternion.LookRotation(Vector3.back, direction);

        transform.position = position;
        direction = velocity.normalized;

        // Zero out acceleration - New
        acceleration = Vector3.zero;

        

    }


    public void ApplyForce(Vector3 force)
    {
        acceleration += force / mass;
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireCube(Vector3.zero, new Vector3(CamWidth*2 -0.5f , CamHeight*2 -2f, 0));

        //Gizmos.DrawLine(transform.position, transform.position + (velocity.normalized * 2));
        Gizmos.color = Color.cyan;
        Gizmos.DrawLine(transform.position, transform.position + Velocity);
        


        Gizmos.DrawWireSphere(transform.position, Radius);

        //Gizmos.DrawWireCube(Position, new Vector3(radius, radius, 0) );
    }


}
