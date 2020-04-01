using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Data", menuName = "Data/dRoad")]
public class dRoad : ScriptableObject
{
    public GameObject dRoadHexTile;
    public string dRoadName = "Road Tile";
    public Color dRoadColor = Color.grey;
}
