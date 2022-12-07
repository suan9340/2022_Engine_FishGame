using System.Collections;
using System.Collections.Generic;
using UnityEditor.Rendering;
using UnityEngine;

public class TextureScrolling : MonoBehaviour
{
    [SerializeField] private Vector2 offset = Vector2.zero;

    private MeshRenderer mesh = null;

    private void Start()
    {
        mesh = GetComponent<MeshRenderer>();
    }

    private void Update()
    {
        mesh.material.mainTextureOffset += offset * Time.deltaTime;
    }
}
 