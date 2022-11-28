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

    private bool isFishHit;




    private void Start()
    {
        myrigid = GetComponent<Rigidbody>();

        foreach (Transform a in fishMom)
        {
            fishs.Add(a.gameObject);
        }

        StartCoroutine(SharkStateCorutine());
    }

    public IEnumerator SharkStateCorutine()
    {
        while (true)
        {
            StartCoroutine(FishAttacks());

            yield return new WaitForSeconds(moveSpeed);

            Debug.Log("erwerwerwerwe");
        }
    }

    private void ChangeState(SharkState _changestate)
    {
        state = _changestate;
        Debug.Log($"Current State {state}");
    }


    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag(ConstantManager.TAG_FISH))
        {
            if (isFishHit) return;
            isFishHit = true;

            fishs.Remove(other.gameObject);
        }
    }


    private IEnumerator FishAttacks()
    {
        int _num = Random.Range(0, fishs.Count);
        Transform _trn = fishs[_num].gameObject.transform;
        gameObject.transform.DOMove(_trn.position, moveSpeed).SetEase(Ease.Linear);
        Debug.Log("qwe");
        yield return null;
    }

}
