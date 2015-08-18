using UnityEngine;
using System.Collections;

[ExecuteInEditMode]
public class TilesManager : MonoBehaviour {

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {

		if (Application.isPlaying==false)
		{
			Vector3 newVector = new Vector3(transform.position.x,transform.position.y,0.0f);
			transform.position = new Vector3(Mathf.Round(newVector.x),Mathf.Round(newVector.y-0.4f),0.0f);
		}
	}
}
