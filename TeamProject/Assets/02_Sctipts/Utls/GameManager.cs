using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Jobs;
using UnityEngine.XR;

public class GameManager : MonoBehaviour
{

    #region SingleTon

    private static GameManager _instance = null;
    public static GameManager Instance
    {
        get
        {
            if (_instance == null)
            {
                _instance = FindObjectOfType<GameManager>();
                if (_instance == null)
                {
                    _instance = new GameObject("GameManager").AddComponent<GameManager>();
                }
            }
            return _instance;
        }
    }

    #endregion


    public FishBase currentFish;

    [Space(20)]
    public DefineManager.GameState gameState;

    [Space(20)]
    [Header("Fishes")]
    public GameObject fishMom;
    public float sharkMoveSpeed;
    public bool isFishDie = false;
    public GameObject sharkObj = null;
    public List<GameObject> fishs = new List<GameObject>();

    public GameObject[] fishes;

    [Space(20)]
    public bool isClear = false;
    public ParticleSystem danger = null;
    public Vector3 dangerOffset = Vector3.zero;

    private void Start()
    {
       
    }

    public bool CheckCurrentFish()
    {
        if (currentFish == null)
        {
            return false;
        }
        else
        {
            return true;
        }
    }

    public void ChangeGameState(DefineManager.GameState _state)
    {
        gameState = _state;
    }

    public void Findfishies()
    {
        isFishDie = false;
        fishes = GameObject.FindGameObjectsWithTag("FishMom");
        ;
        if (fishes.Length == 1)
        {
            fishMom = fishes[0];
        }
        else
        {
            if (fishes[0] != null)
            {
                fishMom = fishes[1];
            }
            else
            {
                fishMom = fishes[0];
            }
        }

        isClear = false;

        fishs.Clear();

        foreach (Transform a in fishMom.transform)
        {
            if (a != null)
                fishs.Add(a.gameObject);
        }
    }

    public void SharkAttack(GameObject _obj)
    {
        StartCoroutine(FishCheckAttackState(_obj));
    }

    public IEnumerator FishCheckAttackState(GameObject _obj)
    {
        while (true)
        {
            yield return FishAttacks(_obj);
        }
    }

    public IEnumerator FishAttacks(GameObject _obj)
    {
        if (gameState != DefineManager.GameState.PLAYING) yield break;

        int _num = Random.Range(0, fishs.Count);
        SoundManager.Instance.SoundAudio(3);


        var _trn = fishs[_num].gameObject.transform.position;


        danger.gameObject.transform.position = _trn + dangerOffset;
        danger.gameObject.SetActive(true);
        Invoke(nameof(DangerDown), 0.1f);


        _obj.transform.DOMove(_trn, sharkMoveSpeed).SetEase(Ease.InCubic);


        yield return new WaitForSeconds(sharkMoveSpeed);
    }

    public void DangerDown()
    {
        danger.gameObject.SetActive(true);
    }

    public void RemoveFishList(GameObject _value)
    {
        fishs.Remove(_value.gameObject);
    }

    public void RemoveFishMomTransform()
    {
        Destroy(fishMom.gameObject);
        fishMom = null;
        fishs.Clear();
    }
}
