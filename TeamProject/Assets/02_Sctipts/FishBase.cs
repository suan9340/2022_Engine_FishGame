using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[Serializable]
public class FishsInfo
{
    public string name;
    public float speed;
    public Color outlineColor;
    public DefineManager.FishRating rating = DefineManager.FishRating.BRONZE;
}

public class FishBase : MonoBehaviour
{
    public FishsInfo fish;
    public bool isMousePointOn = false;
    public Outline outline;

    private void Awake()
    {
        outline = GetComponent<Outline>();
        outline.enabled = false;
        outline.OutlineColor = fish.outlineColor;
    }

    private void OnMouseEnter()
    {
        CheckFishOutline(true);
    }
    private void OnMouseExit()
    {
        CheckFishOutline(false);
    }

    private void CheckFishOutline(bool _boolen)
    {
        if (isMousePointOn == _boolen) return;

        isMousePointOn = _boolen;
        outline.enabled = _boolen;
    }
}
