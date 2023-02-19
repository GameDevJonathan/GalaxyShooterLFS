using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScrollingBackground : MonoBehaviour
{
    public float speed;

    [SerializeField]
    private Renderer _renderer;
    // Start is called before the first frame update

    // Update is called once per frame
    void Update()
    {
        _renderer.material.mainTextureOffset += Vector2.up * speed * Time.deltaTime;
    }
}
