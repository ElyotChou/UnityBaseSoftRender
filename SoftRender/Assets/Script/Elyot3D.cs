using UnityEngine;

public class Elyot3D
{
    public int height;
    public int width;

    private Texture2D frameBuffer;
    public void Init(int width,int height,Texture2D _renderTexture)
    {
        this.height = height;
        this.width = width;
        this.frameBuffer = _renderTexture;
        

    }

    private void Swap(ref int a,ref int b)
    {
        int temp = a;
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
}
