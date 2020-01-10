using UnityEngine;

public class Elyot3D
{
    public int height;
    public int width;

    private Texture2D frameBuffer;
    
    
    //Z-buffer
    private int[] zbuffer;
    public void Init(int width,int height,Texture2D _renderTexture)
    {
        this.height = height;
        this.width = width;
        this.frameBuffer = _renderTexture;
        
        zbuffer = new int[this.height * this.width];
        for (int i = 0; i < zbuffer.Length; i++)
        {
            zbuffer[i] = int.MinValue;
        }
    }

    private void Swap(ref int a,ref int b)
    {
        int temp = a;
        a = b;
        b = temp;
    }
    
    private void SwapVector2(ref Vector2 a,ref Vector2 b)
    {
        Vector2 temp = a;
        a = b;
        b = temp;
    }
    
    public void SetBackgroundColor(Color color)
    {
        for (int i = 0; i < width; i++)
        {
            for (int j = 0; j < height; j++)
            {
                frameBuffer.SetPixel(i,j,color);
            }
        }
    }

    public void Apply()
    {
        frameBuffer.Apply();
    }
    
    //画点
    public void Pixel(int i,int j,Color color)
    {
        frameBuffer.SetPixel(i,j,color);
    }
    
    //画线
    public void line(int x0, int y0, int x1, int y1,Color color) { 
        bool steep = false; 
        if (Mathf.Abs(x0-x1)<Mathf.Abs(y0-y1)) { 
            Swap(ref x0,ref y0); 
            Swap(ref x1,ref y1); 
            steep = true; 
        } 
        if (x0>x1) { 
            Swap(ref x0,ref x1); 
            Swap(ref y0, ref y1); 
        } 
        int dx = x1-x0; 
        int dy = y1-y0; 
        int derror2 = Mathf.Abs(dy)*2; 
        int error2 = 0; 
        int y = y0; 
        for (int x=x0; x<=x1; x++) { 
            if (steep) { 
                Pixel(y, x, color); 
            } else { 
                Pixel(x, y, color); 
            } 
            error2 += derror2; 
            if (error2 > dx) { 
                y += (y1>y0?1:-1); 
                error2 -= dx*2; 
            } 
        } 
    }
    
    //画三角形 扫描线算法
    public void triangle(Vector2 t0, Vector2 t1, Vector2 t2,Color color) { 
        
        // sort the vertices, t0, t1, t2 lower−to−upper (bubblesort yay!) 
        if (t0.y>t1.y) SwapVector2(ref t0, ref t1); 
        if (t0.y>t2.y) SwapVector2(ref t0, ref t2); 
        if (t1.y>t2.y) SwapVector2(ref t1, ref t2); 
        int total_height = (int)t2.y-(int)t0.y; 
        for (int y=(int)t0.y; y<=(int)t1.y; y++) { 
            int segment_height = (int)t1.y-(int)t0.y+1; 
            float alpha = (float)(y-t0.y)/total_height; 
            float beta  = (float)(y-t0.y)/segment_height; // be careful with divisions by zero 
            Vector2 A = t0 + (t2-t0)*alpha; 
            Vector2 B = t0 + (t1-t0)*beta; 
            if (A.x>B.x) SwapVector2(ref A, ref B); 
            for (int j=(int)A.x; j<=(int)B.x; j++) { 
                Pixel(j, y, color); // attention, due to int casts t0.y+i != A.y 
            } 
        } 
        for (int y=(int)t1.y; y<=(int)t2.y; y++) { 
            int segment_height =  (int)t2.y-(int)t1.y+1; 
            float alpha = (float)(y-t0.y)/total_height; 
            float beta  = (float)(y-t1.y)/segment_height; // be careful with divisions by zero 
            Vector2 A = t0 + (t2-t0)*alpha; 
            Vector2 B = t1 + (t2-t1)*beta; 
            if (A.x>B.x) SwapVector2(ref A, ref B); 
            for (int j=(int)A.x; j<=(int)B.x; j++) { 
                Pixel(j, y, color); // attention, due to int casts t0.y+i != A.y 
            } 
        } 
    }
    
    Vector3 barycentric(Vector3[] pts, Vector3 P) { 
        Vector3 u = Vector3.Cross(new Vector3(pts[2][0]-pts[0][0], pts[1][0]-pts[0][0], pts[0][0]-P[0]),new Vector3(pts[2][1]-pts[0][1], pts[1][1]-pts[0][1], pts[0][1]-P[1]));
        /* `pts` and `P` has integer value as coordinates
           so `abs(u[2])` < 1 means `u[2]` is 0, that means
           triangle is degenerate, in this case return something with negative coordinates */
        if (Mathf.Abs(u[2])<1) return new Vector3(-1,1,1);
        return new Vector3(1.0f-(u.x+u.y)/u.z, u.y/u.z, u.x/u.z); 
    }

    //重心检测算法
    public void triangle2(Vector3[] pts,Color color) { 
        Vector2 boxmin = new Vector2(width-1,height - 1);
        Vector2 boxmax = new Vector2(0,0);
        Vector2 clamp = new Vector2(width-1,height - 1);
        
        for (int i=0; i<3; i++) { 
            for (int j=0; j<2; j++) { 
                boxmin[j] = Mathf.Max(0,Mathf.Min(boxmin[j], pts[i][j])); 
                boxmax[j] = Mathf.Min(clamp[j], Mathf.Max(boxmax[j], pts[i][j])); 
            } 
        } 
        Vector3 P = new Vector3(1,1,1); 
        for (P.x=boxmin.x; P.x<=boxmax.x; P.x++) { 
            for (P.y=boxmin.y; P.y<=boxmax.y; P.y++) { 
                Vector3 bc_screen  = barycentric(pts, P); 
                if (bc_screen.x<0 || bc_screen.y<0 || bc_screen.z<0) continue; 
                
                P.z = 0;
                for (int i=0; i<3; i++) P.z += pts[i][2]*bc_screen[i];
                if (zbuffer[(int)P.x+(int)P.y*width]<P.z) {
                    zbuffer[(int)P.x+(int)P.y*width] = (int)P.z;
                    Pixel((int)P.x, (int)P.y, color); 
                }
            } 
        } 
    } 
    
    
    
}
