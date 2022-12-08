using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using DG.Tweening;
using TMPro;

public enum SharkState
{
    IDLE,
    HIT,
    DIE
}

public class Shark : MonoBehaviour
{
    private Rigidbody myrigid;
    public SharkState state;
    public Transform fishMom;

    public List<GameObject> fishs = new List<GameObject>();

    [Space(10)]
    public float moveSpeed;


    private void Start()
    {
        myrigid = GetComponent<Rigidbody>();

        foreach (Transform a in fishMom)
        {
            fishs.Add(a.gameObject);
        }

        GameManager.Instance.SharkAttack(gameObject);
    }
}
