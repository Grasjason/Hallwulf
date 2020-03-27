using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CustomEditor (typeof(HexTileMapGenerator))]
public class hexTileGeneratorEditor : Editor
{
    public override void OnInspectorGUI()
    {
        HexTileMapGenerator tileGen = (HexTileMapGenerator) target;

        DrawDefaultInspector();

        if (GUILayout.Button("Generate"))
        {
            tileGen.CreateHexTileMap();
        }
    }
}
