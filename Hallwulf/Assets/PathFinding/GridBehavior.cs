using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GridBehavior : MonoBehaviour
{
    public bool findDistance = false;
    public int rows = 10;
    public int columns = 10;
    public int scale = 1;

    public GameObject hexPrefab;
    public Vector3 leftBottomLocation = new Vector3(0, 0, 0);

    public GameObject[,] gridArray;
    public int startX = 0;
    public int startY = 0;
    public int endX = 2;
    public int endY = 2;

    public List<GameObject> path = new List<GameObject>();

    // Start is called before the first frame update
    void Awake()
    {
        gridArray = new GameObject[columns, rows];
        if (hexPrefab)
            GenerateGrid();
        else print("Assigner une hexPrefab qui servira de Grille au sol.");
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
                GameObject Obj = (GameObject)Instantiate(hexPrefab, new Vector3(leftBottomLocation.x + scale * i,leftBottomLocation.y, leftBottomLocation.z + scale * j),Quaternion.identity);
                Obj.transform.SetParent(gameObject.transform);

                Obj.GetComponent<GridStats>().x = i;
                Obj.GetComponent<GridStats>().y = j;

                SetTileInfo(Obj, i, j);

                gridArray[i, j] = Obj;
            }
        }
    }

    void SetTileInfo(GameObject GO, int x, int z)
    {
        GO.transform.parent = transform;
        GO.name = x.ToString() + "," + z.ToString();
    }

    void SetDistance()
    {
        InitialSetUp();
        int x = startX;
        int y = startY;
        int[] testArray = new int[rows * columns];
        for (int step = 0; step < rows*columns; step++)
        {
            foreach (GameObject Obj in gridArray)
            {
                if (Obj && Obj.GetComponent<GridStats>().visited == step - 1)
                    TestFourDirections(Obj.GetComponent<GridStats>().x, Obj.GetComponent<GridStats>().y, step);
            }
        }
    }

    void SetPath()
    {
        int step;
        int x = endX;
        int y = endY;
        List<GameObject> tempList = new List<GameObject>();
        path.Clear();

        // Si mon objet ciblé dans ma table d'objet Existe && si mon champ visited > 0 (if negative = Solution FOund)
        if (gridArray[endX,endY] && gridArray[endX, endY].GetComponent<GridStats>().visited > 0) //
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
            GameObject tempObj = FindClosest(gridArray[endX, endY].transform, tempList);
            path.Add(tempObj);
            x = tempObj.GetComponent<GridStats>().x;
            y = tempObj.GetComponent<GridStats>().y;
            tempList.Clear();
            //Debug.Log("Waiting end of path");
        }       
    }

    void TestFourDirections(int x, int y, int step)
    {
        if (TestDirection(x, y, -1, 1))// UP
            SetVisited(x, y + 1, step);
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
            Obj.GetComponent<GridStats>().visited = -1;
            gridArray[startX, startY].GetComponent<GridStats>().visited = 0;
        }
    }

    // Fonction qui test chacune des direction possibles (1=UP; 2=RIGHT; 3=DOWN; 4=LEFT)
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
        float currentDistance = scale * rows * columns;
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
