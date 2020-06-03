using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InputController : MonoBehaviour
{
    public TileController[] selected;
    private GridController gc;

    void Start() {
        gc = gameObject.GetComponent<GridController>();
        selected = new TileController[2];
    }

    public void click(Vector2 pos) {
        TileController tile = gc.getTile(pos);
        if (selected[0] == null) {
            // OK this is the first selection
            selected.SetValue(tile, 0);
        } else {
            // Must be the second selection
            selected.SetValue(tile, 1);
            checkIfNeighbor(selected);
            if (selected[1] == null) { return; }
            // Tell grid controller we have a pair
            notifyGrid(selected);       
        }
    }

    // If it's not a neighbor, reset the array
    private void checkIfNeighbor(TileController[] s) {
        Vector2 v1 = s[0].getVector();
        Vector2 v2 = s[1].getVector();
        Vector2 diff = (v2 - v1).Abs();
        // If it's the same tile, reset/deselect
        if(v1 == v2) { reset(); }
        // If it isnt dist == 1, reset
        if(diff.x > 1 || diff.y > 1) { reset(); }
    } 

    private void reset() {
        foreach(TileController tile in selected) {
            if (tile == null) { return; }
            tile.unSelect();
        }
        selected = new TileController[2];
    }

    // Pass it up to the grid controller and clear
    private void notifyGrid(TileController[] s) {
        StartCoroutine(gc.swapTiles(s));
        reset();
    }
}
