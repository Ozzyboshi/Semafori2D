using UnityEngine;
using System.Collections;

public class CarSpawner : MonoBehaviour {

    public GameObject[] cars;

	// Use this for initialization
	void Start () {
        InvokeRepeating("SpawnCar", 2, 5.3F);
	}
	
	void SpawnCar () {
        GameObject carInstance = Instantiate(cars[0], transform.position, Quaternion.identity) as GameObject;
        GameManager.instance.carInstances.Add(carInstance);
	}
}
