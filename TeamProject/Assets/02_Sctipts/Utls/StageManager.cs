using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class StageManager : MonoBehaviour
{

    #region SingleTon

    private static StageManager _instance = null;
    public static StageManager Instance
    {
        get
        {
            if (_instance == null)
            {
                _instance = FindObjectOfType<StageManager>();
                if (_instance == null)
                {
                    _instance = new GameObject("StageManager").AddComponent<StageManager>();
                }
            }
            return _instance;
        }
    }

    #endregion

    private StageData stageData = null;
    public int arrLvl = 0;
    public int currentLvl = 0;

    [Space(50)]
    [Header("InGameUI")]
    [SerializeField] private Image timerImage = null;
    [SerializeField] private float maxTime = 5f;
    [SerializeField] private Text currentLevelTxt = null;
    [SerializeField] private Text clearLevelTxt = null;
    private bool isTimer = false;
    private bool isTimeStop = false;

    [Space(50)]
    [SerializeField] private List<GameObject> LevelFishObj = null;
    private void Awake()
    {
        stageData = Resources.Load<StageData>("SO/StageData");
        ConnectCurrentStage();
    }

    public void StagePlus()
    {
        clearLevelTxt.text = $"Level : {stageData.currentStage}";
        stageData.currentStage++;
        timerImage.fillAmount = 1f;

        ConnectCurrentStage();
    }

    public void ConnectCurrentStage()
    {
        currentLvl = stageData.currentStage;
        arrLvl = stageData.currentStage - 1;
        currentLevelTxt.text = $"Level {currentLvl}";
        maxTime = stageData.stageBase[arrLvl].maxTime;
    }

    public void InstantiateFishObj(GameObject _shark)
    {
        Instantiate(LevelFishObj[arrLvl]);
        _shark.transform.position = stageData.stageBase[arrLvl].sharkFirstTrn;
    }


    public void ResetStage()
    {
        isTimeStop = false;
        ConnectCurrentStage();
        StartCoroutine(TimerSetCorutine());
    }

    public IEnumerator TimerSetCorutine()
    {
        if (isTimer) yield break;
        isTimer = true;

        var _time = maxTime;
        timerImage.fillAmount = 1f;

        while (true)
        {
            if (isTimeStop)
            {
                isTimer = false;
                _time = maxTime;
                yield break;
            }


            if (GameManager.Instance.gameState == DefineManager.GameState.SETTING)
            {
                yield return new WaitWhile(() => GameManager.Instance.gameState != DefineManager.GameState.PLAYING);
            }


            if (timerImage.fillAmount <= 0)
            {
                isTimer = false;
                GameManager.Instance.isClear = true;
                UIManager.Instance.GameClearShowClear();
                yield break;
            }

            _time -= Time.deltaTime;
            timerImage.fillAmount = _time / maxTime;
            yield return null;
        }
    }

    public void StageStop()
    {
        isTimeStop = true;
    }

    public void timeImageFill()
    {
        timerImage.fillAmount = 1f;
    }
}
