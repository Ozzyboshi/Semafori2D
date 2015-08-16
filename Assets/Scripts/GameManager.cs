using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class GameManager : MonoBehaviour {

	private int _score = 0 ;
	public int score
	{
		get { return _score; }
		set {_score = value; }
	}


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
	}

    void InitGame()
    {
        boardScript.SetupScene(level);

    }

	void OnGUI ()
	{
		GUI.Label (new Rect (50,50,100,50), "Score :"+_score);
	}
}
