using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class HexTileMapGenerator : MonoBehaviour
{

    public GameObject hexTilePrefab;
    public Dictionary<string, GameObject> TileBaseDic = new Dictionary<string, GameObject>();
    public Dictionary<string, GameObject> TileSpawnDic = new Dictionary<string, GameObject>();

    [SerializeField] int mapWidth = 50;
    [SerializeField] int mapHeight = 50;
  
    // Permet d'assurer l'emboitage des Hexagones avec le bon décalage X,Z
    float tileXOffset = 1f;
    float tileZOffset = 0.865f;

    // Start is called before the first frame update
    void Start()
    {
        CreateHexTileMap();
        BaseCreation(TileBaseDic);
        SpawnCreation(TileSpawnDic);

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
                        TileBaseDic.Add(TempGo.name, TempGo);
                    }
                }
                // Correspond a une ligne pour un Spawn a 5% de la bordure Haute Arrondi au supérieur pour gérer les superficies Impair
                
                if (z == baseLimit)
                {
                    if (x <= spawnLimit && x >= -spawnLimit)
                    {
                        TileSpawnDic.Add(TempGo.name, TempGo);
                    }
                }
            }
        }

        /*foreach (KeyValuePair<string, int> items in TileBaseDic)
        {
            //Debug.Log(items.Key.ToString() + " (" + items.Value.ToString()+")");
        }*/
    }

    public void BaseCreation(Dictionary<string, GameObject> TileBaseDic)
    {
        // Get Desired Tile
        string randomKey = TileBaseDic.Keys.ToArray()[(int)UnityEngine.Random.Range(0, TileBaseDic.Keys.Count - 1)];
        GameObject randomObjectFromDictionary = TileBaseDic[randomKey];
        SetTileBase(randomObjectFromDictionary);         
    }

    void SetTileBase(GameObject Base)
    {
        //Get the Renderer component from the new cube
        var TileRenderer = Base.GetComponent<Renderer>();

        //Call SetColor using the shader property name "_Color" and setting the color to red
        TileRenderer.material.SetColor("_Color", Color.green);
    }

    public void SpawnCreation(Dictionary<string, GameObject> TileSpawnDic)
    {
        // Get Desired Tile
        string randomKey = TileSpawnDic.Keys.ToArray()[(int)UnityEngine.Random.Range(0, TileBaseDic.Keys.Count - 1)];
        GameObject randomObjectFromDictionary = TileSpawnDic[randomKey];
        SetTileSpawn(randomObjectFromDictionary);
    }

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
