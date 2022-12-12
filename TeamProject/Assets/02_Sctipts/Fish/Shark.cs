using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum SharkState
{
    IDLE,
    HIT,
    DIE
}

public class Shark : MonoBehaviour
{
    public SharkState state;

    private void Start()
    {
        GameManager.Instance.SharkAttack(gameObject);
    }
}
