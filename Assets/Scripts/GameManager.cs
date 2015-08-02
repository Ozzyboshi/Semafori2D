using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class GameManager : MonoBehaviour {

    public BoardManager boardScript;
    public int level = 1;

    public List<GameObject> carInstances;

    public static GameManager instance = null;

	// Use this for initialization
	void Awake () {

        // Make a singleton
        if (instance == null)
            instance = this;
        else if (instance != this)
            Destroy(gameObject);

        // We want to retain this object through the scenes to preserve data like the score
        DontDestroyOnLoad(gameObject);

        boardScript = GetComponent<BoardManager>();
        InitGame();

        //foreach (GameObject car in carInstances)
        //{
           // CarMovement movement = car.GetComponent<CarMovement>();
            //movement.StartStraightMovement();
        //}
	}

    void InitGame()
    {
        boardScript.SetupScene(level);

    }
	
	// Update is called once per frame
	void Update () {
        //if (carInstances!=null)
        //{
            //foreach  (GameObject car in carInstances) {
                //Transform carposition = car.transform;
                //carposition.Translate(3.5F * Time.deltaTime, 0, 0);
                //LeanTween.moveX(car, 1f, 1f).setEase(LeanTweenType.easeInQuad).setDelay(1f);
                //Vector3 currentPosition = car.transform.position;
                //Vector3 endPosition = car.transform.position;
                //LeanTween.move(car, new Vector3 [] { new Vector3(0, 0,0), new Vector3(1,0,0), new Vector3(1, 0,0), new Vector3(1,1,0) }, 1.0F).setEase(LeanTweenType.easeInQuad).setOrientToPath2d(true);
              //  break;

            //}
            
        //}
	}
}
