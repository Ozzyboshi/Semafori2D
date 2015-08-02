using UnityEngine;
using System.Collections;

public class CarMovement : MonoBehaviour {
    private Vector3 direction;
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
        Vector3 startLineCast = transform.position + direction * (GetComponent<BoxCollider2D>().bounds.extents.x+0.1F);
        Vector3 endLineCast = transform.position+(direction*3.5F);
        Debug.DrawLine(startLineCast, endLineCast, Color.white);
        RaycastHit2D hit = Physics2D.Linecast(startLineCast, endLineCast, 1 << 9);
        if (hit.collider != null)
        {
            Debug.Log("La macchina con posizione "+transform.position+"ha urtato"+hit.collider.gameObject.transform.position);
            float floatHeight;
            float liftForce;
            float damping;
            float distance = Mathf.Abs(hit.point.y - transform.position.y) + 3.5F;
            Debug.Log("Distanza" + distance);
            if (distance < -2.5F)
                LeanTween.cancel(gameObject);
            Time.timeScale = 0.0F;
           
            //float heightError = floatHeight - distance;
            //float force = liftForce * heightError - rigidbody2D.velocity.y * damping;
        }
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
            direction = Vector3.right;
        }
        else  if (rotation == 180)
        {
            LeanTween.moveX(gameObject, transform.position.x - 1F, straightSpeed / 10).setEase(LeanTweenType.linear).setDelay(0f);
            direction = Vector3.left;
        }
        else if ( rotation==270)
        {
            LeanTween.moveY(gameObject, transform.position.y + 1F, straightSpeed / 10).setEase(LeanTweenType.linear).setDelay(0f);
            direction = Vector3.up;
        }
        else if (rotation == 90)
        {
            LeanTween.moveX(gameObject, transform.position.y - 1F, straightSpeed / 10).setEase(LeanTweenType.linear).setDelay(0f);
            direction = Vector3.down;
        }
        transform.rotation = other.gameObject.transform.rotation;
    }

}
