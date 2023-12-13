using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.VFX;


public enum Status
{
    Alive,
    Dead
}

//just as a tag
public enum InfectionStatus
{
    Uninfected,
    Infected
}

public class AgentManager : MonoBehaviour
{

    [SerializeField] public List<Obstacle> obstacles;
    public List<Agent> uninfectedList;
    [SerializeField] public List<Agent> infectedList;

    [SerializeField] private Obstacle obstaclePrefab;
    [SerializeField] private Uninfected uninfectedPrefab;
    [SerializeField] private Infected infectedPrefab;

    //camera
    Camera cam;
    float camHeight;
    float camWidth;

    // Start is called before the first frame update
    void Start()
    {

        cam = Camera.main;
        camHeight = cam.orthographicSize;
        camWidth = camHeight * cam.aspect;
        

        Spawn();

    }

    // Update is called once per frame
    void Update()
    {
        
    }


    private void Spawn()
    {
        SpawnObstacles();

        SpawnUninfected();


    }

    private void SpawnUninfected()
    {
        for (int i = 0; i < 10; i++)
        {
            uninfectedList.Add(Instantiate(uninfectedPrefab, new Vector3 (0,0,0), Quaternion.identity) );
        }
    }


    private void SpawnObstacles()
    {
        obstacles.Clear();

        //spawning in obstacles
        obstacles.Add(
            Instantiate(obstaclePrefab,
                        new Vector3(2.13f, 4.53f, 0),
                        Quaternion.identity)
            );

        obstacles.Add(
            Instantiate(obstaclePrefab,
                        new Vector3(-6.951f, -4.359f, 0),
                        Quaternion.identity)
            );

        obstacles.Add(
            Instantiate(obstaclePrefab,
                        new Vector3(-4.04f, -0.159f, 0),
                        Quaternion.identity)
            );

        obstacles.Add(
            Instantiate(obstaclePrefab,
                        new Vector3(2.51f, -0.45f, 0),
                        Quaternion.identity)
            );

        obstacles.Add(
            Instantiate(obstaclePrefab,
                        new Vector3(5.39f, 0.37f, 0),
                        Quaternion.identity)
            );
    }


    

}
