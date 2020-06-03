using System.Collections;
using System;
using System.Linq;
using System.Collections.Generic;
using UnityEngine;

// Row is Y
// Col is X

public class GridController : MonoBehaviour
{
    public int width, height;
    public GameObject[] tiles;
    private TileController[,] grid;
    private Scoring scoreboard;
    private MatchController mc;
    private bool motion = false;
    private bool userInput = false;

    void Start() {
        // Set camera to bottom-center the grid
        Camera.main.transform.position = new Vector3(width / 2, height / 2.0f, -10);
        scoreboard = GameObject.Find("UI").GetComponent<Scoring>();
        mc = gameObject.GetComponent<MatchController>();
        initializeGrid();
        fillGrid(grid);
        StartCoroutine(checkMatches());
    }

    void Update() {
        checkMotion();
    }

    // Loop through all tiles, if any are true, motion is true
    private void checkMotion() {
        int motionCounter = 0;
        foreach (TileController tile in grid) {
            // Null gate
            if (tile == null) { continue; }
            if (tile.moving) { motionCounter++; }
            if (userInput) { continue; }
        }

        motion = motionCounter > 0;
    }

    private void initializeGrid() {
        grid = new TileController[width, height];
    }

    // Loop through width/height and randomly generate tiles
    private void fillGrid(TileController[,] g) {
        for (int i = 0; i < width; i++) {
            for (int j = 0; j < height; j++) {
                mc.reset();
                Vector2 pos = new Vector2(i, j);
                // Store the script reference as opposed to the gameobject
                TileController tile = randTile(tiles, pos).GetComponent<TileController>();
                tile.setCoord(pos);
                grid.SetValue(tile, i, j);
            }
        }
    }

    // Generate and instantiate a random tile
    private GameObject randTile(GameObject[] t, Vector2 pos) {
        int rand = UnityEngine.Random.Range(0, t.Length);
        return Instantiate(
            t[rand],
            pos,
            Quaternion.identity,
            this.transform
        );
    }

    // Get method for other controllers to use
    public TileController getTile(Vector2 vec) {
        return grid.Get(vec);
    }

    // A standard move requires two (adjacent) tiles
    private void moveTile(TileController from, TileController to) {
        Vector2 firstPos = from.getVector();
        Vector2 secondPos = to.getVector();
        from.setCoord(secondPos);
        to.setCoord(firstPos);
        grid.Swap(firstPos, secondPos);
    }

    // Checks for matches when a user commands a move
    public IEnumerator swapTiles(TileController[] s) {
        // Flag for user input TODO:: Check if still necessary
        userInput = true;
        scoreboard.resetMultiplier();
        // Reset match list
        mc.reset();
        moveTile(s[0], s[1]);
        foreach (TileController tile in s) {
            // Check both tiles
            mc.check(grid, tile);
        }

        if (mc.matches.Count > 0) {
            // Wait for motion to be false before continuing
            yield return new WaitUntil(() => !motion);
            StartCoroutine(removeAndReplaceMatches(mc.matches));
        } else {
            yield return new WaitUntil(() => !motion);
            // Move the tiles back and animate
            moveTile(s[0], s[1]);
            s[0].invalid();
            s[1].invalid();
        }
        userInput = false;
    }

    // Loop through and push grid changes to children
    private void updateTiles() {
        for (int i = 0; i < width; i++) {
            for (int j = 0; j < height; j++) {
                Vector2 pos = new Vector2(i, j);
                if (grid.Get(pos) == null) { continue; } 
                grid.Get(pos).setCoord(pos);
            }
        }
    }

    // Generate new tiles where there are null values
    private void replenishTiles() {
        for (int i = 0; i < width; i++) {
            int nullCount = 1;
            for (int j = 0; j < height; j++) {
                Vector2 pos = new Vector2(i, j);
                if (grid.Get(pos) == null) {
                    Vector2 createPos = new Vector2(i, height + nullCount);
                    TileController tile = randTile(tiles, createPos).GetComponent<TileController>();
                    grid.Set(tile, pos);
                    // Use a counter to offset the height of different rows,
                    // so the tiles drop at a seemingly equal pace
                    nullCount++;
                }
            }
        }
    }

    // Check the entire grid for matches after everything has settled
    public IEnumerator checkMatches() {
        yield return new WaitUntil(() => !motion);
        mc.reset();
        foreach (TileController tile in grid) {
            if (tile == null) { continue; }
            mc.check(grid, tile);
        }
        if (mc.matches.Count > 0) { 
            // Start the process over if there are matches
            StartCoroutine(removeAndReplaceMatches(mc.matches)); 
        }
    }

    // Move all not-null tiles to bottom and recheck
    private void shiftGrid() {
        grid.ShiftValuesDown();
        replenishTiles();
        updateTiles();
        StartCoroutine(checkMatches());
    }

    // Destroy matched tiles
    private IEnumerator removeAndReplaceMatches(List<List<TileController>> matches) {
        yield return new WaitUntil(() => !motion);

        foreach (List<TileController> match in matches) {
            foreach (TileController tile in match) {
                scoreboard.increment();
                grid.Set(null, tile.getVector());
                Destroy(tile.gameObject);
            }
        }
        shiftGrid();
    }
}
