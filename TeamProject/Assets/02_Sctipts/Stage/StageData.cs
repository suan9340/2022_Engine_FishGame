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
    [SerializeField] private List<StageBase> stageBase = new List<StageBase>();

    public StageData()
    {
        for (int i = 0; i < stageBase.Count; i++)
        {
            stageBase[i].stageLevel = $"Level {i + 1}";
        }
    }
}

[Serializable]
public class StageBase
{
    public string stageLevel;
    public int bronzenum;
    public int slivernum;
    public int platinumnum;
    public int diamondnum;

}

