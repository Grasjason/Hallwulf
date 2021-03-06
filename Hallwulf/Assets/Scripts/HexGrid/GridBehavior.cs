﻿using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GridBehavior : MonoBehaviour
{
    public bool findDistance = false;
    public int rows = 40;
    public int columns = 40;
    public int scale = 1;

    public GameObject hexPrefab;
    public dFloor Floor;
    public dBase hexBase;
    public dRoad hexRoad;
    public dSpawn hexSpawn;

    public GameObject baseObject;
    public GameObject spawnObject;

    public Vector3 leftBottomLocation = new Vector3(0, 0, 0);

    public GameObject[,] gridArray;
    public List<GameObject> baseList = new List<GameObject>();
    public List<GameObject> spawnList = new List<GameObject>();

    public int startX = 0;
    public int startY = 0;
    public int endX = 2;
    public int endY = 2;

    // Permet d'assurer l'emboitage des Hexagones avec le bon décalage X,Z
    float tileXOffset = 1.0f;
    float tileZOffset = 0.864f;

    public List<GameObject> path = new List<GameObject>();

    // Start is called before the first frame update
    void Awake()
    {
        gridArray = new GameObject[columns, rows];
        if (Floor.dFloorHexTile)
        {
            GenerateGrid();
            SetBasableAndSpawnable(gridArray);
            ConvertToBaseAndSpawn();
        }            
        else print("Assigner a notre GridGenerator une data dFloor.");
    }

    // Update is called once per frame
    void Update()
    {
        if (findDistance)
        {
            SetDistance();
            SetPath();
            findDistance = false;
        }
    }

    void GenerateGrid()
    {
        // On instantie la Grille avec des hexPrefab et on alimente une table d'Objet pour chaque Position [x,y]
        for (int i = 0; i < columns; i++)
        {
            for (int j = 0; j < rows; j++)
            {
                GameObject Object = Instantiate(Floor.dFloorHexTile);

                if (j % 2 == 0)
                {
                    Object.transform.position = new Vector3(i * tileXOffset, leftBottomLocation.y, j * tileZOffset);
                }
                else
                {
                    Object.transform.position = new Vector3(i * tileXOffset + tileXOffset / 2, leftBottomLocation.y, j * tileZOffset);
                }
                SetTileInfo(Object, i, j);
                Object.transform.SetParent(gameObject.transform);

                Object.GetComponent<GridStats>().x = i;
                Object.GetComponent<GridStats>().y = j;
                Object.GetComponent<Renderer>().material.SetColor("_Color", Floor.dFloorColor);
                gridArray[i, j] = Object;
            }
        }
    }

    // Dans le cas ou la case répond au besoins de placement d'un Spawn ou Bas alors on set sa variable Basable ou Spawnable a 1.
    public void SetBasableAndSpawnable(GameObject[,] gridSetBasable)
    {
        // Correspond a une ligne pour une Base a 5% de la bordure Basse Arrondi au supérieur pour gérer les superficies Impair
        int baseYLimit = (int)System.Math.Floor(rows * 0.05);
        int XLimit = (int)System.Math.Floor(columns * 0.05);
        int spawnYLimit = (int)System.Math.Floor(rows - (rows * 0.05));

        for (int l = 1; l < rows * columns; l++)
        {
            //Pour chaque objet
            foreach (GameObject Obj in gridSetBasable)
            {
                //Si la position de l'objet convient au filtre 5% du bas et 5 % des bords
                if (Obj && Obj.GetComponent<GridStats>().y == baseYLimit && Obj.GetComponent<GridStats>().x >= XLimit && Obj.GetComponent<GridStats>().x <= (columns - XLimit))
                {
                    //Turn into Basable
                    Obj.GetComponent<GridStats>().basable = 1;
                    baseList.Add(Obj);
                }
                if (Obj && Obj.GetComponent<GridStats>().y == spawnYLimit && Obj.GetComponent<GridStats>().x >= XLimit && Obj.GetComponent<GridStats>().x <= (columns - XLimit))
                {
                    //Turn into Basable
                    Obj.GetComponent<GridStats>().spawnable = 1;
                    spawnList.Add(Obj);
                }
            }
        }
    }

    void ConvertToBaseAndSpawn()
    {

        GameObject localBaseObject = baseList[UnityEngine.Random.Range(0, baseList.Count)];
        GameObject localSpawnObject = spawnList[UnityEngine.Random.Range(0, spawnList.Count)];

        //for (int l = 1; l < rows * columns; l++)
        //{
            //Pour chaque objet
            foreach (GameObject Obj in gridArray)
            {

                if (Obj && Obj.GetComponent<GridStats>().spawnable == 1 && Obj.GetComponent<GridStats>().x == localSpawnObject.GetComponent<GridStats>().x && Obj.GetComponent<GridStats>().y == localSpawnObject.GetComponent<GridStats>().y)
                {
                    //Obj.GetComponent<GridStats>().ConvertToSpawn();
                    spawnObject = Obj.GetComponent<GridStats>().SwitchTileToSpawn(hexSpawn, gridArray);
                }
                else
                {
                    Obj.GetComponent<GridStats>().spawnable = 0;
                }
                if (Obj && Obj.GetComponent<GridStats>().basable == 1 && Obj.GetComponent<GridStats>().x == localBaseObject.GetComponent<GridStats>().x && Obj.GetComponent<GridStats>().y == localBaseObject.GetComponent<GridStats>().y)
                {
                    baseObject = Obj.GetComponent<GridStats>().SwitchTileToBase(hexBase, gridArray);
                }
                else
                {
                    Obj.GetComponent<GridStats>().basable = 0;
                }
            }
        //}  
    }
    

    void SetTileInfo(GameObject GO, int x, int z)
    {
        GO.transform.parent = transform;
        GO.name = x.ToString() + "," + z.ToString();
    }

    void SetDistance()
    {
        // Pour chaque objet on le tag a Visited = -1 sauf le start a 0
        InitialSetUp();

        int x = spawnObject.GetComponent<GridStats>().x; 
        int y = spawnObject.GetComponent<GridStats>().y;
        int[] testArray = new int[rows * columns];
        // Pour mon nombre d'éléments dans ma grille
        for (int step = 1; step < rows*columns; step++)
        {
            //Pour chaque objet
            foreach (GameObject Obj in gridArray)
            {
                //Si Obj existe && visited  == 
                if (Obj && Obj.GetComponent<GridStats>().visited == step - 1)
                    TestFourDirections(Obj.GetComponent<GridStats>().x, Obj.GetComponent<GridStats>().y, step);
            }
        }
    }

    void SetPath()
    {
        int step;
        int x = baseObject.GetComponent<GridStats>().x; //endX;
        int y = baseObject.GetComponent<GridStats>().y;//endY;
        List<GameObject> tempList = new List<GameObject>();
        path.Clear();

        // Si mon objet ciblé dans ma table d'objet Existe && si mon champ visited > 0 (if negative = Solution FOund)
        if (gridArray[x, y] && gridArray[x, y].GetComponent<GridStats>().visited > 0) 
        {
            path.Add(gridArray[x, y]);
            step = gridArray[x, y].GetComponent<GridStats>().visited - 1;
        }
        else
        {
            print("SetPath : Ne peut pas atteindre la position souhaitée");
            return;
        }
        for (int i = step ; step > -1; step--)
        {
            if (TestDirection(x, y, step, 1))
                tempList.Add(gridArray[x, y + 1]);
            if (TestDirection(x, y, step, 2))
                tempList.Add(gridArray[x+1, y]);
            if (TestDirection(x, y, step, 3))
                tempList.Add(gridArray[x, y - 1]);
            if (TestDirection(x, y, step, 4))
                tempList.Add(gridArray[x -1 , y]);
            GameObject tempObj = FindClosest(gridArray[x, y].transform, tempList);
            path.Add(tempObj);
            var TileRenderer = tempObj.GetComponent<GridStats>().GetComponent<Renderer>();
            TileRenderer.material.SetColor("_Color", Color.grey);
            x = tempObj.GetComponent<GridStats>().x;
            y = tempObj.GetComponent<GridStats>().y;
            tempList.Clear();
            //Debug.Log("Waiting end of path");
        }        
    }

    void TestFourDirections(int x, int y, int step)
    {
        
        if (TestDirection(x, y, -1, 1))// UP
            SetVisited(x, y + 1, step); // + UnityEngine.Random.Range(0, 10)
        if (TestDirection(x, y, -1, 2))// RIGHT
            SetVisited(x + 1, y, step);
        if (TestDirection(x, y, -1, 3))// DOWN
            SetVisited(x, y - 1, step);
        if (TestDirection(x, y, -1, 4))// LEFT
            SetVisited(x - 1, y, step);
    }

    void InitialSetUp()
    {
        // Pour chaque objet on le tag a Visited = -1 sauf le start a 0
        foreach (GameObject Obj in gridArray)
        {
            if (Obj)
            {
                    // On initialise tt les items Non visités
                    Obj.GetComponent<GridStats>().visited = -1;
                    // On initialise tt les items Non eligible a une Base
                    Obj.GetComponent<GridStats>().basable = 0;
                    // On initialise tt les items Non eligible a un Spawn
                    Obj.GetComponent<GridStats>().spawnable = 0;
            }
                // On initialise le Start comme visité et connu
                spawnObject.GetComponent<GridStats>().visited = 0;
        }
    }

    // Fonction qui test chacune des direction possibles (1=UP; 2=RIGHT; 3=DOWN; 4=LEFT)
    // Si Visited = -1 alors TestDirection = True
    bool TestDirection(int x, int y, int step, int direction)
    {
        switch (direction)
        {
            case 4:
                if (x-1 > -1 && gridArray[x-1, y] && gridArray[x-1, y].GetComponent<GridStats>().visited == step)
                    return true;
                else
                    return false;
            case 3:
                if (y-1 > -1 && gridArray[x, y-1] && gridArray[x, y-1].GetComponent<GridStats>().visited == step)
                    return true;
                else
                    return false;
            case 2:
                if (x+1 < columns && gridArray[x+1,y] && gridArray[x+1,y].GetComponent<GridStats>().visited == step)
                    return true;
                else
                    return false;
            case 1:
                if (y+1 < rows && gridArray[x,y+1] && gridArray[x,y+1].GetComponent<GridStats>().visited == step)
                    return true;
                else
                    return false;
        }
        return false;
    }

    void SetVisited(int x, int y, int step)
    {
        if (gridArray[x, y])
            gridArray[x, y].GetComponent<GridStats>().visited = step;
    }

    GameObject FindClosest(Transform targetLocation,List<GameObject> list)
    {
        float currentDistance = rows * columns; //Suppression du * Scale afin d'éviter que mon offset qui est <1 pose probléme 
        int indexNumber = 0;
        for (int i = 0; i < list.Count; i++)
        {
            if (Vector3.Distance(targetLocation.position, list[i].transform.position) < currentDistance)
            {
                currentDistance = Vector3.Distance(targetLocation.position, list[i].transform.position);
                indexNumber = i;
            }
        }
        return list[indexNumber];

    }
}
