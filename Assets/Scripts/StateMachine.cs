using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StateMachine : MonoBehaviour
{
    public enum gameState
    {
        wait,
        move
    }


    public gameState currentState = gameState.move;
}
