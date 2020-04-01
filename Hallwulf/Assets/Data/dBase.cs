using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Data", menuName = "Data/dBase")]
public class dBase : ScriptableObject
{
    public GameObject dBaseHexTile;
    public string dBaseName = "Base Camp";
    public Color dBaseColor = Color.cyan;
}
