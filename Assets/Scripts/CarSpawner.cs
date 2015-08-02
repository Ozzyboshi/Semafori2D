using UnityEngine;
using System.Collections;

public class CarSpawner : MonoBehaviour {

    public GameObject[] cars;

	// Use this for initialization
	void Awake () 
    {
        InvokeRepeating("SpawnCar", 2, 3.5F);
	}
	
	void SpawnCar ()
    {    
        GameObject carInstance = Instantiate(cars[0], transform.position, Quaternion.identity) as GameObject;
        GameManager.instance.carInstances.Add(carInstance);
	}
}
