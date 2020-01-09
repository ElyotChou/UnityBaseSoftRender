using System.Collections.Generic;
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

        for (int i = 0; i < trans.Count; i+=3)
        {
            Vector3 v0 = verts[trans[i]];
            Vector3 v1 =verts[trans[i+1]];
            
            int x0 = (int) ((v0.x+1.0f)*_elyot3D.width/2.0f); 
            int y0 = (int) ((v0.y+1.0f)*_elyot3D.height/2.0f); 
            int x1 = (int) ((v1.x+1.0f)*_elyot3D.width/2.0f); 
            int y1 = (int) ((v1.y+1.0f)*_elyot3D.height/2.0f); 
            _elyot3D.line(x0, y0, x1, y1,Color.white); 
            
            Vector3 v2 = verts[trans[i+1]];
            Vector3 v3 =verts[trans[i+2]];
            
            x0 = (int) ((v2.x+1.0f)*_elyot3D.width/2.0f); 
            y0 = (int) ((v2.y+1.0f)*_elyot3D.height/2.0f); 
            x1 = (int) ((v3.x+1.0f)*_elyot3D.width/2.0f); 
            y1 = (int) ((v3.y+1.0f)*_elyot3D.height/2.0f); 
            _elyot3D.line(x0, y0, x1, y1,Color.white); 
            
            Vector3 v4 =verts[trans[i + 2]];
            Vector3 v5 = verts[trans[i]];
            
            x0 = (int) ((v4.x+1.0f)*_elyot3D.width/2.0f); 
            y0 = (int) ((v4.y+1.0f)*_elyot3D.height/2.0f); 
            x1 = (int) ((v5.x+1.0f)*_elyot3D.width/2.0f); 
            y1 = (int) ((v5.y+1.0f)*_elyot3D.height/2.0f); 
            _elyot3D.line(x0, y0, x1, y1,Color.white); 
        }
        
        _elyot3D.Apply();
    }
}
