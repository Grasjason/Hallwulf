using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class HexTileMapGenerator : MonoBehaviour
{

    public GameObject hexTilePrefab;
    public GameObject baseHexObj;
    public GameObject spawnHexObj;
    //public Dictionary<string, int[,]> TileBaseDic = new Dictionary<string, GameObject>();
    //public Dictionary<string, int[,]> TileSpawnDic = new Dictionary<string, GameObject>();

    [SerializeField] int mapWidth = 50;
    [SerializeField] int mapHeight = 50;
  
    // Permet d'assurer l'emboitage des Hexagones avec le bon décalage X,Z
    float tileXOffset = 1f;
    float tileZOffset = 0.865f;

    // Start is called before the first frame update
    void Start()
    {
        CreateHexTileMap();
        //baseHexObj = BaseCreation(TileBaseDic);
        //spawnHexObj = SpawnCreation(TileSpawnDic);
        
        // Génère le Chemin aléatoire entre Base et Spawn
        Path Chemin = new Path();

        int mapXStart = (mapWidth / 2);
        //int mapYStart = (mapHeight / 2);

        string[] baseHexStringA = SplitObjectName(baseHexObj);
        string[] spawnHexStringA = SplitObjectName(spawnHexObj);

        var mynewpathh = Chemin.GenerateRandomPath(Convert.ToInt32(spawnHexStringA[0]), Convert.ToInt32(spawnHexStringA[1]), Convert.ToInt32(baseHexStringA[0]), Convert.ToInt32(baseHexStringA[1]), 0.3,-mapWidth, mapWidth);
        //Debug.Log(spawnHexStringA[0].ToString()+","+spawnHexStringA[1].ToString());
        //Debug.Log(baseHexStringA[0].ToString()+","+baseHexStringA[1].ToString());

        DrawTheRoad(mynewpathh, Convert.ToInt32(spawnHexStringA[0]), Convert.ToInt32(spawnHexStringA[1]), Convert.ToInt32(baseHexStringA[0]), Convert.ToInt32(baseHexStringA[1]));

    }

    public void DrawTheRoad(Path RoadPath,int SpawnX, int SpawnY, int BaseX, int BaseY)
    {
        for (int i = 0; i < RoadPath.Count; i++)
        {
            foreach (Direction item in RoadPath)
            {
                
                    switch (item)
                    {
                        case Direction.Left:
                            SpawnX--;
                            if (!(SpawnX == BaseX && SpawnY == BaseY))
                            {
                                ConvertToRoad(SearchObjByPos(SpawnX, SpawnY));
                                Debug.Log("Left : X--" + " ("+SpawnX + "," + SpawnY + ")");
                            }
                            break;
                        case Direction.Right:
                            SpawnX++;
                            if (!(SpawnX == BaseX && SpawnY == BaseY))
                            {
                                ConvertToRoad(SearchObjByPos(SpawnX, SpawnY));
                                Debug.Log("Right : X++" + " (" + SpawnX + "," + SpawnY + ")");
                            }
                            break;
                        case Direction.Down:
                            SpawnY--;
                            if (!(SpawnX == BaseX && SpawnY == BaseY))
                            {
                                ConvertToRoad(SearchObjByPos(SpawnX, SpawnY));
                                Debug.Log("Down : Y--" + " (" + SpawnX + "," + SpawnY + ")");
                            }
                            break;
                    }
                
            }
        }
    }

    //Recherche un objet en fonction de sa position
    public GameObject SearchObjByPos(int x, int y)
    {
        return GameObject.Find(x+","+y);
    }

    //Met a jour un objet pour qu'il devienne une route.
    public void ConvertToRoad(GameObject Obj)
    {
        var TileRenderer = Obj.GetComponent<Renderer>();
        TileRenderer.material.SetColor("_Color", Color.yellow);
    }

    //Récupére la position sous forme de string[] d'un objet en paramètre
    public string[] SplitObjectName(GameObject Obj)
    {
        string[] splitArray;
        return splitArray = Obj.name.Split(char.Parse(",")); //Here we assing the splitted string to array by that char
        //name = splitArray[0]; //Here we assign the first part to the name
    }

    // Instantiate : HexTiles & Memorise les #ID des Tiles Base et Spawn éligibles
    public void CreateHexTileMap()
    {

        // les mapStart variables permettent de centrer la génération des tuiles autour du point 0,0
        int mapXStart = (mapWidth / 2);
        int mapYStart = (mapHeight / 2);        

        for (int x = -mapXStart; x <= mapXStart; x++)
        {
            for (int z = -mapYStart; z <= mapYStart; z++)
            {
                GameObject TempGo = Instantiate(hexTilePrefab);

                if (z % 2 == 0)
                {
                    TempGo.transform.position = new Vector3(x * tileXOffset, 0, z * tileZOffset);
                }
                else
                {
                    TempGo.transform.position = new Vector3(x * tileXOffset + tileXOffset / 2, 0, z * tileZOffset);
                }
                SetTileInfo(TempGo,x,z);

                // Correspond a une ligne pour une Base a 5% de la bordure Basse Arrondi au supérieur pour gérer les superficies Impair
                int baseLimit = (int) Math.Floor(mapYStart - (mapHeight * 0.05));
                int spawnLimit = (int) Math.Floor(mapXStart - (mapWidth * 0.05));

                if (z == -baseLimit)
                {
                    if (x <= spawnLimit && x >= -spawnLimit)
                    {
                        //TileBaseDic.Add(TempGo.name, TempGo);
                    }
                }
                // Correspond a une ligne pour un Spawn a 5% de la bordure Haute Arrondi au supérieur pour gérer les superficies Impair
                
                if (z == baseLimit)
                {
                    if (x <= spawnLimit && x >= -spawnLimit)
                    {
                        //TileSpawnDic.Add(TempGo.name, TempGo);
                    }
                }
            }
        }

        /*foreach (KeyValuePair<string, int> items in TileBaseDic)
        {
            //Debug.Log(items.Key.ToString() + " (" + items.Value.ToString()+")");
        }*/
    }

    public GameObject BaseCreation(Dictionary<string, GameObject> TileBaseDic)
    {
        // Get Desired Tile
        string randomKey = TileBaseDic.Keys.ToArray()[(int)UnityEngine.Random.Range(0, TileBaseDic.Keys.Count - 1)];
        GameObject randomObjectFromDictionary = TileBaseDic[randomKey];
        SetTileBase(randomObjectFromDictionary);

        return randomObjectFromDictionary;
    }

    void SetTileBase(GameObject Base)
    {
        //Get the Renderer component from the new cube
        var TileRenderer = Base.GetComponent<Renderer>();

        //Call SetColor using the shader property name "_Color" and setting the color to red
        TileRenderer.material.SetColor("_Color", Color.green);
    }

    /*public GameObject SpawnCreation(Dictionary<string, GameObject> TileSpawnDic)
    {
        // Get Desired Tile
        //string randomKey = TileSpawnDic.Keys.ToArray()[(int)UnityEngine.Random.Range(0, TileBaseDic.Keys.Count - 1)];
        //GameObject randomObjectFromDictionary = TileSpawnDic[randomKey];
        //SetTileSpawn(randomObjectFromDictionary);

        //return randomObjectFromDictionary;
    }*/

    void SetTileSpawn(GameObject Spawn)
    {
        //Get the Renderer component from the new cube
        var TileRenderer = Spawn.GetComponent<Renderer>();

        //Call SetColor using the shader property name "_Color" and setting the color to red
        TileRenderer.material.SetColor("_Color", Color.red);
    }

    void SetTileInfo(GameObject GO, int x, int z)
        {
            GO.transform.parent = transform;
            GO.name = x.ToString() + "," + z.ToString();
        }
}
