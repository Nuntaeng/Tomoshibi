using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum PlayerState
{
    Active, Hiding, Idle, Dead
}

public class PlayerStatus : MonoBehaviour {
    public PlayerState state;
    public float moveSpeed;
    public float maxLightPower;
    public float lightPower;
    //感知度と感知カウント
    public float detectPoint;
    public int detectCount;

    public int posX, posY;
    public Vector2 direction;
    public Vector2 checkPos;

    private void Awake()
    {
        checkPos = new Vector2(-100.0f, -100.0f);
    }
}
