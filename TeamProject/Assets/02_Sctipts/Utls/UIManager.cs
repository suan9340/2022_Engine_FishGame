using Mono.Cecil;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
using Unity.VisualScripting;
using UnityEditorInternal;

public class UIManager : MonoBehaviour
{
    #region SingleTon

    private static UIManager _instance = null;
    public static UIManager Instance
    {
        get
        {
            if (_instance == null)
            {
                _instance = FindObjectOfType<UIManager>();
                if (_instance == null)
                {
                    _instance = new GameObject("UIManager").AddComponent<UIManager>();
                }
            }
            return _instance;
        }
    }

    #endregion

    private FishManagerSO currentFishSO;

    [Header("FishInfos")]
    [SerializeField] private GameObject fishInfoObject = null;
    [SerializeField] private Text fishName = null;
    [SerializeField] private Image fishColor = null;
    [SerializeField] private Image fishSpeed = null;
    [SerializeField] private Image fishIcon = null;
    private bool isFishUI = false;


    [Space(50)]
    [Header("Menu UI")]
    [SerializeField] private GameObject gameStartImg = null;
    [SerializeField] private Image gameStartbackImg = null;
    [SerializeField] private float startAnimationSpeed = 3f;


    [Space(50)]
    [Header("InGameUI")]
    [SerializeField] private Text gameCountText = null;
    [SerializeField] private Text levelText = null;
    [SerializeField] private RectTransform centTrn = null;
    private bool isCountDown = false;

    [Space(50)]
    [Header("SettingUI")]
    [SerializeField] private GameObject SettingObj = null;
    [SerializeField] private GameObject ReallySettingDownObj = null;
    [SerializeField] private Text ReallyOutSettingText = null;
    [SerializeField] private List<string> settingText = new List<string>();
    public float fadeUISpeed = 0.03f;


    [Space(50)]
    [Header("GameClearUI")]
    [SerializeField] private GameObject gameClearObj = null;


    private bool isSettingOn = false;
    private bool isReallySettingOn = false;
    private bool isUiMoving = false;


    private void Awake()
    {
        currentFishSO = Resources.Load<FishManagerSO>("SO/FishManager");
    }

    private void Start()
    {
        if (fishInfoObject.activeSelf)
            fishInfoObject.SetActive(false);

        SettingObj.GetComponent<CanvasGroup>().alpha = 0f;
        ReallySettingDownObj.GetComponent<CanvasGroup>().alpha = 0f;
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            OnClickSetting();
        }

