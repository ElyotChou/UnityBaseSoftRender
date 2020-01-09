using System;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
 
public class LoadObj
{
    private List<Vector3> TVerts = new List<Vector3>();
    private List<Vector3> TNormals = new List<Vector3>();
    private List<int> TTriangles = new List<int>();
    private List<int> TNormalIndexs = new List<int>();
    private List<Mesh> meshs = new List<Mesh>();
    private bool readFinsh;

    public List<Vector3> Verts
    {
        get { return TVerts; }
    }
    
    public List<int> Triangles
    {
        get { return TTriangles; }
    }
    
    public void ReadInfoObj(string path)
    {
        readFinsh = false;
        if (!File.Exists(path)) return;
        string[] lines = File.ReadAllLines(path);
        foreach (string line in lines)
        {
            //解析内嵌材质信息
            string[] unit = System.Text.RegularExpressions.Regex.Split(line, "\\s+", System.Text.RegularExpressions.RegexOptions.IgnoreCase);
            switch (unit[0])
            {
                case "v":
                    TVerts.Add(new Vector3(float.Parse(unit[1]), float.Parse(unit[2]), float.Parse(unit[3])));
                    break;
                case "vn":
                    TNormals.Add(new Vector3(float.Parse(unit[1]), float.Parse(unit[2]), float.Parse(unit[3])));
                    break;
                case "f":
                    TTriangles.Add(int.Parse(unit[1].Split('/')[0]) - 1);
                    TNormalIndexs.Add(int.Parse(unit[1].Split('/')[2]) - 1);
                    TTriangles.Add(int.Parse(unit[2].Split('/')[0]) - 1);
                    TNormalIndexs.Add(int.Parse(unit[2].Split('/')[2]) - 1);
                    TTriangles.Add(int.Parse(unit[3].Split('/')[0]) - 1);
                    TNormalIndexs.Add(int.Parse(unit[3].Split('/')[2]) - 1);
                    break;
                default:
                    break;
            }
        }
        readFinsh = true;
    }
    void ConstructMesh() {
        List<int> triangles = new List<int>(TTriangles);
        List<Vector3> verts = new List<Vector3>(TVerts);
        List<Vector3> normals = new List<Vector3>(TNormals);
        Mesh mesh = new Mesh();
        mesh.name = "obj";
        mesh.vertices = verts.ToArray();
        mesh.normals = normals.ToArray();
        mesh.triangles = triangles.ToArray();
        meshs.Add(mesh);
        
    }
}
