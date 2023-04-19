using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Networking.Match;
using UnityEngine.SocialPlatforms.Impl;

public class MatchesManager : MonoBehaviour
{
    private GridManager _grid;
    public List<GameObject> currentMatches = new List<GameObject>();
    int addscore = GameManager.instance.scoreForMatch;//Se si vuole cambiare per i livelli futuri

    // Start is called before the first frame update
    void Start()
    {
        _grid = FindObjectOfType<GridManager>();        
    }
    /// <summary>
    /// When called start a Coroutine
    /// </summary>
    public void findAllMatches()
    {
        StartCoroutine(findAllMatchesCo());
    }
    /// <summary>
    /// This coroutine loops through each position on the grid and checks for matches in each direction (left, right, up, and down).
    ///If a match is found(If there are neighboring candies, the method checks if all three candies have the same tag.), the script adds the matching candies to the currentMatches list and sets their isMatched variable to true. 
    ///It also increases the score using the UIManager and clears the currentMatches list to ensure we only count each match once.
    /// </summary>
    /// <returns></returns>
    private IEnumerator findAllMatchesCo()
    {
        yield return new WaitForSeconds(.2f);
        for (int x = 0; x < _grid.width; x++)
        {
            for (int y = 0; y < _grid.height; y++)
            {
                GameObject currentCandy = _grid.allCandies[x, y];
                if (currentCandy != null)
                {
                    if (x > 0 && x < _grid.width - 1)
                    {
                        GameObject leftCandy = _grid.allCandies[x - 1, y];
                        GameObject rightCandy = _grid.allCandies[x + 1, y];
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
                                UIManager.instance.IncreaseScore(addscore);
                                currentMatches.Clear();

                            }
                        }
                    }
                    if (y > 0 && y < _grid.height - 1)
                    {
                        GameObject downCandy = _grid.allCandies[x, y - 1];
                        GameObject upCandy = _grid.allCandies[x, y + 1];
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
                                UIManager.instance.IncreaseScore(addscore);
                                currentMatches.Clear();
                            }
                        }
                    }
                }
            }
        }
    }
}
