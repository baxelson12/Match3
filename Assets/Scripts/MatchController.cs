using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

// Row is Y
// Col is X

public class MatchController : MonoBehaviour
{
    public int matchCount;
    [Space(5)]
    public TileController selected;
    [Space(10)]
    public List<List<TileController>> matches;
    private TileController[] row, col;
    private GridController gc;
    private TileController[,] grid;
    private Vector2[] directions;

    void Start() {
        gc = gameObject.GetComponent<GridController>();
        matches = new List<List<TileController>>();
    }

    // Entry point :: Set needed variables
    public void check(TileController[,] g, TileController s) {
        grid = g;
        selected = s;
        row = g.GetRow(s.getVector().y.ToInt());
        col = g.GetCol(s.getVector().x.ToInt());
        matchRow(row, selected);
        matchCol(col, selected);
        selected = null;
    }

    // Clear out the variables
    public void reset() {
        grid = null;
        matches = new List<List<TileController>>();
        selected = null;
    }

    // Look for matching tiles to the left and right
    private void matchRow(TileController[] r, TileController s) {
        List<TileController> rowMatches = new List<TileController>();
        // The tile in question's position
        Vector2 origin = s.getVector();
        // Iterate from origin to left edge of grid
        for (int i = (int)origin.x; i >= 0; i--) {
            TileController nextTile = (TileController)r.GetValue(i);
            if (nextTile.tag == s.tag) {
                // If the tags match, add to list
                // otherwise, we're done here
                rowMatches.Add(nextTile);
            } else { break; }
        }
        // Now check origin to right edge of grid
        for (int i = (int)origin.x; i < gc.width; i++) {
            TileController nextTile = (TileController)r.GetValue(i);
            if (nextTile.tag == s.tag) {
                rowMatches.Add(nextTile);
            } else { break; }
        }

        // Remove any duplicate values
        rowMatches = rowMatches.Distinct().ToList();
        // if it's larger than 3 (specified count) then add it to the master list
        if (rowMatches.Count >= matchCount) {
            matches.Add(rowMatches);
        }
    }

    // Same as row except up/down
    private void matchCol(TileController[] c, TileController s) {
        List<TileController> colMatches = new List<TileController>();
        Vector2 origin = s.getVector();

        for (int i = (int)origin.y; i >= 0; i--) {
            TileController nextTile = (TileController)c.GetValue(i);
            if (nextTile.tag == s.tag) {
                colMatches.Add(nextTile);
            } else { break; }
        }
        for (int i = (int)origin.y; i < gc.height; i++) {
            TileController nextTile = (TileController)c.GetValue(i);
            if (nextTile.tag == s.tag) {
                colMatches.Add(nextTile);
            } else { break; }
        }

        colMatches =colMatches.Distinct().ToList();
       if (colMatches.Count >= matchCount) {
            matches.Add(colMatches);
       }
    }
}
