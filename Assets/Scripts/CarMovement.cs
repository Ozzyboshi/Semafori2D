using UnityEngine;
using System.Collections;

public class CarMovement : MonoBehaviour {
    private float straightSpeed=3.5F;
    //private bool straightMovement;

	public bool showCarCollisionLine = false;
	public bool showCarCrossroadStraightLine = false;
	LTDescr tweenMovement = null;
	int tweenId ;

	[SerializeField]
	private bool isTraversingCrossroad = false;
	[SerializeField]
	private bool isSteering = false;
	[SerializeField]
	private bool isStill = false;
	[SerializeField]
	private bool isBraking = false;
	[SerializeField]
	private bool isAccellerating = false;
	[SerializeField]
	private float distance = 0;
	[SerializeField]
	private Vector3 direction = Vector3.zero;
	[SerializeField]
	private float brakeDestination = 0;
	[SerializeField]
	private string stopLayerToCheck;
	[SerializeField]
	private int gameObjectCausingBrakeId;
	
	// Use this for initialization
	void Start () {

		// Debug flags
		showCarCollisionLine = true;
	}
	
	// Update is called once per frame
	void Update () 
    {
		// If the car is not moving it can't go colliding to other objects;
		if (isStill == true)
			return;
		Vector2 startLineCast;

		if (direction == Vector3.zero||isSteering==true||isTraversingCrossroad==true) return;
		if (direction == Vector3.left || direction == Vector3.right)
        	startLineCast = transform.position + direction * (GetComponent<BoxCollider2D>().bounds.extents.x);
		else
			startLineCast = transform.position + direction * (GetComponent<BoxCollider2D>().bounds.extents.y);

		if (showCarCollisionLine==true) Debug.DrawRay (startLineCast, direction*4,Color.white);

		// This raycast should prevent the car to collide with other cars and stoppoints
		RaycastHit2D hit = Physics2D.Raycast(startLineCast, direction,4, (1 << LayerMask.NameToLayer("cars")) | (1 << LayerMask.NameToLayer(stopLayerToCheck)));
        if (hit.collider != null)
        {
            //Debug.Log("La macchina con posizione " + transform.position + "ha urtato" + hit.collider.gameObject.transform.position + "posizione start:" + startLineCast + "bounds" + GetComponent<BoxCollider2D>().bounds.extents.x+"direction:"+direction+"tag"+hit.transform.tag);
			/*float distance = 0;
			if (direction == Vector3.right || direction==Vector3.left)
            	distance = Mathf.Abs(hit.point.x - transform.position.x);
			else
				distance = Mathf.Abs(hit.point.y - transform.position.y);
			//distance = hit.distance;
			//Debug.Log(hit.distance+":"+distance);*/
			distance = getDistanceFromRaycastHit2D(hit);
            if (distance < 0.5F) 
			{
				StopMovement();
            }
			else if ( distance < 1.5F && isBraking==false) {
				//Debug.Log("1Distanza" + hit.distance + "hitpointposition" + hit.point.x);
				//LeanTween.moveX(gameObject, hit.point.x, 10F).setEase(LeanTweenType.linear).setDelay(0f);
				//tween.setTime(10.5f);
				//braking = true;

				Brake(hit.collider);
			}
			else
			{
				//if (isStill==true)
				//	AccellerateStraight();
			}
        }
		else
		{
			//if (isStill==true)
			//	AccellerateStraight();
		}
	}
	// Called to stop car movement
	void StopMovement()
	{
		//LeanTween.cancel(gameObject);
		LeanTween.cancel (gameObject);
		isStill=true;
		isBraking = false;
		StartCoroutine (WaitForResumeMovement (1));
	}

