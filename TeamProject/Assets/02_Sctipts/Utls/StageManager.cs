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

    [Space(50)]
    [SerializeField] private Text currentLevelTxt = null;
    [SerializeField] private Text clearLevelTxt = null;
    [SerializeField] private Text donClearLevelTxt = null;
    private bool isTimer = false;
    private bool isTimeStop = false;
    private int fakerStage = 0;

    [Space(50)]
    [SerializeField] private List<GameObject> LevelFishObj = null;
    private void Awake()
    {
        stageData = Resources.Load<StageData>("SO/StageData");
        ConnectCurrentStage();
        fakerStage = stageData.stageBase.Count;
        //PlayerPrefs.DeleteAll();
    }

    public void StagePlus()
    {
        clearLevelTxt.text = $"Level : {stageData.currentStage}";
        donClearLevelTxt.text = $"Level : {stageData.currentStage}";

        if (stageData.currentStage >= stageData.stageBase.Count)
        {
            stageData.currentStage = Random.Range(0, stageData.stageBase.Count);
        }
        else
        {
            stageData.currentStage++;
        }
        ConnectStagePlusStage();
        timerImage.fillAmount = 1f;
    }

    public void ConnectCurrentStage()
    {
        currentLvl = stageData.currentStage;
        arrLvl = stageData.currentStage - 1;
        currentLevelTxt.text = $"{currentLvl}";
        donClearLevelTxt.text = $"Level : {stageData.currentStage}";
        maxTime = stageData.stageBase[arrLvl].maxTime;
    }

    private void ConnectStagePlusStage()
    {
        currentLvl = stageData.currentStage;
        arrLvl = stageData.currentStage - 1;
        donClearLevelTxt.text = $"Level : {stageData.currentStage}";
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
                SoundManager.Instance.SoundAudio(5);
                UIManager.Instance.FishiUIReset();
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
