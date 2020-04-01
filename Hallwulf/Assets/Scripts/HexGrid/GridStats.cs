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

    public void SwitchTileToBase(dBase hexBase)
    {
        if (hexBase)
        {
            GameObject hexBaseGo = Instantiate(hexBase.dBaseHexTile);

            hexBaseGo.transform.parent = this.GetComponent<GridStats>().transform.parent;
            hexBaseGo.GetComponent<GridStats>().x = this.GetComponent<GridStats>().GetComponent<GridStats>().x;
            hexBaseGo.GetComponent<GridStats>().y = this.GetComponent<GridStats>().GetComponent<GridStats>().y;
            hexBaseGo.GetComponent<GridStats>().visited = this.GetComponent<GridStats>().GetComponent<GridStats>().visited;
            hexBaseGo.GetComponent<GridStats>().basable = this.GetComponent<GridStats>().GetComponent<GridStats>().basable;
            hexBaseGo.GetComponent<GridStats>().spawnable = this.GetComponent<GridStats>().GetComponent<GridStats>().spawnable;
            hexBaseGo.transform.position = this.GetComponent<GridStats>().transform.position;
            hexBaseGo.name = this.GetComponent<GridStats>().name + "(Base)";
            Destroy(this.gameObject);
        }
        else
        {
            Debug.Log("dBase n'est pas affecté a dFloor");
        }

    }
    public void SwitchTileToSpawn(dSpawn hexSpawn)
    {
        if (hexSpawn)
        {
            GameObject hexBaseGo = Instantiate(hexSpawn.dSpawnHexTile);

            hexBaseGo.transform.parent = this.GetComponent<GridStats>().transform.parent;
            hexBaseGo.GetComponent<GridStats>().x = this.GetComponent<GridStats>().GetComponent<GridStats>().x;
            hexBaseGo.GetComponent<GridStats>().y = this.GetComponent<GridStats>().GetComponent<GridStats>().y;
            hexBaseGo.GetComponent<GridStats>().visited = this.GetComponent<GridStats>().GetComponent<GridStats>().visited;
            hexBaseGo.GetComponent<GridStats>().basable = this.GetComponent<GridStats>().GetComponent<GridStats>().basable;
            hexBaseGo.GetComponent<GridStats>().spawnable = this.GetComponent<GridStats>().GetComponent<GridStats>().spawnable;
            hexBaseGo.transform.position = this.GetComponent<GridStats>().transform.position;
            hexBaseGo.name = this.GetComponent<GridStats>().name + "(Spawn)";
            Destroy(this.gameObject);
        }
        else
        {
            Debug.Log("dSpawn n'est pas affecté a dFloor");
        }

    }
}