	// Called to brake the car because an object (traffic light or another car) is near
	void Brake(Collider2D collider)
	{
		Vector3 destination = collider.gameObject.transform.position;

		// Don't brake if the object ahead is a traversing crossroad car
		if (collider.gameObject.tag=="car")
		{
			CarMovement movement = collider.gameObject.GetComponent<CarMovement>();
			//if (movement.isTraversingCrossroad==true||movement.isSteering) return ;
			if (movement.isStill==false) return ;
		}

		// Start of braking procedure
		gameObjectCausingBrakeId=collider.gameObject.GetInstanceID();
		isBraking = true;
		LeanTween.cancel (gameObject);
		if (direction== Vector3.up || direction == Vector3.down) 
		{
			brakeDestination=destination.y;
			LeanTween.moveY (gameObject, brakeDestination, 5).setEase (LeanTweenType.linear).setDelay (0).setOnComplete (EndBraking);
		}
		else if (direction== Vector3.left || direction == Vector3.right) 
		{
			brakeDestination=destination.x;
			LeanTween.moveX (gameObject, brakeDestination, 5).setEase (LeanTweenType.linear).setDelay (0).setOnComplete (EndBraking);
		}

		/*if (direction == Vector3.down)
			LeanTween.moveY (gameObject, transform.position.y - destination.y, 30).setEase (LeanTweenType.easeOutBack).setDelay (0).setOnComplete (EndBraking);
		else if (direction == Vector3.up)
			LeanTween.moveY (gameObject, destination.y, 30).setEase (LeanTweenType.easeOutBack).setDelay (0).setOnComplete (EndBraking);
		else if (direction== Vector3.right)
			LeanTween.moveX (gameObject, transform.position.x + destination.x, 30).setEase (LeanTweenType.easeOutBack).setDelay (0).setOnComplete (EndBraking);
		else if (direction == Vector3.left)
			LeanTween.moveX (gameObject, transform.position.x - destination.x, 30).setEase (LeanTweenType.easeOutBack).setDelay (0).setOnComplete (EndBraking);
		*/
	}

	// Called for moving between tiles
	void continueMoving(int rotation,int squares)
	{
		if (isTraversingCrossroad == true)
			return;

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

		moveStraight (rotation, 0, straightSpeed/5, LeanTweenType.linear,squares);
		isStill = false;
	}

	// Called afer a stop has occured (stoppoint met or other car collision)
	void AccellerateStraight() 
	{
		if (isTraversingCrossroad == true)
			return;

		if (isSteering == true)
			return ;
		moveStraight ((int)transform.rotation.eulerAngles.z, 1, straightSpeed, LeanTweenType.easeInQuad,1).setOnComplete(EndAccellerating);
		isAccellerating=true;
		isStill=false;
	}

	// This function moves a car in a certain time after a certain delay in a certain tweentype based on the rotation passed
	// It is called for resuming car movement after a stop (stoppoint met or other car collision) or for tile switching while moving
	// This function comutes straight movements only based on the input rotation
	LTDescr moveStraight(int rotation,float delay,float time,LeanTweenType tweenType,int squares)
	{
		if (isTraversingCrossroad == true)
			return null;
		if (isSteering == true)
			return null;

		float movementSpace = (float)squares * 1F;

		if (rotation == 0) {
			tweenMovement = LeanTween.moveX (gameObject, transform.position.x + movementSpace, time).setEase (tweenType).setDelay (delay);
			direction = Vector3.right;
			stopLayerToCheck="stopleft";
		} else  if (rotation == 180) {
			tweenMovement = LeanTween.moveX (gameObject, transform.position.x - movementSpace, time).setEase (tweenType).setDelay (delay);
			direction = Vector3.left;
			stopLayerToCheck="stopright";
		} else if (rotation == 270) {
			tweenMovement = LeanTween.moveY (gameObject, transform.position.y - movementSpace, time).setEase (tweenType).setDelay (delay);
			direction = Vector3.down;
			stopLayerToCheck="stopup";
		} else if (rotation == 90) {
			tweenMovement = LeanTween.moveY (gameObject, transform.position.y + movementSpace, time).setEase (tweenType).setDelay (delay);
			direction = Vector3.up;
			stopLayerToCheck="stopdown";
		} else
			Debug.Log ("rotazione anomala : " + rotation);
		tweenId = tweenMovement.id;
		return tweenMovement;
	}

