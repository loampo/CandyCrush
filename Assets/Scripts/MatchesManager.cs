using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.SocialPlatforms.Impl;

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
                        GameObject leftCandy = grid.allCandies[x - 1, y];
                        GameObject rightCandy = grid.allCandies[x + 1, y];
                        if (leftCandy != null && rightCandy != null)
                        {
                            if (leftCandy.tag == currentCandy.tag && rightCandy.tag == currentCandy.tag)
                            {
                                if (!currentMatches.Contains(leftCandy))
                                {
                                    currentMatches.Add(leftCandy);
                                }
                                leftCandy.GetComponent<Candy>().isMatched = true;
                                if (!currentMatches.Contains(rightCandy))
                                {
                                    currentMatches.Add(rightCandy);
                                }
                                rightCandy.GetComponent<Candy>().isMatched = true;
                                if (!currentMatches.Contains(currentCandy))
                                {
                                    currentMatches.Add(currentCandy);
                                }
                                currentCandy.GetComponent<Candy>().isMatched = true;

                                // Increase the score and reset the currentMatches list
                                UIManager.instance.IncreaseScore(10);
                                currentMatches.Clear();

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
                                // Increase the score and reset the currentMatches list
                                UIManager.instance.IncreaseScore(10);
                                currentMatches.Clear();
                            }
                        }
                    }
                }
            }
        }
    }
}
