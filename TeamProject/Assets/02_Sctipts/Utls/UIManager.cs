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
    [SerializeField] private Image attackEffectImage = null;
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
    private bool isGameClear = false;


    [Space(50)]
    [Header("GameDontClearUI")]
    [SerializeField] private GameObject gameDonClearObj = null;
    private bool isGameDonClear = false;



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
            StartCoroutine(GameClearCorutine());
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
        StartCoroutine(SettingUIClickCorutine());
    }

    public void OnClickReallySettingOut()
    {
        StartCoroutine(ReallyOutSettingClickCorutine());
    }

    public void GameClearShowClear()
    {
        if (GameManager.Instance.gameState == DefineManager.GameState.DONTCLEAR) return;

        
        StartCoroutine(GameClearCorutine());
    }

    public void FishAttackEffect()
    {
        ShackeCam(0.08f, 0.7f, 0);
        StartCoroutine(FishAttackEffectCorutine());
    }

    public void ShackeCam(float _dur, float _str, int _vib)
    {
        Camera.main.DOShakePosition(_dur, _str, _vib).OnComplete(() => { Camera.main.transform.position = new Vector3(0, 2.66f, -10); });
    }


    public void GameDonClear()
    {
        if (GameManager.Instance.gameState == DefineManager.GameState.CLEAR) return;

        GameManager.Instance.ResetFishCam();
        StageManager.Instance.StageStop();
        StartCoroutine(GameDonDlearCorutine());
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
            if (_isdelay && i == 1)
            {
                gameCountText.fontSize = 72;
                gameCountText.text = $"상어를 피해 도망치세요!";
            }
            else
            {
                gameCountText.text = $"{i}";
            }
            yield return new WaitForSeconds(1f);
        }

        isCountDown = false;

        gameCountText.DOFade(0f, 0.5f);

        yield return new WaitForSeconds(0.5f);

        GameManager.Instance.ChangeGameState(DefineManager.GameState.PLAYING);
        gameCountText.gameObject.SetActive(false);
        gameCountText.fontSize = 150;

        yield break;
    }

    public IEnumerator SettingUIClickCorutine()
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

    public IEnumerator MenuFadeCorutine()
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

    public IEnumerator ReallyOutSettingClickCorutine()
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

    public IEnumerator GameClearCorutine()
    {
        if (isUiMoving) yield break;

        isGameClear = !isGameClear;

        float _alpha = 0;
        isUiMoving = true;

        gameClearObj.SetActive(true);

        if (isGameClear)
        {
            levelText.gameObject.SetActive(false);
            StageManager.Instance.StagePlus();

            _alpha = 0;
            while (_alpha <= 1)
            {
                _alpha += fadeUISpeed;
                gameClearObj.GetComponent<CanvasGroup>().alpha = _alpha;
                yield return null;
            }
            GameManager.Instance.ChangeGameState(DefineManager.GameState.CLEAR);
        }
        else
        {
            _alpha = 1;
            while (_alpha >= 0)
            {
                _alpha -= fadeUISpeed;
                gameClearObj.GetComponent<CanvasGroup>().alpha = _alpha;
                yield return null;
            }

            gameClearObj.SetActive(false);
            GameManager.Instance.ChangeGameState(DefineManager.GameState.SETTING);
        }

        isUiMoving = false;
        yield break;
    }

    public IEnumerator GameDonDlearCorutine()
    {
        if (isUiMoving) yield break;

        isGameDonClear = !isGameDonClear;

        float _alpha = 0;
        isUiMoving = true;

        if (isGameDonClear)
        {
            gameDonClearObj.SetActive(true);
            _alpha = 0;
            while (_alpha <= 1)
            {
                _alpha += fadeUISpeed;
                gameDonClearObj.GetComponent<CanvasGroup>().alpha = _alpha;
                yield return null;
            }
            GameManager.Instance.ChangeGameState(DefineManager.GameState.DONTCLEAR);
        }
        else
        {
            _alpha = 1;
            while (_alpha >= 0)
            {
                _alpha -= fadeUISpeed;
                gameDonClearObj.GetComponent<CanvasGroup>().alpha = _alpha;
                yield return null;
            }

            gameDonClearObj.SetActive(false);
            GameManager.Instance.ChangeGameState(DefineManager.GameState.SETTING);
        }

        isUiMoving = false;
        yield break;
    }

    public IEnumerator LevelAnimationCorutine()
    {
        yield return GameClearCorutine();

        yield return gameCountTextCorutine(false);

        StageManager.Instance.ResetStage();

        yield break;
    }

    public IEnumerator ReLevelAnimationCorutine()
    {
        yield return StartCoroutine(GameDonDlearCorutine());

        yield return StartCoroutine(gameCountTextCorutine(false));

        StageManager.Instance.ResetStage();

        yield break;
    }

    public IEnumerator MenuObjectCorutine()
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

    public IEnumerator FishAttackEffectCorutine()
    {
        for (int i = 0; i < 2; i++)
        {
            attackEffectImage.gameObject.SetActive(true);
            yield return new WaitForSeconds(0.15f);
            attackEffectImage.gameObject.SetActive(false);
            yield return new WaitForSeconds(0.15f);
        }

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

    public void SetMenu()
    {
        if (SettingObj.activeSelf)
        {
            StartCoroutine(SettingUIClickCorutine());
        }

        if (gameClearObj.activeSelf)
        {
            gameClearObj.SetActive(false);
        }

        StartCoroutine(MenuFadeCorutine());
    }

    public void OnClickNextLevel()
    {
        GameManager.Instance.RemoveFishMomTransform();

        StageManager.Instance.InstantiateFishObj(GameManager.Instance.sharkObj);

        GameManager.Instance.Findfishies();

        levelText.gameObject.SetActive(true);

        StartCoroutine(LevelAnimationCorutine());
    }

    public void OnClickGameDonClickRestart()
    {
        StageManager.Instance.timeImageFill();

        GameManager.Instance.RemoveFishMomTransform();

        StageManager.Instance.InstantiateFishObj(GameManager.Instance.sharkObj);

        GameManager.Instance.Findfishies();

        StartCoroutine(ReLevelAnimationCorutine());

    }
}
