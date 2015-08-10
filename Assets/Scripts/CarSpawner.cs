using UnityEngine;
using System.Collections;

public class CarSpawner : MonoBehaviour {

    public GameObject[] cars;
    public bool active=true;

	// Use this for initialization
	void Awake () 
    {
        InvokeRepeating("SpawnCar", 2, 23.5F);
	}
	
	void SpawnCar ()
    {
        if (active == false) return;
        GameObject carInstance = Instantiate(cars[0], transform.position, Quaternion.identity) as GameObject;
        GameManager.instance.carInstances.Add(carInstance);
	}
}