	// Called every time an accelleration has ended
	void EndAccellerating()
	{
		isAccellerating = false;
		continueMoving ((int)transform.rotation.eulerAngles.z,1);
	}

	// Called every time an accelleration has ended
	void EndBraking()
	{
		isBraking = false;
		gameObjectCausingBrakeId = 0;
		continueMoving ((int)transform.rotation.eulerAngles.z,1);
	}

	// Called every time a car exit a crossroad to resume normal movement
	void EndCrossroadMovement()
	{
		isSteering = false;
		isTraversingCrossroad = false;
		continueMoving ((int)transform.rotation.eulerAngles.z,1);
	}

	// Called every time car changes road tile
    void OnTriggerEnter2D(Collider2D other)
    {
		/*Debug.Log ("steering" + isSteering);
		Debug.Log ("still" + isStill);
		Debug.Log ("braking" + isBraking);*/
		if (isSteering==true||isTraversingCrossroad==true||isBraking==true) return;

		// If the car hits a crossroad
		if (other.tag=="crossroad")
		{
			// 0 means go straight, 1 means steer
			int directionChoice = Random.Range(0, 2);
			//directionChoice=1;
			if (directionChoice==0)
			{
				// Go straight through the crossroad
				GoOtherSideSCrossroadWhenAvailable(other);
				return ;
			}
			else {
				// Steer
				SteerSideSCrossroadWhenAvailable(other);
				return;
			}
		}
        
		continueMoving ((int)other.gameObject.transform.rotation.eulerAngles.z,1);
        transform.rotation = other.gameObject.transform.rotation;
    }

	IEnumerator WaitForCrossroadToBeReady(float duration,Vector2 start,Vector2 end,Collider2D other)
	{
		RaycastHit2D hit = Physics2D.Linecast(start,end, (1 << LayerMask.NameToLayer("cars")));
		Debug.DrawLine(start,end,Color.red);
		while (hit) {
			//CarMovement otherCar = hit.collider.gameObject.GetComponent<CarMovement>();
			yield return new WaitForSeconds(duration);   //Wait
			hit = Physics2D.Linecast(start,end, (1 << LayerMask.NameToLayer("cars")));
			Debug.DrawLine(start,end,Color.red);
		}
		isTraversingCrossroad = true;
		Steer (other);
	}

	IEnumerator WaitForCrossroadToBeReady2(float duration,Vector2 start,Vector2 end,Collider2D other)
	{
		RaycastHit2D hit = Physics2D.Linecast(start,end, (1 << LayerMask.NameToLayer("cars")));
		Debug.DrawLine(start,end,Color.red);
		while (hit) 
		{
			yield return new WaitForSeconds(duration);   //Wait
			hit = Physics2D.Linecast(start,end, (1 << LayerMask.NameToLayer("cars")));
			Debug.DrawLine(start,end,Color.red);
		}
		
		//continueMoving ((int)other.gameObject.transform.rotation.eulerAngles.z,2);
		moveStraight ((int)other.gameObject.transform.rotation.eulerAngles.z, 0, straightSpeed/5*3, LeanTweenType.linear,3).setOnComplete(EndCrossroadMovement);
		isTraversingCrossroad = true;
		isStill = false;
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
		isTraversingCrossroad = true;
		LeanTween.move(gameObject, new Vector3 [] { currentPosition, middlePosition1,middlePosition2,endPosition}, 5.0F).setEase(LeanTweenType.linear).setOrientToPath2d(true).setOnComplete(EndCrossroadMovement);

		return ;
	}

