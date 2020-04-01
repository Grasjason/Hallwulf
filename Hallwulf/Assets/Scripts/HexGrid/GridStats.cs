using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GridStats : MonoBehaviour
{
    public int visited = -1;
    public int x = 0;
    public int y = 0;
    public int basable;
    public int spawnable;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void ConvertToBase()
    {
        //Get the Renderer component from the new cube
        var TileRenderer = this.GetComponent<Renderer>();

        //Call SetColor using the shader property name "_Color" and setting the color to red
        TileRenderer.material.SetColor("_Color", Color.green);
    }

    public void ConvertToSpawn()
    {
        //Get the Renderer component from the new cube
        var TileRenderer = this.GetComponent<Renderer>();

        //Call SetColor using the shader property name "_Color" and setting the color to red
        TileRenderer.material.SetColor("_Color", Color.red);
    }    
}
