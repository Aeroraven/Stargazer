using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace TinyRenderer
{
    class ArvnMesh
    {
        private List<float> vertices = new List<float>();
        private List<float> textureVertices = new List<float>();
        private List<float> vertexNormals = new List<float>();
        private List<int> faces = new List<int>();
        private List<int> facesTextureIdx = new List<int>();
        private List<int> facesVNormalIdx = new List<int>();
        private int vNums = 0;
        private int fNums = 0;
        private int vtNums = 0;
        private int vnNums = 0;
        public object[] ExportVertices()
        {
            object[] ret = new object[faces.Count ];
            for(int i = 0; i < faces.Count / 3 ; i++)
            {
                int a, b, c;
                GetFace(i, out a, out b, out c);
                a--;
                b--;
                c--;
                ret[i * 3] = new float[3] { vertices[3 * a], vertices[3 * a + 1], vertices[3 * a + 2] };
                ret[i * 3 + 1] = new float[3] { vertices[3 * b], vertices[3 * b + 1], vertices[3 * b + 2] };
                ret[i * 3 + 2] = new float[3] { vertices[3 * c], vertices[3 * c + 1], vertices[3 * c + 2] };
            }
            return ret;
        }
        public object[] ExportVertexNormals()
        {
            object[] ret = new object[faces.Count];
            for (int i = 0; i < faces.Count / 3; i++)
            {
                int a, b, c;
                GetFace(i, out a, out b, out c);
                a--;
                b--;
                c--;
                ret[i * 3] = new float[3] { vertexNormals[3 * a], vertexNormals[3 * a + 1], vertexNormals[3 * a + 2] };
                ret[i * 3 + 1] = new float[3] { vertexNormals[3 * b], vertexNormals[3 * b + 1], vertexNormals[3 * b + 2] };
                ret[i * 3 + 2] = new float[3] { vertexNormals[3 * c], vertexNormals[3 * c + 1], vertexNormals[3 * c + 2] };
            }
            return ret;
        }
        public object[] ExportVertexTexture()
        {
            object[] ret = new object[faces.Count];
            for (int i = 0; i < faces.Count / 3; i++)
            {
                int a, b, c;
                GetFaceVertexIdx(i, out a, out b, out c);
                a--;
                b--;
                c--;
                ret[i * 3] = new float[3] { textureVertices[3 * a], textureVertices[3 * a + 1], textureVertices[3 * a + 2] };
                ret[i * 3 + 1] = new float[3] { textureVertices[3 * b], textureVertices[3 * b + 1], textureVertices[3 * b + 2] };
                ret[i * 3 + 2] = new float[3] { textureVertices[3 * c], textureVertices[3 * c + 1], textureVertices[3 * c + 2] };
            }
            return ret;
        }
        public int[] ExportFaceIndexes()
        {
            int[] idx = new int[faces.Count];
            for(int i=0;i< faces.Count; i++)
            {
                idx[i] = i;
            }
            return idx;
        }
        public int GetTextureVertexNums()
        {
            return vtNums;
        }
        public int GetVertexNums()
        {
            return vNums;
        }
        public int GetVertexNormalNums()
        {
            return vnNums;
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
        public void GetFaceVertexIdx(int idx, out int ta, out int tb, out int tc)
        {
            ta = facesTextureIdx[3 * idx];
            tb = facesTextureIdx[3 * idx + 1];
            tc = facesTextureIdx[3 * idx + 2];
        }
        public void GetVertex(int idx, out float ta, out float tb, out float tc)
        {
            ta = vertices[3 * idx - 3];
            tb = vertices[3 * idx + 1 - 3];
            tc = vertices[3 * idx + 2 - 3];
        }

        public void GetVertexNormal(int idx, out float ta, out float tb, out float tc)
        {
            ta = vertexNormals[3 * idx - 3];
            tb = vertexNormals[3 * idx + 1 - 3];
            tc = vertexNormals[3 * idx + 2 - 3];
        }
        public void GetTextureVertex(int idx, out float ta, out float tb, out float tc)
        {
            ta = textureVertices[3 * idx - 3];
            tb = textureVertices[3 * idx + 1 - 3];
            tc = textureVertices[3 * idx + 2 - 3];
        }
        public void ParseFromWavefront(string path)
        {
            StreamReader sr = new StreamReader(path);
            string line;
            while ((line = sr.ReadLine()) != null)
            {
                line = line.Replace("  ", " ");
                string[] el = line.Split(" ");
                if (el[0] == "v")
                {
                    vNums++;
                    vertices.Add(float.Parse(el[1]));
                    vertices.Add(float.Parse(el[2]));
                    vertices.Add(float.Parse(el[3]));
                }
                if (el[0] == "vt")
                {
                    vtNums++;
                    textureVertices.Add(float.Parse(el[1]));
                    textureVertices.Add(float.Parse(el[2]));
                    textureVertices.Add(float.Parse(el[3]));
                }
                if (el[0] == "vn")
                {
                    vnNums++;
                    vertexNormals.Add(float.Parse(el[1]));
                    vertexNormals.Add(float.Parse(el[2]));
                    vertexNormals.Add(float.Parse(el[3]));
                }
                if (el[0] == "f")
                {
                    fNums++;
                    for(int i = 1; i <= 3; i++)
                    {
                        string[] g = el[i].Split("/");
                        faces.Add(int.Parse(g[0]));
                        if (g.Length >= 1)
                        {
                            facesTextureIdx.Add(int.Parse(g[1]));
                        }
                        if (g.Length >= 2)
                        {
                            facesVNormalIdx.Add(int.Parse(g[2]));
                        }

                    }
                }
            }
            sr.Close();
        }
    }
}
