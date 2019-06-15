using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScrollNormals : MonoBehaviour
{
    [SerializeField] private Vector2 scrollSpeed = Vector2.one * 0.25f;

    private Renderer _renderer;

    void Start()
    {
        _renderer = GetComponent<Renderer>();
    }

    void Update()
    {
        Vector2 currentOffset = _renderer.material.GetTextureOffset("_MainTex");
        Vector2 newOffset = currentOffset + (scrollSpeed * Time.deltaTime);
        _renderer.material.SetTextureOffset("_MainTex", newOffset);
    }
}
