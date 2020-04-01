using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Data", menuName = "Data/dSpawn")]
public class dSpawn : ScriptableObject
{
    public GameObject dSpawnHexTile;
    public string dSpawnName = "Base Camp";
    public Color dSpawnColor = Color.red;
}
