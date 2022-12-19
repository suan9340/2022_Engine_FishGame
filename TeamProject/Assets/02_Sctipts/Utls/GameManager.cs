using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Jobs;

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

    public Camera fishCam = null;

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

    private void Start()
    {
        //StageManager.Instance.InstantiateFishObj(sharkObj);
        //Findfishies();
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

    public void FishCamSetting(Transform _trn)
    {
        fishCam.transform.position = new Vector3(_trn.position.x, _trn.position.y, -10f);

        if (fishCam.transform.parent == _trn) return;
        fishCam.transform.SetParent(_trn);
    }

    public void ResetFishCam()
    {
        fishCam.transform.SetParent(null);
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
                Debug.Log($"0 번째는 null");
            }
            else
            {
                fishMom = fishes[0];
                Debug.Log($"1 번째는 null");
            }
        }

        //Debug.Log(fishMom.transform.childCount);

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

        SoundManager.Instance.PlayerAttackSound(0);
        int _num = Random.Range(0, fishs.Count);
        Transform _trn = fishs[_num].gameObject.transform;
        _obj.transform.DOMove(new Vector3(_trn.position.x, transform.position.y, -4.78f), sharkMoveSpeed).SetEase(Ease.InCubic);

        yield return new WaitForSeconds(sharkMoveSpeed);
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
