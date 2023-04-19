using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MatchesManager : MonoBehaviour
{
    private GridManager grid;
    public List<GameObject> currentMatches = new List<GameObject>();


    // Start is called before the first frame update
    void Start()
    {
        grid = FindObjectOfType<GridManager>();
    }

    public void findAllMatches()
    {
        StartCoroutine(findAllMatchesCo());
    }

    private IEnumerator findAllMatchesCo()
    {
        yield return new WaitForSeconds(.2f);
        for (int x = 0; x < grid.width; x++)
        {
            for (int y = 0; y < grid.height; y++)
            {
                GameObject currentCandy = grid.allCandies[x, y];
                if (currentCandy != null)
                {
                    if (x > 0 && x < grid.width - 1)
                    {
                        GameObject lCandy = grid.allCandies[x - 1, y];
                        GameObject rCandy = grid.allCandies[x + 1, y];
                        if (lCandy != null && rCandy != null)
                        {
                            if (lCandy.tag == currentCandy.tag && rCandy.tag == currentCandy.tag)
                            {
                                if (!currentMatches.Contains(lCandy))
                                {
                                    currentMatches.Add(lCandy);
                                }
                                lCandy.GetComponent<Candy>().isMatched = true;
                                if (!currentMatches.Contains(rCandy))
                                {
                                    currentMatches.Add(rCandy);
                                }
                                rCandy.GetComponent<Candy>().isMatched = true;
                                if (!currentMatches.Contains(currentCandy))
                                {
                                    currentMatches.Add(currentCandy);
                                }
                                currentCandy.GetComponent<Candy>().isMatched = true;

                            }
                        }
                    }
                    if (y > 0 && y < grid.height - 1)
                    {
                        GameObject downCandy = grid.allCandies[x, y - 1];
                        GameObject upCandy = grid.allCandies[x, y + 1];
                        if (downCandy != null && upCandy != null)
                        {
                            if (downCandy.tag == currentCandy.tag && upCandy.tag == currentCandy.tag)
                            {
                                if (!currentMatches.Contains(downCandy))
                                {
                                    currentMatches.Add(downCandy);
                                }
                                downCandy.GetComponent<Candy>().isMatched = true;
                                if (!currentMatches.Contains(upCandy))
                                {
                                    currentMatches.Add(upCandy);
                                }
                                upCandy.GetComponent<Candy>().isMatched = true;
                                if (!currentMatches.Contains(currentCandy))
                                {
                                    currentMatches.Add(currentCandy);
                                }
                                currentCandy.GetComponent<Candy>().isMatched = true;

                            }
                        }
                    }
                }
            }
        }
    }
}
