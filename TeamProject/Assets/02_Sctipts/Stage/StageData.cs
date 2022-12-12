using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq.Expressions;
using UnityEngine;

[CreateAssetMenu(fileName = "StageData", menuName = "SO/StageData")]
public class StageData : ScriptableObject
{
    public int currentStage = 1;

    [Space(20)]
    public List<StageBase> stageBase = new List<StageBase>();
}

[Serializable]
public class StageBase
{
    public string stageLevel;

    [Space(10)]
    [Range(0, 100)]
    public float maxTime = 50;
}

