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

    public bool isMousePointOn = false;
    public bool isCharging = false;

    private Vector3 startPos = Vector3.zero;
    private Vector3 endPos = Vector3.zero;


    private Outline outline;
    private LineRenderer lineRenderer;

    private void Awake()
    {
        ConnectingRatingFish();
        Cashing();
        SettingOutLineSetting();
    }

    private void Update()
    {
        ChargingLineRenderUpdate();
    }

    private void Cashing()
    {
        outline = GetComponent<Outline>();
        lineRenderer = GetComponentInChildren<LineRenderer>();
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
    }


    #region OutlineCode

    private void SettingOutLineSetting()
    {
        outline.enabled = false;
        outline.OutlineColor = fishInfo.outlineColor;
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
        if (isMousePointOn == _boolen) return;

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
            isCharging = true;
            lineRenderer.enabled = true;
            //startPos = MouseScreenValue();
            startPos = new Vector3(transform.position.x, transform.position.y, 0f);
            lineRenderer.SetPosition(0, startPos);
        }

        if (Input.GetMouseButton(0))
        {
            endPos = MouseScreenValue();
            lineRenderer.SetPosition(1, endPos);
        }

        if (Input.GetMouseButtonUp(0))
        {
            lineRenderer.enabled = false;
            isCharging = false;

            CheckFishOutline(false);
        }
    }

    private Vector3 MouseScreenValue()
    {
        Vector3 _pos = Camera.main.ScreenToWorldPoint(new Vector3(Input.mousePosition.x, Input.mousePosition.y, -Camera.main.transform.position.z));
        return _pos;
    }
    #endregion
}
