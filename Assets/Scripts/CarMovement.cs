using UnityEngine;
using System.Collections;

public class CarMovement : MonoBehaviour {

    private float straightSpeed=3.5F;
    public bool straightMovement;

    public void StartStraightMovement()
    {
        straightMovement = true;
    }

	// Use this for initialization
	void Start () {
	}
	
	// Update is called once per frame
	void Update () 
    {
        Vector3 forward = transform.TransformDirection(Vector3.forward) * 10;
        Debug.DrawRay(transform.position, forward, Color.white);
        Debug.Log(transform.position+":"+forward);
        if (straightMovement == true)
        {
            transform.Translate(straightSpeed*Time.deltaTime, 0, 0);
        }
	}

    void OnTriggerEnter2D(Collider2D other)
    {
        int rotation = (int) other.gameObject.transform.rotation.eulerAngles.z;
        
        if (rotation==0) 
        {
            LeanTween.moveX(gameObject, transform.position.x + 1F, straightSpeed / 10).setEase(LeanTweenType.linear).setDelay(0f);
        }
        else  if (rotation == 180)
        {
            LeanTween.moveX(gameObject, transform.position.x - 1F, straightSpeed / 10).setEase(LeanTweenType.linear).setDelay(0f);
        }
        else if ( rotation==270)
        {
            LeanTween.moveY(gameObject, transform.position.y + 1F, straightSpeed / 10).setEase(LeanTweenType.linear).setDelay(0f);
        }
        else if (rotation == 90)
        {
            LeanTween.moveX(gameObject, transform.position.y - 1F, straightSpeed / 10).setEase(LeanTweenType.linear).setDelay(0f);
        }
        transform.rotation = other.gameObject.transform.rotation;
    }

}
