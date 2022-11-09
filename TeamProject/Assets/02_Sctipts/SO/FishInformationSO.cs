using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "FishInfo", menuName = "SO/Fish")]
[Serializable]
public class FishInformationSO : ScriptableObject
{
    public string fishname;
    public DefineManager.FishRating rating = DefineManager.FishRating.BRONZE;
    public float speed;
    public Color outlineColor;
}
