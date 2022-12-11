using System;
using System.Collections;
using System.Collections.Generic;
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
    private bool isTimer = false;

    private void Awake()
    {
        stageData = Resources.Load<StageData>("SO/StageData");
    }

    private void Start()
    {
        ConnectCurrentStage();

        StartCoroutine(TimerSet());
    }

    public void StagePlus()
    {
        stageData.currentStage++;


        ConnectCurrentStage();
    }

    public void ConnectCurrentStage()
    {
        currentLvl = stageData.currentStage;
        arrLvl = stageData.currentStage - 1;
        currentLevelTxt.text = $"Level {currentLvl}";
        maxTime = stageData.stageBase[arrLvl].maxTime;
    }

    public IEnumerator TimerSet()
    {
        if (isTimer) yield break;
        isTimer = true;
        var _time = maxTime;

        while (true)
        {
            if (timerImage.fillAmount <= 0)
            {
                GameManager.Instance.isClear = true;
                Debug.Log("End");
                isTimer = false;
                yield break;
            }

            _time -= Time.deltaTime;
            timerImage.fillAmount = _time / maxTime;
            yield return null;
        }
    }
}
