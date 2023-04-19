using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.Rendering;

public class Candy : MonoBehaviour
{
    public bool isMatched = false;
    public int col;
    public int row;
    public int targetX;
    public int targetY;
    public int prevCol;
    public int prevRow;

    public GameObject shadow;

    private MatchesManager _match;
    private GridManager _grid;

    private GameObject _otherCandies;

    private Vector2 _firstTouchPos;
    private Vector2 _finalTouchPos;
    private Vector2 _tempPos;

    public float swipeResist = 1f;
    public float swipeAngle = 0;

    void Start()
    {
        _grid = FindObjectOfType<GridManager>();
        _match = FindObjectOfType<MatchesManager>();
    }
    /// <summary>
    /// updates the position of the candy on the grid, and checks for any new matches(MatchesManager.findAllMatches() method) that the candy might be a part of.
    /// It does this by moving the candy towards its target position using Vector2.Lerp(),
    /// and updating the allCandies array in the GridManager
    /// </summary>
    void Update()
    {
        targetX = col;
        targetY = row;
        if (Mathf.Abs(targetX - transform.position.x) > .1)
        {
            //Move towards target
            _tempPos = new Vector2(targetX, transform.position.y);
            transform.position = Vector2.Lerp(transform.position, _tempPos, .6f);
            // Update the candy's position in the grid
            if (_grid.allCandies[col, row] != this.gameObject)
            {
                _grid.allCandies[col, row] = this.gameObject;
            }
            // Find all matches on the grid
            _match.findAllMatches();

        }
        else
        {
            //Directly set the position
            _tempPos = new Vector2(targetX, transform.position.y);
            transform.position = _tempPos;
            // Update the candy's position in the grid
            _grid.allCandies[col, row] = this.gameObject;
        }
        if (Mathf.Abs(targetY - transform.position.y) > .1)
        {
            //Move towards target
            _tempPos = new Vector2(transform.position.x, targetY);
            transform.position = Vector2.Lerp(transform.position, _tempPos, .4f);
            // Update the candy's position in the grid
            if (_grid.allCandies[col, row] != this.gameObject)
            {
                _grid.allCandies[col, row] = this.gameObject;
            }
            // Find all matches on the grid
            _match.findAllMatches();
        }
        else
        {
            //Directly set the position
            _tempPos = new Vector2(transform.position.x, targetY);
            transform.position = _tempPos;
        }
    }
    /// <summary>
    ///  coroutine checks if the candy has moved to a valid position, and either 
    ///  moves the candy back to its previous position or destroys matches if a match is found.
    /// </summary>
    /// <returns></returns>
    public IEnumerator checkMoveCo()
    {
        yield return new WaitForSeconds(.5f);
        if (_otherCandies != null)
        {
            if (!isMatched && !_otherCandies.GetComponent<Candy>().isMatched)
            {
                _otherCandies.GetComponent<Candy>().row = row;
                _otherCandies.GetComponent<Candy>().col = col;
                row = prevRow;
                col = prevCol;
                yield return new WaitForSeconds(.5f);
                _grid.currentState = GridManager.gameState.move;
            }
            else
            {
                _grid.destroyMatches();

            }
            _otherCandies = null;
        }

    }

    private void OnMouseDown()
    {
        if (_grid.currentState == GridManager.gameState.move)
        {
            _firstTouchPos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        }
    }
    //On MouseDown and Up handle player input
    private void OnMouseUp()
    {
        if (_grid.currentState == GridManager.gameState.move)
        {
            _finalTouchPos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            calculateAngle();
        }
        else
        {
            _grid.currentState = GridManager.gameState.move;
        }

    }
    /// <summary>
    /// calculates the angle of the swipe gesture made by the player
    /// </summary>
    private void calculateAngle()
    {
        if (Mathf.Abs(_finalTouchPos.y - _firstTouchPos.y) > swipeResist || Mathf.Abs(_finalTouchPos.x - _firstTouchPos.x) > swipeResist)
        {
            swipeAngle = Mathf.Atan2(_finalTouchPos.y - _firstTouchPos.y, _finalTouchPos.x - _firstTouchPos.x) * 180 / Mathf.PI;
            movePieces();
            _grid.currentState = GridManager.gameState.wait;
        }

    }
    /// <summary>
    /// moves the candy object in response to the swipe direction.
    /// </summary>
    private void movePieces()
    {
        if (swipeAngle > -45 && swipeAngle <= 45 && col < _grid.width - 1)
        {
            //Right Swipe
            _otherCandies = _grid.allCandies[col + 1, row];
            prevRow = row;
            prevCol = col;
            _otherCandies.GetComponent<Candy>().col -= 1;
            col += 1;

        }
        else if (swipeAngle > 45 && swipeAngle <= 135 && row < _grid.height - 1)
        {
            //Up Swipe
            _otherCandies = _grid.allCandies[col, row + 1];
            prevRow = row;
            prevCol = col;
            _otherCandies.GetComponent<Candy>().row -= 1;
            row += 1;

        }
        else if ((swipeAngle > 135 || swipeAngle <= -135) && col > 0)
        {
            //Left Swipe
            _otherCandies = _grid.allCandies[col - 1, row];
            prevRow = row;
            prevCol = col;
            _otherCandies.GetComponent<Candy>().col += 1;
            col -= 1;
        }
        else if (swipeAngle < -45 && swipeAngle >= -135 && row > 0)
        {
            //Down Swipe
            _otherCandies = _grid.allCandies[col, row - 1];
            prevRow = row;
            prevCol = col;
            _otherCandies.GetComponent<Candy>().row += 1;
            row -= 1;
        }

        StartCoroutine(checkMoveCo());
    }

    private void findMatches()
    {
        if (col > 0 && col < _grid.width - 1)
        {
            GameObject leftCandy1 = _grid.allCandies[col - 1, row];
            GameObject rightCandy1 = _grid.allCandies[col + 1, row];
            if (leftCandy1 != null && rightCandy1 != null)
            {
                if (leftCandy1.tag == this.gameObject.tag && rightCandy1.tag == this.gameObject.tag)
                {
                    leftCandy1.GetComponent<Candy>().isMatched = true;
                    rightCandy1.GetComponent<Candy>().isMatched = true;
                    isMatched = true;
                }
            }
        }
        if (row > 0 && row < _grid.height - 1)
        {
            GameObject upCandy1 = _grid.allCandies[col, row + 1];
            GameObject downCandy1 = _grid.allCandies[col, row - 1];
            if (upCandy1 != null && downCandy1 != null)
            {
                if (upCandy1.tag == this.gameObject.tag && downCandy1.tag == this.gameObject.tag)
                {
                    upCandy1.GetComponent<Candy>().isMatched = true;
                    downCandy1.GetComponent<Candy>().isMatched = true;
                    isMatched = true;
                }
            }
        }
    }
    //Effects to know which candy you are selecting 
    void OnMouseOver()
    {
        shadow.SetActive(true);
    }

    void OnMouseExit()
    {
        shadow.SetActive(false);
    }
}
