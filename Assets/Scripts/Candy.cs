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

    private MatchesManager match;
    private GridManager grid;

    private GameObject otherCandies;

    private Vector2 firstTouchPos;
    private Vector2 finalTouchPos;
    private Vector2 tempPos;

    public float swipeResist = 1f;
    public float swipeAngle = 0;

    void Start()
    {
        grid = FindObjectOfType<GridManager>();
        match = FindObjectOfType<MatchesManager>();
    }

    void Update()
    {
        //findMatches();
        if (isMatched)
        {
            SpriteRenderer mySprite = GetComponent<SpriteRenderer>();
        }
        targetX = col;
        targetY = row;
        if (Mathf.Abs(targetX - transform.position.x) > .1)
        {
            //Move towards target
            tempPos = new Vector2(targetX, transform.position.y);
            transform.position = Vector2.Lerp(transform.position, tempPos, .6f);
            if (grid.allCandies[col, row] != this.gameObject)
            {
                grid.allCandies[col, row] = this.gameObject;
            }
            match.findAllMatches();

        }
        else
        {
            //Directly set the position
            tempPos = new Vector2(targetX, transform.position.y);
            transform.position = tempPos;
            grid.allCandies[col, row] = this.gameObject;
        }
        if (Mathf.Abs(targetY - transform.position.y) > .1)
        {
            //Move towards target
            tempPos = new Vector2(transform.position.x, targetY);
            transform.position = Vector2.Lerp(transform.position, tempPos, .4f);
            if (grid.allCandies[col, row] != this.gameObject)
            {
                grid.allCandies[col, row] = this.gameObject;
            }
            match.findAllMatches();
        }
        else
        {
            //Directly set the position
            tempPos = new Vector2(transform.position.x, targetY);
            transform.position = tempPos;
        }
    }

    public IEnumerator checkMoveCo()
    {
        yield return new WaitForSeconds(.5f);
        if (otherCandies != null)
        {
            if (!isMatched && !otherCandies.GetComponent<Candy>().isMatched)
            {
                otherCandies.GetComponent<Candy>().row = row;
                otherCandies.GetComponent<Candy>().col = col;
                row = prevRow;
                col = prevCol;
                yield return new WaitForSeconds(.5f);
                grid.currentState = GridManager.gameState.move;
            }
            else
            {
                grid.destroyMatches();

            }
            otherCandies = null;
        }

    }

    private void OnMouseDown()
    {
        if (grid.currentState == GridManager.gameState.move)
        {
            firstTouchPos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        }
    }

    private void OnMouseUp()
    {
        if (grid.currentState == GridManager.gameState.move)
        {
            finalTouchPos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            calculateAngle();
        }
        else
        {
            grid.currentState = GridManager.gameState.move;
        }

    }

    private void calculateAngle()
    {
        if (Mathf.Abs(finalTouchPos.y - firstTouchPos.y) > swipeResist || Mathf.Abs(finalTouchPos.x - firstTouchPos.x) > swipeResist)
        {
            swipeAngle = Mathf.Atan2(finalTouchPos.y - firstTouchPos.y, finalTouchPos.x - firstTouchPos.x) * 180 / Mathf.PI;
            movePieces();
            grid.currentState = GridManager.gameState.wait;
        }

    }

    private void movePieces()
    {
        if (swipeAngle > -45 && swipeAngle <= 45 && col < grid.width - 1)
        {
            //Right Swipe
            otherCandies = grid.allCandies[col + 1, row];
            prevRow = row;
            prevCol = col;
            otherCandies.GetComponent<Candy>().col -= 1;
            col += 1;

        }
        else if (swipeAngle > 45 && swipeAngle <= 135 && row < grid.height - 1)
        {
            //Up Swipe
            otherCandies = grid.allCandies[col, row + 1];
            prevRow = row;
            prevCol = col;
            otherCandies.GetComponent<Candy>().row -= 1;
            row += 1;

        }
        else if ((swipeAngle > 135 || swipeAngle <= -135) && col > 0)
        {
            //Left Swipe
            otherCandies = grid.allCandies[col - 1, row];
            prevRow = row;
            prevCol = col;
            otherCandies.GetComponent<Candy>().col += 1;
            col -= 1;
        }
        else if (swipeAngle < -45 && swipeAngle >= -135 && row > 0)
        {
            //Down Swipe
            otherCandies = grid.allCandies[col, row - 1];
            prevRow = row;
            prevCol = col;
            otherCandies.GetComponent<Candy>().row += 1;
            row -= 1;
        }

        StartCoroutine(checkMoveCo());
    }

    private void findMatches()
    {
        if (col > 0 && col < grid.width - 1)
        {
            GameObject leftCandy1 = grid.allCandies[col - 1, row];
            GameObject rightCandy1 = grid.allCandies[col + 1, row];
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
        if (row > 0 && row < grid.height - 1)
        {
            GameObject upCandy1 = grid.allCandies[col, row + 1];
            GameObject downCandy1 = grid.allCandies[col, row - 1];
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
    void OnMouseOver()
    {
        shadow.SetActive(true);
    }

    void OnMouseExit()
    {
        shadow.SetActive(false);
    }
}
