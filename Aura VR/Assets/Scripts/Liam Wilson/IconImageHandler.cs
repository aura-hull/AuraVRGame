using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IconImageHandler : MonoBehaviour
{
    [SerializeField] private MeshRenderer targetRenderer;
    [SerializeField] private Texture texture;

    void Start()
    {
        if (targetRenderer == null) return;
        if (texture == null) return;
        targetRenderer.material.mainTexture = texture;
    }
}
