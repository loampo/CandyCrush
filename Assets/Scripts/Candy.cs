using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using UnityEngine;

public class Candy : MonoBehaviour
{
    public bool isMatched = false;
    public int col;
    public int row;
    public int targetX;
    public int targetY;
    public int prevCol;
    public int prevRow;


    private MatchesManager match;
    private GridManager grid;

    private GameObject otherCandies;

    private Vector2 firstTouchPos;
    private Vector2 finalTouchPos;
    private Vector2 tempPos;

    public float swipeResist = 1f;
    public float swipeAngle = 0;

    // Start is called before the first frame update
    void Start()
    {
        grid = FindObjectOfType<GridManager>();
        match = FindObjectOfType<MatchesManager>();
    }

    // Update is called once per frame
    void Update()
    {
        //findMatches();
        if (isMatched)
        {
            SpriteRenderer mySprite = GetComponent<SpriteRenderer>();
            //mySprite.color = new Color(0f, 0f, 0f, .2f);
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
            //board.allDots[col, row] = this.gameObject;
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
            //Debug.Log(swipeAngle);
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
            GameObject leftDot1 = grid.allCandies[col - 1, row];
            GameObject rightDot1 = grid.allCandies[col + 1, row];
            if (leftDot1 != null && rightDot1 != null)
            {
                if (leftDot1.tag == this.gameObject.tag && rightDot1.tag == this.gameObject.tag)
                {
                    leftDot1.GetComponent<Candy>().isMatched = true;
                    rightDot1.GetComponent<Candy>().isMatched = true;
                    isMatched = true;
                }
            }
        }
        if (row > 0 && row < grid.height - 1)
        {
            GameObject upDot1 = grid.allCandies[col, row + 1];
            GameObject downDot1 = grid.allCandies[col, row - 1];
            if (upDot1 != null && downDot1 != null)
            {
                if (upDot1.tag == this.gameObject.tag && downDot1.tag == this.gameObject.tag)
                {
                    upDot1.GetComponent<Candy>().isMatched = true;
                    downDot1.GetComponent<Candy>().isMatched = true;
                    isMatched = true;
                }
            }
        }
    }
}