	void SteerSideSCrossroadWhenAvailable(Collider2D other) {
		Vector2 endLineCast=new Vector2(0,0);
		if (direction == Vector3.right)
			endLineCast=new Vector3(other.transform.position.x,transform.position.y-1.5F);
		else if (direction == Vector3.left)
			endLineCast=new Vector3(other.transform.position.x,transform.position.y+1.5F);
		else if (direction == Vector3.up)
			endLineCast=new Vector3(other.transform.position.x+1.5F,other.transform.position.y);
		else if (direction == Vector3.down)
			endLineCast=new Vector3(other.transform.position.x-1.5F,other.transform.position.y);
		
		
		RaycastHit2D hit = Physics2D.Linecast(other.transform.position,endLineCast, (1 << LayerMask.NameToLayer("cars")));
		Debug.DrawLine(other.transform.position,endLineCast,Color.red);
		//Pause();
		if (hit.collider!=null)
		{
			LeanTween.cancel(gameObject);
			isSteering=true;
			StartCoroutine(WaitForCrossroadToBeReady(1,other.transform.position,endLineCast,other));
			return ;
		}
		isTraversingCrossroad = true;
		Steer(other);
		return ;
	}

	void GoOtherSideSCrossroadWhenAvailable(Collider2D other) {

		if (isTraversingCrossroad == true)
			return;
		Vector2 endLineCast=new Vector2(0,0);
		if (direction == Vector3.right)
			endLineCast=new Vector3(other.transform.position.x+3,transform.position.y);
		else if (direction == Vector3.left)
			endLineCast=new Vector3(other.transform.position.x-3,transform.position.y);
		else if (direction == Vector3.up)
			endLineCast=new Vector3(other.transform.position.x,transform.position.y+3);
		else if (direction == Vector3.down)
			endLineCast=new Vector3(other.transform.position.x,transform.position.y-3);

		RaycastHit2D hit = Physics2D.Linecast(other.transform.position,endLineCast, (1 << LayerMask.NameToLayer("cars")));
		if (showCarCrossroadStraightLine) Debug.DrawLine(other.transform.position,endLineCast,Color.green);
		if (hit.collider!=null)
		{
			//Debug.Log ("sto collidendo connnnn "+hit.collider.gameObject.tag);
			//Time.timeScale = 0.0F;

			LeanTween.cancel(gameObject);
			StartCoroutine(WaitForCrossroadToBeReady2(1,other.transform.position,endLineCast,other));
			return ;
		}

		moveStraight ((int)other.gameObject.transform.rotation.eulerAngles.z, 0, straightSpeed/5*3, LeanTweenType.linear,3).setOnComplete(EndCrossroadMovement);
		isTraversingCrossroad = true;
		isStill = false;
	}



	IEnumerator WaitForResumeMovement(float duration)
	{
		Vector2 startLineCast;
		if (direction == Vector3.left || direction == Vector3.right)
			startLineCast = transform.position + direction * (GetComponent<BoxCollider2D>().bounds.extents.x);
		else
			startLineCast = transform.position + direction * (GetComponent<BoxCollider2D>().bounds.extents.y);
		
		if (showCarCollisionLine==true) Debug.DrawRay (startLineCast, direction*4,Color.white);
		RaycastHit2D hit = Physics2D.Raycast(startLineCast, direction,4, (1 << LayerMask.NameToLayer("cars")) | (1 << LayerMask.NameToLayer(stopLayerToCheck)));
		while (hit && getDistanceFromRaycastHit2D(hit)<0.5F) 
		{
			yield return new WaitForSeconds(duration);
			hit = Physics2D.Raycast(startLineCast, direction,4, (1 << LayerMask.NameToLayer("cars")) | (1 << LayerMask.NameToLayer(stopLayerToCheck)));
			if (showCarCollisionLine==true) Debug.DrawRay (startLineCast, direction*4,Color.white);
		}
		continueMoving ((int)transform.rotation.eulerAngles.z,1);
		isStill = false;
		isBraking = false;
	}

	void Pause() {Time.timeScale = 0.0F;}

	float getDistanceFromRaycastHit2D(RaycastHit2D hit)
	{
		if (direction == Vector3.right || direction==Vector3.left)
			return Mathf.Abs(hit.point.x - transform.position.x);
		return Mathf.Abs(hit.point.y - transform.position.y);
	}
}
