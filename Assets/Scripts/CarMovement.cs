using UnityEngine;
using System.Collections;

public class CarMovement : MonoBehaviour {
    private Vector3 direction;
    private float straightSpeed=3.5F;
    private bool braking = false;
	private bool isStill = false;
	private bool isSteering = false;
	private bool isAccellerating = false;
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

		if (direction == Vector3.zero||isSteering==true) return;
		if (direction == Vector3.left || direction == Vector3.right)
        	startLineCast = transform.position + direction * (GetComponent<BoxCollider2D>().bounds.extents.x);
		else
			startLineCast = transform.position + direction * (GetComponent<BoxCollider2D>().bounds.extents.y);

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
				StopMovement();

				//LeanTween.pause(gameObject);
                //Debug.Log("2Distanza" + hit.distance+"hitpointposition"+hit.point.x);
                //LeanTween.cancel(gameObject);
                //Time.timeScale = 0.0F;
            }
			else
			{
				if (isStill==true)
					AccellerateStraight();
			}

            //Time.timeScale = 0.0F;
           
            //float heightError = floatHeight - distance;
            //float force = liftForce * heightError - rigidbody2D.velocity.y * damping;
        }
		else
		{
			if (isStill==true)
				AccellerateStraight();
		}
        /*if (straightMovement == true)
        {
            transform.Translate(straightSpeed*Time.deltaTime, 0, 0);
        }*/
	}

	/*void moveStraight(int rotation)
	{
		if (isAccellerating == true)
			return;
		LeanTween.cancel (gameObject);
		if (rotation == 0) {
			tween = LeanTween.moveX (gameObject, transform.position.x + 1F, straightSpeed / 10).setEase (LeanTweenType.linear).setDelay (0f);
			direction = Vector3.right;
			isStill=false;
		} else  if (rotation == 180) {
			tween = LeanTween.moveX (gameObject, transform.position.x - 1F, straightSpeed / 10).setEase (LeanTweenType.linear).setDelay (0f);
			direction = Vector3.left;
			isStill=false;
		} else if (rotation == 270) {
			tween = LeanTween.moveY (gameObject, transform.position.y - 1, straightSpeed / 10).setEase (LeanTweenType.linear).setDelay (0f);
			direction = Vector3.down;
			isStill=false;
		} else if (rotation == 90) {
			tween = LeanTween.moveY (gameObject, transform.position.y + 1F, straightSpeed / 10).setEase (LeanTweenType.linear).setDelay (0f);
			direction = Vector3.up;
			isStill=false;
		} else
			Debug.Log ("rotazione anomala : " + rotation);
	}*/

	// Called to stop car movement
	void StopMovement()
	{
		isStill=true;
		LeanTween.cancel(gameObject);
	}

	// Called for moving between tiles
	void continueMoving(int rotation)
	{
		if (rotation >= 359 || rotation<=1)
			rotation = 0;
		else if (rotation >= 89 && rotation<=91)
			rotation = 90;
		else if (rotation >= 269 && rotation<=271)
			rotation = 270;
		else if (rotation >= 179 && rotation<=181)
			rotation = 180;
		if (rotation!=0 && rotation!=90 && rotation!=180 && rotation!=270) {
			Debug.Log ("wrong rotation "+rotation);
			return ;
		}

		moveStraight (rotation, 0, straightSpeed/5, LeanTweenType.linear);
		isStill = false;
	}

	// Called afer a stop has occured (stoppoint met or other car collision)
	void AccellerateStraight() 
	{
		if (isSteering == true)
			return ;
		moveStraight ((int)transform.rotation.eulerAngles.z, 1, straightSpeed, LeanTweenType.easeInQuad).setOnComplete(EndAccellerating);
		isAccellerating=true;
		isStill=false;
	}

	// This function moves a car in a certain time after a certain delay in a certain tweentype based on the rotation passed
	// It is called for resuming car movement after a stop (stoppoint met or other car collision) or for tile switching while moving
	// This function comutes straight movements only based on the input rotation
	LTDescr moveStraight(int rotation,float delay,float time,LeanTweenType tweenType)
	{
		if (isSteering == true)
			return null;
		LTDescr tweenMovement = null;
		if (rotation == 0) {
			tweenMovement = LeanTween.moveX (gameObject, transform.position.x + 1F, time).setEase (tweenType).setDelay (delay);
			direction = Vector3.right;
		} else  if (rotation == 180) {
			tweenMovement = LeanTween.moveX (gameObject, transform.position.x - 1F, time).setEase (tweenType).setDelay (delay);
			direction = Vector3.left;
		} else if (rotation == 270) {
			tweenMovement = LeanTween.moveY (gameObject, transform.position.y - 1, time).setEase (tweenType).setDelay (delay);
			direction = Vector3.down;
		} else if (rotation == 90) {
			tweenMovement = LeanTween.moveY (gameObject, transform.position.y + 1F, time).setEase (tweenType).setDelay (delay);
			direction = Vector3.up;
		} else
			Debug.Log ("rotazione anomala : " + rotation);
		return tweenMovement;
	}

	// Called every time an accelleration has ended
	void EndAccellerating()
	{
		isAccellerating = false;
		continueMoving ((int)transform.rotation.eulerAngles.z);
	}

	// Called every time a steer has ended (on crossroad)
	void EndSteering()
	{
		isSteering = false;
		continueMoving ((int)transform.rotation.eulerAngles.z);

	}

	// Called every time car changes road tile
    void OnTriggerEnter2D(Collider2D other)
    {
		if (braking == true||isSteering==true) return;

		// If the car hits a crossroad it drifts
		if (other.tag=="crossroad")
		{
			isSteering=true;
			// Check if the other side of the road is available - to be finished
			Vector2 endLineCast=new Vector2(0,0);
			if (direction == Vector3.right)
				endLineCast=new Vector3(other.transform.position.x,transform.position.y-1);
			else if (direction == Vector3.left)
				endLineCast=new Vector3(other.transform.position.x,transform.position.y+1);
			else if (direction == Vector3.up)
				endLineCast=new Vector3(other.transform.position.x+1,transform.position.y);
			else if (direction == Vector3.down)
				endLineCast=new Vector3(other.transform.position.x-1,transform.position.y);


			RaycastHit2D hit = Physics2D.Linecast(other.transform.position,endLineCast, (1 << LayerMask.NameToLayer("cars")));
			Debug.DrawLine(other.transform.position,endLineCast,Color.red);
			if (hit.collider!=null)
			{
				LeanTween.cancel(gameObject);
				isSteering=true;
				StartCoroutine(WaitForCrossroadToBeReady(1,other.transform.position,endLineCast,other));
				return ;
				//	Debug.Log("sto hittando"+hit.collider.gameObject.transform.tag);
			//		return ;
			}
			Steer(other);

			/*Vector3 currentPosition = transform.position;
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
				endPosition = new Vector3(transform.position.x+1f,other.transform.position.y,transform.position.z);
			}
			else if (direction==Vector3.down)
			{
				middlePosition1 = new Vector3(other.transform.position.x,other.transform.position.y,transform.position.z);
				middlePosition2 = new Vector3(other.transform.position.x,other.transform.position.y,transform.position.z);
				endPosition = new Vector3(other.transform.position.x-1f,other.transform.position.y,transform.position.z);
			}
			if (isAccellerating)
			{
				isAccellerating=false;
				LeanTween.cancel(gameObject);
			}
			isSteering=true;
			LeanTween.move(gameObject, new Vector3 [] { currentPosition, middlePosition1,middlePosition2,endPosition}, 5.0F).setEase(LeanTweenType.linear).setOrientToPath2d(true).setOnComplete(EndSteering);
			*/
			return ;
		}
        
		if (isAccellerating == true)
			return;
		continueMoving ((int)other.gameObject.transform.rotation.eulerAngles.z);
        transform.rotation = other.gameObject.transform.rotation;
    }

	IEnumerator WaitForCrossroadToBeReady(float duration,Vector2 start,Vector2 end,Collider2D other)
	{
		RaycastHit2D hit = Physics2D.Linecast(start,end, (1 << LayerMask.NameToLayer("cars")));
		Debug.DrawLine(start,end,Color.red);
		//Debug.Log ("valuto");
		while (hit) {
			//Time.timeScale = 0.0F;
			CarMovement otherCar = hit.collider.gameObject.GetComponent<CarMovement>();
			//Debug.Log("sto hittandoooooooooooooooo"+hit.collider.gameObject.transform.tag+otherCar.isSteering);
			//Debug.Log(hit.collider.gameObject.GetInstanceID()+"--"+gameObject.GetInstanceID());
			yield return new WaitForSeconds(duration);   //Wait
			hit = Physics2D.Linecast(start,end, (1 << LayerMask.NameToLayer("cars")));
			Debug.DrawLine(start,end,Color.red);
		}
		//Debug.Log("Ora partirei: "+Time.time);
		//Time.timeScale = 0.0F;
		Steer (other);
	}

	void Steer(Collider2D other) 
	{
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
			endPosition = new Vector3(transform.position.x+1f,other.transform.position.y,transform.position.z);
		}
		else if (direction==Vector3.down)
		{
			middlePosition1 = new Vector3(other.transform.position.x,other.transform.position.y,transform.position.z);
			middlePosition2 = new Vector3(other.transform.position.x,other.transform.position.y,transform.position.z);
			endPosition = new Vector3(other.transform.position.x-1f,other.transform.position.y,transform.position.z);
		}
		if (isAccellerating)
		{
			isAccellerating=false;
			LeanTween.cancel(gameObject);
		}
		isSteering=true;
		//Debug.Log ("si parte");
		LeanTween.move(gameObject, new Vector3 [] { currentPosition, middlePosition1,middlePosition2,endPosition}, 5.0F).setEase(LeanTweenType.linear).setOrientToPath2d(true).setOnComplete(EndSteering);

		return ;
	}

}
