using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class DefineManager
{
    public enum FishRating
    {
        BRONZE,
        SLIVER,
        PLATINUM,
        DIAMOND,
    }

    public enum GameState
    {
        MENU,
        PLAYING,
        SETTING,
        FINISH,
    }
}
