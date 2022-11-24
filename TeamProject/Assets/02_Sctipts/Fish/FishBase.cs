using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class FishBase : MonoBehaviour
{
    public DefineManager.FishRating rating = DefineManager.FishRating.BRONZE;

    [Space(30)]
    public FishInformationSO fishInfo;

    private bool isMousePointOn = false;
    private bool isCharging = false;

    private Vector3 startPos = Vector3.zero;
    private Vector3 endPos = Vector3.zero;
    private Vector3 resetPos = Vector3.zero;

    public Vector2 force = Vector3.zero;

    public Vector2 minPower;
    public Vector2 maxPower;
    [Space(30)]
    public bool isMoving = false;


    private Outline outline;
    public LineRenderer lineRenderer;
    private Rigidbody myrigid;
    private FishManagerSO fishManagerSO;

    private void Awake()
    {
        ConnectingRatingFish();
        Cashing();
        SettingOutLineSetting();
    }

    private void Update()
    {
        ChargingLineRenderUpdate();
        LookFishRotationMousePostion();
    }

    private void Cashing()
    {
        outline = GetComponentInChildren<Outline>();
        myrigid = GetComponent<Rigidbody>();

        if (lineRenderer == null)
        {
            lineRenderer = GameObject.Find("ChargingLine").GetComponent<LineRenderer>();
        }
        //lineRenderer = GetComponentInChildren<LineRenderer>();
    }

    /// <summary>
    /// Connect Scriptable Object from EnumRating
    /// </summary>
    private void ConnectingRatingFish()
    {
        if (fishInfo != null) return;

        switch (rating)
        {
            case DefineManager.FishRating.BRONZE:
                fishInfo = Resources.Load<FishInformationSO>("SO/Bronze");
                break;

            case DefineManager.FishRating.SLIVER:
                fishInfo = Resources.Load<FishInformationSO>("SO/Sliver");
                break;

            case DefineManager.FishRating.PLATINUM:
                fishInfo = Resources.Load<FishInformationSO>("SO/Platinum");
                break;

            case DefineManager.FishRating.DIAMOND:
                fishInfo = Resources.Load<FishInformationSO>("SO/Diamond");
                break;
        }

        fishManagerSO = Resources.Load<FishManagerSO>("SO/FishManager");
    }


    #region OutlineCode

    private void SettingOutLineSetting()
    {
        outline.enabled = false;
        outline.OutlineColor = fishInfo.outlineColor;
        fishManagerSO.mouseOnFish = null;
    }

    private void OnMouseEnter()
    {
        CheckFishOutline(true);
    }
    private void OnMouseExit()
    {
        if (!isCharging)
            CheckFishOutline(false);
    }

    private void CheckFishOutline(bool _boolen)
    {
        fishManagerSO.mouseOnFish = fishInfo;

        if (fishManagerSO.currrentFish != null)
        {
            fishManagerSO.mouseOnFish = fishManagerSO.currrentFish;
            return;
        }

        if (isMousePointOn == _boolen) return;

        UIManager.Instance.FishUION();
        GameManager.Instance.FishCamSetting(transform);

        isMousePointOn = _boolen;

        outline.enabled = _boolen;
    }
    #endregion

    #region LineRender_Charging
    private void ChargingLineRenderUpdate()
    {
        if (!isMousePointOn) return;

        if (Input.GetMouseButtonDown(0))
        {
            MouseClick();
        }

        if (Input.GetMouseButton(0))
        {
            MouseDrag();
        }

        if (Input.GetMouseButtonUp(0))
        {
            MouseUp();
        }
    }

    private void MouseClick()
    {
        isCharging = true;
        lineRenderer.enabled = true;

        fishManagerSO.currrentFish = fishInfo;
        GameManager.Instance.FishCamSetting(transform);

        startPos = new Vector3(transform.position.x, transform.position.y, 0f);
        lineRenderer.SetPosition(0, startPos);
    }

    private void MouseDrag()
    {
        LookAtShootDir();
        endPos = MouseScreenValue();
        lineRenderer.SetPosition(1, endPos);
    }

    private void MouseUp()
    {
        lineRenderer.enabled = false;
        isCharging = false;

        fishManagerSO.currrentFish = null;

        lineRenderer.SetPosition(0, resetPos);
        lineRenderer.SetPosition(1, resetPos);

        var a = Vector3.Distance(startPos, endPos);

        var dir = startPos - endPos; dir.x = dir.y = 0;
        //Debug.Log($"차징 길이 차이는 {a}");

        force = new Vector2(Mathf.Clamp(startPos.x - endPos.x, minPower.x, maxPower.x),
                Mathf.Clamp(startPos.y - endPos.y, minPower.y, maxPower.y));
        myrigid.AddForce(force * fishInfo.speed, ForceMode.Impulse);

        CheckFishOutline(false);
    }

    private Vector3 MouseScreenValue()
    {
        Vector3 _pos = Camera.main.ScreenToWorldPoint(new Vector3(Input.mousePosition.x, Input.mousePosition.y, -Camera.main.transform.position.z));
        return _pos;
    }

    private void LookAtShootDir()
    {
        Vector3 _dir = new Vector3(0f, 0f, endPos.y);
        Vector3 _dir22 = new Vector3(0f, endPos.y, endPos.z);
        //transform.rotation = Quaternion.Euler(new Vector3(0f, 0f, endPos.y));
        transform.rotation = Quaternion.Euler(new Vector3(0f, 0f, endPos.z));
    }

    private void LookFishRotationMousePostion()
    {
        //Vector3 dir = new Vector3(0, 0, Input.GetAxis("Mouse Y"));
        //transform.Rotate(dir * 5);
    }

    //private IEnumerator FishMoving()
    //{
    //    while(transform.position <= )
    //    {

    //    }
    //    yield return null;
    //}


    #endregion
}
