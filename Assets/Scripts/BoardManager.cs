using UnityEngine;
using System;
using System.Collections.Generic;

public class BoardManager : MonoBehaviour {

    public const int rows = 16;
    public const int columns = 16;
    
    public GameObject road;
    public GameObject crossroad;
	public GameObject trafficlight;
    public GameObject[] cars;
    public GameObject spawn_point;
    public GameObject stop_point;
    public GameObject exit;
    public GameObject grass;

    private Transform boardHolder;
	private Transform crossroadList;
	private Transform singleCrossroad;
    private Transform spawners;

    void BoardSetup()
    {
        boardHolder = new GameObject("Board").transform;
		crossroadList = new GameObject("CrossroadList").transform;
		singleCrossroad = new GameObject("SingleCrossroad").transform;
		singleCrossroad.transform.SetParent (crossroadList);
        spawners = new GameObject("Spawners").transform;

        for (int x = 0; x < rows; x++)
        {
            for (int y = 0; y < columns; y++)
            {
                GameObject toInstantiate = grass;
                
				if (x==7&&y==4) { instantiateCrossroad(x,y); continue;}
				if ((x == 7 || x == 8) && (y == 3 || y == 4)) continue;
                else if (y == 3 || y == 4) toInstantiate = road;
                else if (x == 7 || x == 8) {
                    toInstantiate = road;
                    GameObject instance2 = Instantiate(toInstantiate, new Vector3(x, y, 0F), Quaternion.identity) as GameObject;
                    if (x==8) instance2.transform.rotation *= Quaternion.Euler(0F, 0F, 90F);
                    else instance2.transform.rotation *= Quaternion.Euler(0F, 0F, 270F);
                    instance2.transform.SetParent(boardHolder);
                    continue;
                }
                GameObject instance = Instantiate(toInstantiate, new Vector3(x,y,0F),Quaternion.identity) as GameObject;
                if (y == 4) instance.transform.rotation *= Quaternion.Euler(0F, 0F, 180F);
                instance.transform.SetParent(boardHolder);
            }
        }
        //for (int x = 0; x < rows; x++)
        //{
        //    GameObject carInstance = Instantiate(cars[0], new Vector3(7F, x, 0F), Quaternion.identity) as GameObject;
        //    GameManager.instance.carInstances.Add(carInstance);
        //}
        (Instantiate(exit, new Vector3(rows, columns, 0F), Quaternion.identity) as GameObject).transform.SetParent(boardHolder);
        
		(Instantiate(spawn_point, new Vector3(-1, 3F, 0F), Quaternion.identity) as GameObject).transform.SetParent(spawners);
        (Instantiate(spawn_point, new Vector3(columns, 4F, 0F), Quaternion.Euler(0F, 0F, 180F)) as GameObject).transform.SetParent(spawners);

		(Instantiate(spawn_point, new Vector3(7, columns, 0F), Quaternion.Euler(0F, 0F, 270F)) as GameObject).transform.SetParent(spawners);
		(Instantiate(spawn_point, new Vector3(8, -1, 0F), Quaternion.Euler(0F, 0F, 90F)) as GameObject).transform.SetParent(spawners);
	}

	//This function builds a crossroad with 4 tiles and wraps them up into a crossroadpair gameobject
	void instantiateCrossroad(int x,int y) 
	{
		Transform crossroadPair = new GameObject("CrossroadPair").transform;
		crossroadPair.SetParent (singleCrossroad);
		GameObject instance = Instantiate(crossroad, new Vector3(x,y,0F),Quaternion.identity) as GameObject;
		instance.transform.SetParent(crossroadPair);
		instance = Instantiate(crossroad, new Vector3(x+1,y-1,0F),Quaternion.identity) as GameObject;
		instance.transform.SetParent(crossroadPair);

		crossroadPair = new GameObject("CrossroadPair").transform;
		crossroadPair.SetParent (singleCrossroad);
		instance = Instantiate(crossroad, new Vector3(x,y-1,0F),Quaternion.identity) as GameObject;
		instance.transform.SetParent(crossroadPair);
		instance = Instantiate(crossroad, new Vector3(x+1,y,0F),Quaternion.identity) as GameObject;
		instance.transform.SetParent(crossroadPair);
	}

    void InitializeStops()
    {
		bool changeTrafficLightState;
        GameObject[] roadGameObjects;
        GameObject[] crossroadGameObjects;
        roadGameObjects = GameObject.FindGameObjectsWithTag("simpleroad");
        crossroadGameObjects = GameObject.FindGameObjectsWithTag("crossroad");
        foreach (GameObject roadObject in roadGameObjects)
        {
			changeTrafficLightState=false;
            Vector3 checkVector = roadObject.gameObject.transform.position;
			Vector3 trafficlightVector = roadObject.gameObject.transform.position;
            if (roadObject.gameObject.transform.rotation.eulerAngles.z > -0.1 && roadObject.gameObject.transform.rotation.eulerAngles.z < 0.1)
			{
				changeTrafficLightState=true;
                checkVector.x++;
				trafficlightVector.y--;
			}
            else if (roadObject.gameObject.transform.rotation.eulerAngles.z > 179.9 && roadObject.gameObject.transform.rotation.eulerAngles.z < 181.1)
			{
				changeTrafficLightState=true;
				checkVector.x--;
				trafficlightVector.y++;
			}
			else if (roadObject.gameObject.transform.rotation.eulerAngles.z > 89.9 && roadObject.gameObject.transform.rotation.eulerAngles.z < 90.1)
			{
				checkVector.y++;
				trafficlightVector.x++;
			}
			else if (roadObject.gameObject.transform.rotation.eulerAngles.z > 269.9 && roadObject.gameObject.transform.rotation.eulerAngles.z < 271.1)
			{
				checkVector.y--;
				trafficlightVector.x--;
			}
            foreach (GameObject crossroadObject in crossroadGameObjects)
            {
                if (crossroadObject.transform.position==checkVector)
                {
                    (Instantiate(stop_point, roadObject.gameObject.transform.position, roadObject.gameObject.transform.rotation) as GameObject).transform.SetParent(crossroadObject.transform);
					GameObject newTrafficLight=Instantiate(trafficlight,trafficlightVector, roadObject.gameObject.transform.rotation) as GameObject;
					newTrafficLight.transform.SetParent(crossroadObject.transform);
					if (changeTrafficLightState)
						newTrafficLight.GetComponent<TrafficLightManager>().changeTrafficlightState();
				}
            }
        }
    }

    public void SetupScene(int level) 
    {
        BoardSetup();
        InitializeStops();
    }

}