        if (Input.GetKeyDown(KeyCode.C))
        {
            StartCoroutine(GameClear());
        }
    }

    public void FishUION()
    {
        isFishUI = !isFishUI;

        if (isFishUI)
        {
            fishInfoObject.SetActive(true);
            fishName.text = currentFishSO.mouseOnFish.fishname;
            fishSpeed.fillAmount = currentFishSO.mouseOnFish.speed / 10;
            fishColor.color = currentFishSO.mouseOnFish.outlineColor;
        }
        else
        {
            fishInfoObject.SetActive(false);
        }
    }

    public void OnClickStart()
    {
        //gameStartImg.SetActive(false);
        gameStartImg.transform.DOScale(new Vector3(9f, 9f, 9f), startAnimationSpeed).SetEase(Ease.Unset);
        gameStartbackImg.DOFade(0f, startAnimationSpeed);
        Invoke(nameof(StartGameState), startAnimationSpeed);
        StartCoroutine(gameCountTextCorutine(true));
    }

    public void OnClickExit()
    {
        Debug.Log("GameOut");
        Application.Quit();
    }

    private void StartGameState()
    {
        gameStartImg.transform.DOScale(new Vector3(1f, 1f, 1f), 0.1f);
        gameStartImg.SetActive(false);
    }

    public void OnClickSetting()
    {
        StartCoroutine(SettingUIClick());
    }

    public void OnClickReallySettingOut()
    {
        StartCoroutine(ReallyOutSettingClick());
    }


    public IEnumerator gameCountTextCorutine(bool _isdelay)
    {
        isCountDown = true;

        if (_isdelay)
            yield return new WaitForSeconds(1f);

        gameCountText.gameObject.SetActive(true);

        gameCountText.DOFade(1f, 0.5f);


        for (int i = 3; i >= 1; i--)
        {
            gameCountText.text = $"{i}";
            yield return new WaitForSeconds(1f);
        }
        isCountDown = false;

        gameCountText.DOFade(0f, 0.5f);

        yield return new WaitForSeconds(0.5f);

        GameManager.Instance.ChangeGameState(DefineManager.GameState.PLAYING);
        gameCountText.gameObject.SetActive(false);
        yield break;
    }


    public IEnumerator SettingUIClick()
    {
        if (isUiMoving || ReallySettingDownObj.activeSelf || isCountDown || GameManager.Instance.gameState == DefineManager.GameState.CLEAR) yield break;
        isSettingOn = !isSettingOn;

        float _alpha = 0;
        isUiMoving = true;

        if (isSettingOn)
        {
            GameManager.Instance.ChangeGameState(DefineManager.GameState.SETTING);


            SettingObj.SetActive(true);
            _alpha = 0;
            while (_alpha <= 1)
            {
                _alpha += fadeUISpeed;
                SettingObj.GetComponent<CanvasGroup>().alpha = _alpha;
                yield return null;
            }
        }
        else
        {
            _alpha = 1;

            while (_alpha >= 0)
            {
                _alpha -= fadeUISpeed;
                SettingObj.GetComponent<CanvasGroup>().alpha = _alpha;
                yield return null;
            }

            SettingObj.SetActive(false);
            StartCoroutine(gameCountTextCorutine(false));
        }

        isUiMoving = false;
        yield break;
    }

    public IEnumerator MenuFade()
    {
        if (isUiMoving) yield break;

        float _alpha = 0;
        isUiMoving = true;

        gameStartImg.SetActive(true);
        _alpha = 0;

        while (_alpha <= 1)
        {
            _alpha += fadeUISpeed;
            gameStartImg.GetComponent<CanvasGroup>().alpha = _alpha;
            yield return null;
        }
        GameManager.Instance.ChangeGameState(DefineManager.GameState.MENU);


        isUiMoving = false;
        yield break;
    }

    public IEnumerator GameClearObjFadeOut()
    {
        if (isUiMoving) yield break;

        float _alpha = 0;
        isUiMoving = true;

        gameClearObj.SetActive(true);
        _alpha = 1;

        while (_alpha >= 0)
        {
            _alpha -= fadeUISpeed;
            gameClearObj.GetComponent<CanvasGroup>().alpha = _alpha;
            yield return null;
        }
        GameManager.Instance.ChangeGameState(DefineManager.GameState.CLEAR);


        isUiMoving = false;
        yield break;
    }

    public IEnumerator ReallyOutSettingClick()
    {
        if (isUiMoving) yield break;
        isReallySettingOn = !isReallySettingOn;

        float _alpha = 0;
        isUiMoving = true;

        if (isReallySettingOn)
        {
            ReallySettingDownObj.SetActive(true);
            _alpha = 0;

            while (_alpha <= 1)
            {
                _alpha += fadeUISpeed;
                ReallySettingDownObj.GetComponent<CanvasGroup>().alpha = _alpha;
                yield return null;
            }
        }
        else
        {
            _alpha = 1;

            while (_alpha >= 0)
            {
                _alpha -= fadeUISpeed;
                ReallySettingDownObj.GetComponent<CanvasGroup>().alpha = _alpha;
                yield return null;
            }

            ReallySettingDownObj.SetActive(false);
        }

        isUiMoving = false;
        yield break;
    }


    public IEnumerator GameClear()
    {
        if (isUiMoving || GameManager.Instance.gameState != DefineManager.GameState.PLAYING) yield break;

        float _alpha = 0;
        isUiMoving = true;

        gameClearObj.SetActive(true);
        _alpha = 0;

        while (_alpha <= 1)
        {
            _alpha += fadeUISpeed;
            gameClearObj.GetComponent<CanvasGroup>().alpha = _alpha;
            yield return null;
        }
        GameManager.Instance.ChangeGameState(DefineManager.GameState.CLEAR);




        isUiMoving = false;
        yield break;
    }

    public IEnumerator LevelAnimation()
    {
        levelText.gameObject.SetActive(false);

        yield return GameClearObjFadeOut();

        levelText.gameObject.SetActive(true);

        StartCoroutine(gameCountTextCorutine(false));
    }

    public IEnumerator MenuObject()
    {
        if (isUiMoving) yield break;

        float _alpha = 0;
        isUiMoving = true;




        gameStartImg.SetActive(true);
        _alpha = 0;

        while (_alpha <= 1)
        {
            _alpha += fadeUISpeed;
            gameStartImg.GetComponent<CanvasGroup>().alpha = _alpha;
            yield return null;
        }
        GameManager.Instance.ChangeGameState(DefineManager.GameState.MENU);




        isUiMoving = false;
        yield break;
    }

    public void OnClickGoToMenu()
    {
        ReallyOutSettingText.text = settingText[0];

        OnClickReallySettingOut();
    }

    public void OnClickStageRestart()
    {
        ReallyOutSettingText.text = settingText[1];

        OnClickReallySettingOut();
    }

    public void OnClickReallySettingYes()
    {
        if (ReallyOutSettingText.text == settingText[0])
        {
            Debug.Log("�޴��� ������");
        }
        else if (ReallyOutSettingText.text == settingText[1])
        {
            Debug.Log("�������� �ٽý����ϱ�");
        }
    }

    public void OnClickReallySettingNo()
    {
        OnClickReallySettingOut();
    }

    public void SetMenu()
    {
        if (SettingObj.activeSelf)
        {
            StartCoroutine(SettingUIClick());
        }

        if (gameClearObj.activeSelf)
        {
            gameClearObj.SetActive(false);
        }

        StartCoroutine(MenuFade());
    }
    public void OnClickNextLevel()
    {
        StageManager.Instance.StagePlus();

        StartCoroutine(LevelAnimation());
    }

}
