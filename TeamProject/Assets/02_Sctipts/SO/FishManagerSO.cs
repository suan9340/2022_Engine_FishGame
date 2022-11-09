using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "FishManager", menuName = "SO/CurrentFish")]
[Serializable]
public class FishManagerSO : ScriptableObject
{
    [Header("CurrentFish")]
    public FishInformationSO currrentFish;
}
