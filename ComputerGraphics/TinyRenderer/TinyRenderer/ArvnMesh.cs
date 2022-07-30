using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace TinyRenderer
{
    class ArvnMesh
    {
        private List<float> vertices = new List<float>();
        private List<int> faces = new List<int>();
        private int vNums = 0;
        private int fNums = 0;

        public int GetVertexNums()
        {
            return vNums;
        }

        public int GetFaceNums()
        {
            return fNums;
        }
        public void GetFace(int idx,out int ta,out int tb,out int tc)
        {
            ta = faces[3 * idx];
            tb = faces[3 * idx + 1];
            tc = faces[3 * idx + 2];
        }
        public void GetVertex(int idx, out float ta, out float tb, out float tc)
        {
            ta = vertices[3 * idx - 3];
            tb = vertices[3 * idx + 1 - 3];
            tc = vertices[3 * idx + 2 - 3];
        }

        public void ParseFromWavefront(string path)
        {
            StreamReader sr = new StreamReader(path);
            string line;
            while ((line = sr.ReadLine()) != null)
            {
                string[] el = line.Split(" ");
                if (el[0] == "v")
                {
                    vNums++;
                    vertices.Add(float.Parse(el[1]));
                    vertices.Add(float.Parse(el[2]));
                    vertices.Add(float.Parse(el[3]));
                }
                if(el[0] == "f")
                {
                    fNums++;
                    for(int i = 1; i <= 3; i++)
                    {
                        string[] g = el[i].Split("/");
                        faces.Add(int.Parse(g[0]));
                    }
                }
            }
            sr.Close();
        }
    }
}
