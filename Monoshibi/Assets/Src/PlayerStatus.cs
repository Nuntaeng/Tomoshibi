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
    //光の最大値と現在地、光で確保される視界のサイズの最低値
    public float maxLightPower;
    public float lightPower;
    [HideInInspector]
    public float minLightCircle = 80.0f;
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
