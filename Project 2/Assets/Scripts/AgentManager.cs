using System.Collections;
using System.Collections.Generic;
using UnityEngine;



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
    public List<Uninfected> uninfectedList;
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
        
        this.GetComponent<PlayerController>().MainCamera = cam;

        Spawn();

        //temp 
        infectedList.Clear();

    }

    // Update is called once per frame
    void Update()
    {
        if (this.GetComponent<PlayerController>().ToSpawn)
        {
            SpawnInfected(this.GetComponent<PlayerController>().MousePosition);
            this.GetComponent<PlayerController>().ToSpawn = false;
        }


        if (this.GetComponent<PlayerController>().ToReset)
        {
            Reset();
            this.GetComponent<PlayerController>().ToReset = false;
        }
    }


    private void Spawn()
    {

        SpawnObstacles();
        SpawnUninfected();

    }

    void CleanUp()
    {
        if (uninfectedList.Count > 0)
        {
            foreach (Uninfected a in uninfectedList)
            {
                Destroy(a.gameObject);
            }
            uninfectedList.Clear();
        }
        
        if (infectedList.Count > 0)
        {
            foreach (Infected b in infectedList)
            {
                Destroy(b.gameObject);
            }

            infectedList.Clear();
        }
        
    }

    private void Reset()
    {
        CleanUp();

        SpawnUninfected();
    }

    private void SpawnUninfected()
    {
        uninfectedList.Clear();
        for (int i = 0; i < 50; i++)
        {
            uninfectedList.Add(Instantiate(uninfectedPrefab, new Vector3 (0,0,0), Quaternion.identity) );

            uninfectedList[i].GetComponent<Agent>().Manager = this.gameObject;

            
            
        }
    }

    //player click
    private void SpawnInfected(Vector2 position)
    {
        Agent infected = Instantiate(infectedPrefab, new Vector3(position.x, position.y, 0), Quaternion.identity).GetComponent<Agent>();
        infected.GetComponent<Agent>().Manager = this.gameObject;
        infectedList.Add(infected );
        
        
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
