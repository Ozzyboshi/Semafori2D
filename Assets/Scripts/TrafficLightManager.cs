using UnityEngine;
using System.Collections;

public class TrafficLightManager : MonoBehaviour {

	public Sprite greenSprite;
	public Sprite redSprite;



	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update ()
	{
		/*bool moving = false;
		GameObject go;
		if(Input.touchCount == 1)
		{    
			// touch on screen
			if(Input.GetTouch(0).phase == TouchPhase.Began)
			{
				Ray ray = Camera.main.ScreenPointToRay(Input.GetTouch(0).position);
				RaycastHit hit = new RaycastHit();
				moving = Physics.Raycast (ray, out hit);
				if(moving)
				{
					go = hit.transform.gameObject;
					Debug.Log("Touch Detected on : "+go.name);
				}
			}
		}*/

	}

	public void changeTrafficlightState(){
		changeTrafficlightObjectState (this.gameObject);
	}

	void changeTrafficlightObjectState(GameObject gm) 
	{ 
		if (gm.GetComponent<SpriteRenderer>().sprite == greenSprite)
		{
			gm.GetComponent<SpriteRenderer>().sprite = redSprite;
			transform.parent.GetChild(0).GetComponent<BoxCollider2D>().enabled=false;
		}
		else
		{
			gm.GetComponent<SpriteRenderer>().sprite = greenSprite;
			transform.parent.GetChild(0).GetComponent<BoxCollider2D>().enabled=false;
			Debug.Log (transform.parent.GetChild(0).tag);
		}
	}
	void OnMouseDown() 
	{
		foreach(Transform crossroadPair in transform.parent.parent)
		{
			if (crossroadPair.tag=="crossroad") 
			{
				for (int i=0;i<crossroadPair.childCount;i++)
				{
					Transform trafficLight=crossroadPair.GetChild(i);
					if (trafficLight.tag=="trafficlight")
					{
						changeTrafficlightObjectState(trafficLight.gameObject);

						if (trafficLight.GetInstanceID() == transform.GetInstanceID())
						{
							int index=transform.parent.parent.GetSiblingIndex();
							Transform otherTrafficlightPair;
							if (index==0)
								otherTrafficlightPair = transform.parent.parent.parent.GetChild(index + 1);
							else
								otherTrafficlightPair = transform.parent.parent.parent.GetChild(index - 1);

							for (int j=0;j<otherTrafficlightPair.childCount;j++)
							{
								Transform otherTrafficLight=otherTrafficlightPair.GetChild(j);
								if (otherTrafficLight.tag=="crossroad") 
								{
									for (int z=0;z<otherTrafficLight.childCount;z++)
									{

										if (otherTrafficLight.GetChild(z).tag=="trafficlight")
										{
											changeTrafficlightObjectState(otherTrafficLight.GetChild(z).gameObject);
											//Debug.Log(otherTrafficLight.GetChild(z));
										}
									}

									//changeTrafficlightState(otherTrafficLight.gameObject);
								}


							}
						}
						/*Transform otherTrafficlightPair = transform.parent.parent.GetChild(index + 1);
						if (otherTrafficlightPair==null)
							otherTrafficlightPair = transform.parent.GetChild(index - 1);

						for (int j=0;j<otherTrafficlightPair.childCount;j++)
						{
							Transform otherTrafficLight=crossroadPair.GetChild(i);
							Debug.Log (otherTrafficLight.tag);
							//changeTrafficlightState(otherTrafficLight.gameObject);
						}*/

					}
				}
			}
		}
	}
}
