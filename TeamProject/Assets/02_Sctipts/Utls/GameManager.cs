using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices.WindowsRuntime;
using UnityEngine;

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

    public void ChangeGameState(DefineManager.GameState _state)
    {
        gameState = _state;
    }
}
