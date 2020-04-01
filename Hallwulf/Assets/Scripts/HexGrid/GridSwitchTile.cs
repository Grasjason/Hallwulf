using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GridSwitchTile : MonoBehaviour
{
    public dFloor hexFloor;
    public dBase hexBase;

    public void SwitchTileToBase(GameObject currentTile)
    {
        GameObject Instantiated = Instantiate(hexBase.dBaseHexTile);

        Instantiated.transform.parent = currentTile.transform.parent;
        Instantiated.GetComponent<GridStats>().x = currentTile.GetComponent<GridStats>().x;
        Instantiated.GetComponent<GridStats>().y = currentTile.GetComponent<GridStats>().y;
        Instantiated.GetComponent<GridStats>().visited = currentTile.GetComponent<GridStats>().visited;
        Instantiated.GetComponent<GridStats>().basable = currentTile.GetComponent<GridStats>().basable;
        Instantiated.GetComponent<GridStats>().spawnable = currentTile.GetComponent<GridStats>().spawnable;
        Instantiated.transform.position = currentTile.transform.position;
        Instantiated.name = currentTile.name + "(Base)";
        Destroy(currentTile);
        Instantiated.transform.SetParent(gameObject.transform);
    }

}
