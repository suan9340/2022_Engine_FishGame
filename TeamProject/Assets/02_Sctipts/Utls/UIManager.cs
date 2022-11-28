using Mono.Cecil;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

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
    [SerializeField] private Text fishName;
    [SerializeField] private Image fishColor;
    [SerializeField] private Image fishSpeed;
    [SerializeField] private Image fishIcon;
    private bool isFishUI = false;


    [Space(50)]
    [SerializeField] private GameObject gameStartImg = null;
    [SerializeField] private Image gameStartbackImg = null;


    private void Awake()
    {
        currentFishSO = Resources.Load<FishManagerSO>("SO/FishManager");
    }

    private void Start()
    {
        if (fishInfoObject.activeSelf)
            fishInfoObject.SetActive(false);
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
        gameStartImg.transform.DOScale(new Vector3(9f, 9f, 9f), 2f).SetEase(Ease.Unset);
        Invoke(nameof(StartGameState), 2f);
    }

    public void OnClickExit()
    {
        Debug.Log("GameOut");
        Application.Quit();
    }

    private void StartGameState()
    {
        //gameStartbackImg.DOFade(0f, 1f,()=>{ });
        //gameStartImg.SetActive(false);
    }
}
