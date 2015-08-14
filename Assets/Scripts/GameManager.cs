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
	}

    void InitGame()
    {
        boardScript.SetupScene(level);

    }
}
