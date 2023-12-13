using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class Obstacle : MonoBehaviour
{
    [SerializeField] private float radius;

    public float Radius { get { return radius; } }


    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }


    private void OnDrawGizmos()
    {
        //Gizmos.DrawLine(transform.position, transform.position + (velocity.normalized * 2));
        Gizmos.color = Color.cyan;

        Gizmos.DrawWireSphere(transform.position, radius);

        //Gizmos.DrawWireCube(Position, new Vector3(radius, radius, 0) );
    }

}
