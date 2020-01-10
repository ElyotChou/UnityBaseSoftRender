﻿using System.Collections.Generic;
using UnityEngine;

public class Main
{
    private Elyot3D _elyot3D;
    
    private LoadObj _loadObj;
    public void main(Elyot3D elyot3D)
    {
        this._elyot3D = elyot3D;
        
        _loadObj = new LoadObj();
        _loadObj.ReadInfoObj(Application.dataPath + "/african_head.obj");
        MainFunc();
    }

    public void MainFunc()
    {
        _elyot3D.SetBackgroundColor(Color.black);

        
//        _elyot3D.line(13, 20, 80, 40,Color.white); 
//        _elyot3D.line(20, 13, 40, 80,Color.red); 
//        _elyot3D.line(80, 40, 13, 20,Color.red);

        List<Vector3> verts = _loadObj.Verts;
        List<int> trans = _loadObj.Triangles;

        Vector3 lightDir = Vector3.forward;
        
        for (int i = 0; i < trans.Count; i+=3)
        {
            Vector3 v0 = verts[trans[i]];
            Vector3 v1 = verts[trans[i+1]];
            Vector3 v2 =verts[trans[i + 2]];

            Vector3 scenecoord1 = new Vector3((int)((v0.x +1.0f)*_elyot3D.width/2.0f),(int)((v0.y +1.0f)*_elyot3D.height/2.0f),v0.z); 
            Vector3 scenecoord2 = new Vector3((int)((v1.x +1.0f)*_elyot3D.width/2.0f),(int)((v1.y +1.0f)*_elyot3D.height/2.0f),v1.z); 
            Vector3 scenecoord3 = new Vector3((int)((v2.x +1.0f)*_elyot3D.width/2.0f),(int)((v2.y +1.0f)*_elyot3D.height/2.0f),v2.z); 
            
            Vector3 n = Vector3.Cross(v1 - v0, v2 - v1);
            n = n.normalized;

            float intensity = Vector3.Dot(n, lightDir);
            intensity = Mathf.Abs(intensity);
            
            
            _elyot3D.triangle2(new Vector3[] {scenecoord1, scenecoord2, scenecoord3},
                new Color(intensity, intensity, intensity, 1));
            
//            _elyot3D.triangle(scenecoord1, scenecoord2, scenecoord3,new Color(intensity,intensity,intensity,1)); 
        }

//        Vector2[] t0 = {new Vector2(10, 70),   new Vector2(50, 160),  new Vector2(70, 80)}; 
//        Vector2[] t1 = {new Vector2(180, 50),  new Vector2(150, 1),   new Vector2(70, 180)}; 
//        Vector2[] t2 = {new Vector2(180, 150), new Vector2(140, 300), new Vector2(200, 180)}; 
//        _elyot3D.triangle(t0[0], t0[1], t0[2],Color.red); 
//        _elyot3D.triangle(t1[0], t1[1], t1[2],Color.white); 
//        _elyot3D.triangle(t2[0], t2[1], t2[2],Color.green);
        
        _elyot3D.Apply();
    }
}
