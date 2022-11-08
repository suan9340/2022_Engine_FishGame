using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "FishInfo", menuName = "SO/Fish")]
[Serializable]
public class FishInformationSO : ScriptableObject
{
    //[SerializeField] private List<Fishs> fish = new List<Fishs>();

    public string fishname;
    public float speed;
    public DefineManager.FishRating rating = DefineManager.FishRating.BRONZE;
}
