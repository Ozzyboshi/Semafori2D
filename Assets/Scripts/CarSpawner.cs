using UnityEngine;
using System.Collections;

public class CarSpawner : MonoBehaviour {

    public GameObject[] cars;
    public bool active=true;

	// Use this for initialization
	void Awake () 
    {
        InvokeRepeating("SpawnCar", 2, 12.5F);
	}
	
	void SpawnCar ()
    {
        if (active == false) return;
		GameObject carInstance = Instantiate(cars[Random.Range(0, cars.Length)], transform.position, Quaternion.identity) as GameObject;
		carInstance.GetComponent<CarMovement> ().showCarCrossroadStraightLine = true;
		GameManager.instance.carInstances.Add(carInstance);
		carInstance.transform.SetParent (GameObject.Find ("CarsList").transform);
	}
}
