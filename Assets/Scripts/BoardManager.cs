using UnityEngine;
using System;
using System.Collections.Generic;

public class BoardManager : MonoBehaviour {

    public const int rows = 16;
    public const int columns = 16;
    
    public GameObject road;
    public GameObject[] cars;
    public GameObject spawn_point;
    public GameObject exit;
    public GameObject grass;

    private Transform boardHolder;
    private List<Vector3> gridPosition = new List<Vector3>();

    void InitializeList()
    {
        /*gridPosition.Clear();

        for (int x = 0; x < rows; x++)
        {
            for (int y = 0; y < columns; y++)
            {
                gridPosition.Add(new Vector3(x, y, 0f));
            }
        }*/
    }

    void BoardSetup()
    {
        boardHolder = new GameObject("Board").transform;
        for (int x = 0; x < rows; x++)
        {
            for (int y = 0; y < columns; y++)
            {
                GameObject toInstantiate = grass;                 
                if (y ==3 || y == 4) toInstantiate = road;
                GameObject instance = Instantiate(toInstantiate, new Vector3(x,y,0F),Quaternion.identity) as GameObject;
                if (y == 4) instance.transform.rotation *= Quaternion.Euler(0F, 0F, 180F);
                instance.transform.SetParent(boardHolder);
            }
        }
        //for (int x = 0; x < rows; x++)
        //{
        //    GameObject carInstance = Instantiate(cars[0], new Vector3(7F, x, 0F), Quaternion.identity) as GameObject;
        //    GameManager.instance.carInstances.Add(carInstance);
        //}
        (Instantiate(exit, new Vector3(rows, columns, 0F), Quaternion.identity) as GameObject).transform.SetParent(boardHolder);
        (Instantiate(spawn_point, new Vector3(-1, 3F, 0F), Quaternion.identity) as GameObject).transform.SetParent(boardHolder);
        (Instantiate(spawn_point, new Vector3(columns, 4F, 0F), Quaternion.Euler(0F, 0F, 180F)) as GameObject).transform.SetParent(boardHolder);
        
    }

    public void SetupScene(int level) 
    {
        BoardSetup();
        InitializeList();
    }

}
