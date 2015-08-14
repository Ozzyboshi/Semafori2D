using UnityEngine;
using System.Collections;

public class CarMovement : MonoBehaviour {
    private Vector3 direction;
    private float straightSpeed=3.5F;
    private bool braking = false;
	private bool still = false;
	private bool steering = false;
    public bool straightMovement;
	private LTDescr tween;

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
		Vector2 startLineCast;

        if (direction == Vector3.zero||steering==true) return;
		if (direction == Vector3.left || direction == Vector3.right)
        	startLineCast = transform.position + direction * (GetComponent<BoxCollider2D>().bounds.extents.x);
		else
			startLineCast = transform.position + direction * (GetComponent<BoxCollider2D>().bounds.extents.y);
        //Vector2 endLineCast = transform.position+(direction*3.5F);
        //Vector2 startLineCast = new Vector2(transform.position.x);
        //Vector2 endLineCast = new Vector2(transform.position.x);
        //Ray ray = new Ray(transform.position, Vector2.right);
        //RaycastHit2D hit = Physics2D.Raycast(transform.position,Vector2.right, 1 << LayerMask.NameToLayer("cars"));

		Debug.DrawRay (startLineCast, direction*4,Color.white);
        //Debug.DrawLine(startLineCast, endLineCast, Color.white);
        //RaycastHit2D hit = Physics2D.Linecast(startLineCast, endLineCast, (1 << LayerMask.NameToLayer("cars")) | (1 << LayerMask.NameToLayer("stop")));

		// This raycast should prevent the car to collide with other cars and stoppoints
		RaycastHit2D hit = Physics2D.Raycast(startLineCast, direction,4, (1 << LayerMask.NameToLayer("cars")) | (1 << LayerMask.NameToLayer("stop")));
        if (hit.collider != null)
        {
            //Debug.Log("La macchina con posizione " + transform.position + "ha urtato" + hit.collider.gameObject.transform.position + "posizione start:" + startLineCast + "bounds" + GetComponent<BoxCollider2D>().bounds.extents.x+"direction:"+direction+"tag"+hit.transform.tag);
			float distance = 0;
			if (direction == Vector3.right || direction==Vector3.left)
            	distance = Mathf.Abs(hit.point.x - transform.position.x);
			else
				distance = Mathf.Abs(hit.point.y - transform.position.y);
			//distance = hit.distance;
			//Debug.Log(hit.distance+":"+distance);
            if (hit.distance < 1.5F && braking==false) {
                //Debug.Log("1Distanza" + hit.distance + "hitpointposition" + hit.point.x);
				//LeanTween.moveX(gameObject, hit.point.x, 10F).setEase(LeanTweenType.linear).setDelay(0f);
				//tween.setTime(10.5f);
                //braking = true;
            }
            if (hit.distance < 0.2F) {
				still=true;
				LeanTween.pause(gameObject);
                //Debug.Log("2Distanza" + hit.distance+"hitpointposition"+hit.point.x);
                //LeanTween.cancel(gameObject);
                //Time.timeScale = 0.0F;
            }
			else
			{
				if (still==true)
					LeanTween.resume(gameObject);
			}

            //Time.timeScale = 0.0F;
           
            //float heightError = floatHeight - distance;
            //float force = liftForce * heightError - rigidbody2D.velocity.y * damping;
        }
		else
		{
			if (still==true)
				LeanTween.resume(gameObject);
		}
        /*if (straightMovement == true)
        {
            transform.Translate(straightSpeed*Time.deltaTime, 0, 0);
        }*/
	}

	void moveStraight(int rotation)
	{
		if (rotation==0) 
		{
			tween=LeanTween.moveX(gameObject, transform.position.x + 1F, straightSpeed / 10).setEase(LeanTweenType.linear).setDelay(0f);
			direction = Vector3.right;
		}
		else  if (rotation == 180)
		{
			tween=LeanTween.moveX(gameObject, transform.position.x - 1F, straightSpeed / 10).setEase(LeanTweenType.linear).setDelay(0f);
			direction = Vector3.left;
		}
		else if ( rotation==270)
		{
			tween=LeanTween.moveY(gameObject, transform.position.y -1, straightSpeed / 10).setEase(LeanTweenType.linear).setDelay(0f);
			direction = Vector3.down;
		}
		else if (rotation == 90)
		{
			tween=LeanTween.moveY(gameObject, transform.position.y + 1F, straightSpeed / 10).setEase(LeanTweenType.linear).setDelay(0f);
			direction = Vector3.up;
		}
	}

	// Called every time a steer has ended (on crossroad)
	void EndSteering()
	{
		moveStraight ((int)transform.rotation.eulerAngles.z);
		steering = false;
	}

	// Called every time car changes road tile
    void OnTriggerEnter2D(Collider2D other)
    {
		if (braking == true||steering==true) return;

		// If the car hits a crossroad it drifts
		if (other.tag=="crossroad")
		{
			steering=true;
			Vector3 currentPosition = transform.position;
			Vector3 middlePosition1;
			Vector3 middlePosition2;
			Vector3 endPosition;
			if (direction==Vector3.right)
			{
				middlePosition1 = new Vector3(other.transform.position.x,transform.position.y,transform.position.z);
				middlePosition2 = new Vector3(other.transform.position.x,transform.position.y,transform.position.z);
				endPosition = new Vector3(other.transform.position.x,transform.position.y-1,transform.position.z);
			}
			else if (direction==Vector3.left)
			{
				middlePosition1 = new Vector3(other.transform.position.x,transform.position.y,transform.position.z);
				middlePosition2 = new Vector3(other.transform.position.x,transform.position.y,transform.position.z);
				endPosition = new Vector3(other.transform.position.x,transform.position.y+1,transform.position.z);
			}
			else if (direction==Vector3.up)
			{
				middlePosition1 = new Vector3(other.transform.position.x,other.transform.position.y,transform.position.z);
				middlePosition2 = new Vector3(other.transform.position.x,other.transform.position.y,transform.position.z);
				endPosition = new Vector3(other.transform.position.x+1f,other.transform.position.y,transform.position.z);
			}
			else if (direction==Vector3.down)
			{
				middlePosition1 = new Vector3(other.transform.position.x,other.transform.position.y,transform.position.z);
				middlePosition2 = new Vector3(other.transform.position.x,other.transform.position.y,transform.position.z);
				endPosition = new Vector3(other.transform.position.x-1f,other.transform.position.y,transform.position.z);
			}
			LeanTween.move(gameObject, new Vector3 [] { currentPosition, middlePosition1,middlePosition2,endPosition}, 10.0F).setEase(LeanTweenType.linear).setOrientToPath2d(true).setOnComplete(EndSteering);
			return ;
		}
        
		moveStraight ((int)other.gameObject.transform.rotation.eulerAngles.z);
        transform.rotation = other.gameObject.transform.rotation;
    }

}
