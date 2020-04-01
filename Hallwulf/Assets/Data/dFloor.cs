using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Data", menuName = "Data/dFloor", order = 1)]
public class dFloor : ScriptableObject
{
    public GameObject dFloorHexTile;
    public string dFloorName = "Floor Tile";
    public Color dFloorColor = Color.blue;
}

