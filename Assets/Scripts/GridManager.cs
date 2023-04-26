using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;
using UnityEngine.EventSystems;
using UnityEngine.XR;
using UnityEngine.SocialPlatforms.Impl;

public class GridManager : MonoBehaviour
{
    public enum gameState
    {
        wait,
        move
    }


    public gameState currentState = gameState.move;
    public int width;
    public int height;
    public int offset;
    public GameObject tilePrefab;
    private Tile[,] _allTiles;
    public GameObject[] candies;
    public GameObject[,] allCandies;
    private MatchesManager _findMatches;

    // Start is called before the first frame update
    void Start()
    {
        _findMatches = FindObjectOfType<MatchesManager>();
        _allTiles = new Tile[width, height];
        allCandies = new GameObject[width, height];
        Setup();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape) && Time.timeScale == 1)
        {
            GameManager.instance.Pause();         
        }
    }


    //Creating the board
    private void Setup()
    {
        for (int x = 0; x < width; x++)
        {
            for (int y = 0; y < height; y++)
            {
                Vector2 tempPos = new Vector2(x, y + offset);
                GameObject backgroundTile = Instantiate(tilePrefab, tempPos, Quaternion.identity) as GameObject;
                backgroundTile.transform.parent = this.transform;
                backgroundTile.name = "( " + x + ',' + y + " )";
                int dotToUse = Random.Range(0, candies.Length);

                int maxIterations = 0;
                while (matchesAt(x, y, candies[dotToUse]) && maxIterations < 100)
                {
                    dotToUse = Random.Range(0, candies.Length);
                    maxIterations++;
                }
                maxIterations = 0;

                GameObject Candy = Instantiate(candies[dotToUse], tempPos, Quaternion.identity);
                Candy.GetComponent<CandyTile>().row = y;
                Candy.GetComponent<CandyTile>().col = x;
                Candy.transform.parent = this.transform;
                Candy.name = "Candy: ( " + x + ',' + y + " )";
                allCandies[x, y] = Candy;
            }
        }
    }

    private bool matchesAt(int col, int row, GameObject obj)
    {
        if (col > 1 && row > 1)
        {
            if (allCandies[col - 1, row].tag == obj.tag && allCandies[col - 2, row].tag == obj.tag)
            {
                return true;
            }
            if (allCandies[col, row - 1].tag == obj.tag && allCandies[col, row - 2].tag == obj.tag)
            {
                return true;
            }

        }
        else if (col <= 1 || row <= 1)
        {
            if (row > 1)
            {
                if (allCandies[col, row - 1].tag == obj.tag && allCandies[col, row - 2].tag == obj.tag)
                {
                    return true;
                }
            }
            if (col > 1)
            {
                if (allCandies[col - 1, row].tag == obj.tag && allCandies[col - 2, row].tag == obj.tag)
                {
                    return true;
                }
            }
        }
        return false;
    }

    private void destroyMatchesAt(int col, int row)
    {
        if (allCandies[col, row].GetComponent<CandyTile>().isMatched)
        {
            _findMatches.currentMatches.Remove(allCandies[col, row]);
            Destroy(allCandies[col, row]);
            allCandies[col, row] = null;
        }
    }

    public void destroyMatches()
    {
        for (int x = 0; x < width; x++)
        {
            for (int y = 0; y < height; y++)
            {
                if (allCandies[x, y] != null)
                {
                    destroyMatchesAt(x, y)
;
                }
            }
        }
        StartCoroutine(decreaceRowCo());
    }

    private IEnumerator decreaceRowCo()
    {
        int nullCount = 0;
        for (int x = 0; x < width; x++)
        {
            for (int y = 0; y < height; y++)
            {
                if (allCandies[x, y] == null)
                {
                    nullCount++;
                }
                else if (nullCount > 0)
                {
                    allCandies[x, y].GetComponent<CandyTile>().row -= nullCount;
                    allCandies[x, y] = null;
                }
            }
            nullCount = 0;
        }

        yield return new WaitForSeconds(.4f);
        StartCoroutine(fillBoardCo());
    }

    private void refillBoard()
    {
        for (int x = 0; x < width; x++)
        {
            for (int y = 0; y < height; y++)
            {
                if (allCandies[x, y] == null)
                {
                    Vector2 tempPos = new Vector2(x, y + offset);
                    int dotToUse = Random.Range(0, candies.Length);
                    GameObject Candy = Instantiate(candies[dotToUse], tempPos, Quaternion.identity);
                    allCandies[x, y] = Candy;
                    Candy.GetComponent<CandyTile>().row = y;
                    Candy.GetComponent<CandyTile>().col = x;
                }
            }
        }
    }

    private bool matchesOnBoard()
    {
        for (int x = 0; x < width; x++)
        {
            for (int y = 0; y < height; y++)
            {
                if (allCandies[x, y] != null)
                {
                    if (allCandies[x, y].GetComponent<CandyTile>().isMatched)
                    {
                        return true;
                    }
                }
            }
        }
        return false;
    }

    private IEnumerator fillBoardCo()
    {
        refillBoard();

        yield return new WaitForSeconds(.3f);

        while (matchesOnBoard())
        {
            yield return new WaitForSeconds(.3f);
            destroyMatches();
        }

        yield return new WaitForSeconds(2f);
        currentState = gameState.move;
    }
}
