﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GridStats : MonoBehaviour
{

    public int visited = -1;
    public int x = 0;
    public int y = 0;
    public int basable;
    
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

    /*// the private variable for the class
    private int x;

    // public getter and setter for other classes to use
    public int x
    {
        get
        {
            return this.x;
        }
        set
        {
            // include any checks you want to take place in here before setting the value
            x = value;
        }
    }

    private int y;
    public int y
    {
        get
        {
            return this.y;
        }
        set
        {
            // include any checks you want to take place in here before setting the value
            y = value;
        }
    }

    private int visited;
    public int visited
    {
        get
        {
            return this.visited;
        }
        set
        {
            // include any checks you want to take place in here before setting the value
            visited = value;
        }
    }*/
}
