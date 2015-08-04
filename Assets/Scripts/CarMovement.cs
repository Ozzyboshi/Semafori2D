using UnityEngine;
using System.Collections;

public class CarMovement : MonoBehaviour {
    private Vector3 direction;
    private float straightSpeed=3.5F;
    private bool braking = false;
    public bool straightMovement;

    public void StartStraightMovement()
    {
        straightMovement = true;
    }

	// Use this for initialization
	void Start () {
	}
	
	// Update is called once per frame
	void FixedUpdate () 
    {
        if (direction == Vector3.zero) return;
        Vector2 startLineCast = transform.position + direction * (GetComponent<BoxCollider2D>().bounds.extents.x+0.1F);
        Vector2 endLineCast = transform.position+(direction*3.5F);
        //Vector2 startLineCast = new Vector2(transform.position.x);
        //Vector2 endLineCast = new Vector2(transform.position.x);
        //Ray ray = new Ray(transform.position, Vector2.right);
        //RaycastHit2D hit = Physics2D.Raycast(transform.position,Vector2.right, 1 << LayerMask.NameToLayer("cars"));
        Debug.DrawLine(startLineCast, endLineCast, Color.white);
        RaycastHit2D hit = Physics2D.Linecast(startLineCast, endLineCast, 1 << LayerMask.NameToLayer("cars"));
        if (hit.collider != null)
        {
            //Debug.Log("La macchina con posizione " + transform.position + "ha urtato" + hit.collider.gameObject.transform.position + "posizione start:" + startLineCast + "posizione end:" + endLineCast + "bounds" + GetComponent<BoxCollider2D>().bounds.extents.x+"direction:"+direction);
            float floatHeight;
            float liftForce;
            float damping;
            //float distance = Mathf.Abs(hit.point.x - transform.position.x);
            if (hit.distance < 2.5F && braking==false) {
                //Debug.Log("1Distanza" + hit.distance + "hitpointposition" + hit.point.x);
                LeanTween.moveX(gameObject, transform.position.x - 1F, 0.1F).setEase(LeanTweenType.linear).setDelay(0f);
                braking = true;
            }
            else if (hit.distance < 0.5F) {
                //Debug.Log("2Distanza" + hit.distance+"hitpointposition"+hit.point.x);
                LeanTween.cancel(gameObject);
                //Time.timeScale = 0.0F;
            }
            //Time.timeScale = 0.0F;
           
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
