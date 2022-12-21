using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChangeCusor : MonoBehaviour
{
    [SerializeField] private Texture2D cusorImg = null;

    private void Start()
    {
        Cursor.SetCursor(cusorImg, Vector2.zero, CursorMode.ForceSoftware);
    }
}
