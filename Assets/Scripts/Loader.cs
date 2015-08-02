using UnityEngine;
using System.Collections;

public class Loader : MonoBehaviour {

    public GameObject gameManager;

	// Use this for initialization
	void Awake () {

        // If the singleton is not istantiated a GameManager prefab is instantiated
        if (GameManager.instance == null)
            Instantiate(gameManager);
	}
}
