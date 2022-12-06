using Mono.Cecil;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
using Unity.VisualScripting;

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

    [Space(50)]
    [Header("SettingUI")]
    [SerializeField] private GameObject SettingObj = null;
    [SerializeField] private GameObject ReallySettingDownObj = null;
    [SerializeField] private Text ReallyOutSettingText = null;
    [SerializeField] private List<string> settingText = new List<string>();
    public float fadeUISpeed = 0.03f;

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
        StartCoroutine(gameCountTextCorutine());
    }

    public void OnClickExit()
    {
        Debug.Log("GameOut");
        Application.Quit();
    }

    private void StartGameState()
    {
        gameStartImg.SetActive(false);
    }

    public IEnumerator gameCountTextCorutine()
    {
        yield return new WaitForSeconds(1f);

        gameCountText.gameObject.SetActive(true);

        gameCountText.DOFade(1f, 0.5f);

        for (int i = 3; i >= 1; i--)
        {
            gameCountText.text = $"{i}";
            yield return new WaitForSeconds(1f);
        }

        gameCountText.gameObject.SetActive(false);
        GameManager.Instance.ChangeGameState(DefineManager.GameState.PLAYING);
        yield break;
    }

    public void OnClickSetting()
    {
        StartCoroutine(SettingUIClick());
    }

    public void OnClickReallySettingOut()
    {
        StartCoroutine(ReallyOutSettingClick());
    }

    public IEnumerator SettingUIClick()
    {
        if (isUiMoving || ReallySettingDownObj.activeSelf) yield break;
        isSettingOn = !isSettingOn;

        float _alpha = 0;
        isUiMoving = true;

        if (isSettingOn)
        {
            SettingObj.SetActive(true);
            _alpha = 0;

            while (_alpha <= 1)
            {
                _alpha += fadeUISpeed;
                SettingObj.GetComponent<CanvasGroup>().alpha = _alpha;
                yield return null;
            }
            GameManager.Instance.ChangeGameState(DefineManager.GameState.SETTING);
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
            GameManager.Instance.ChangeGameState(DefineManager.GameState.PLAYING);
        }

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
            Debug.Log("메뉴로 나가기");
        }
        else if (ReallyOutSettingText.text == settingText[1])
        {
            Debug.Log("스테이지 다시시작하기");
        }
    }

    public void OnClickReallySettingNo()
    {
        OnClickReallySettingOut();
    }

}
