using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.Tilemaps;

public class Tile : MonoBehaviour, IPointerClickHandler
{
    public TileData data;
    

    public void Initialize(GridManager gridM, int rowInit, int columnInit)
    {
        data = new TileData(gridM, rowInit, columnInit);
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        Debug.Log(gameObject.name);
    }
}
