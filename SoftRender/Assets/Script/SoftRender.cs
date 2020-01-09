using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SoftRender : MonoBehaviour
{
    public int height = 600;
    public int width = 800;

    public RawImage window;

    private Main main;
    private Elyot3D _elyot3D;
    
    private Texture2D _texture;

    void Awake()
    {
        _texture = new Texture2D(width,height);
        window.GetComponent<RectTransform>().sizeDelta = new Vector2(width,height);
        window.texture = _texture;
        
        _elyot3D = new Elyot3D();
        main = new Main();
    }
    
    // Start is called before the first frame update
    void Start()
    {
        _elyot3D.Init(width,height,_texture);
        main.main(_elyot3D);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnPreRender()
    {
        
    }
}
