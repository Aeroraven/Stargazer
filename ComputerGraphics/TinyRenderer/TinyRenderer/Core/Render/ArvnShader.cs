using System;
using System.Collections.Generic;
using System.Text;
using TinyRenderer.Core.Drawing;
using TinyRenderer.Shaders;

namespace TinyRenderer.Core.Render
{
    abstract class ArvnShader : IArvnShaderCaller
    {
        public static int arFragColor = 0;
        public static int arPosition = 1;

        public const int tpInt = 0;
        public const int tpFloat = 1;
        public const int tpVec2f = 2;
        public const int tpVec3f = 3;
        public const int tpVec4f = 4;
        public const int tpMat3f = 5;
        public const int tpMat4f = 6;
        public const int tpMat2f = 7;
        public const int tpSampler2d = 8;
        class ArvnShaderException : ApplicationException
        {
            public ArvnShaderException(string message) : base(message) { }
        }
        protected int activeIndex = 0;
        protected int activeVIndex = 0;
        private bool compiled = false;

        //Varyings
        int nVaryings = 0;
        protected Dictionary<string, int> varyingKeyMap = new Dictionary<string, int>();
        protected object[][] varyingList;
        protected int[] varyingTypeList;
        protected object[] interpolatedVaryingList;

        protected Dictionary<int, object[]> varyingListPre = new Dictionary<int, object[]>();
        protected Dictionary<int, object> interpolatedVaryingListPre = new Dictionary<int, object>();
        protected Dictionary<int, int> varyingTypeListPre = new Dictionary<int, int>();

        //Uniforms & Attributes
        protected Dictionary<string, object> varList = new Dictionary<string, object>();
        protected Dictionary<string, int> typeList = new Dictionary<string, int>();

        protected Dictionary<string, object[]> attributeList = new Dictionary<string, object[]>();
        protected Dictionary<string, int> attributeTypeList = new Dictionary<string, int>();

        private bool uniformChanged = false;

        //Internal
        protected object[] internalList = new object[2];
        public object GetInternalVariable(int varName)
        {
            return internalList[varName];
        }
        public void SetInternalVariable(int varName,object value)
        {
            internalList[varName] = value;
        }
        public void BeforeRunning()
        {
            if(!compiled)CacheShaderVariables();
            compiled = true;
        }
        protected void CacheShaderVariables()
        {
            varyingList = new object[nVaryings][];
            varyingTypeList = new int[nVaryings];
            interpolatedVaryingList = new object[nVaryings];
            for (int i = 0; i < nVaryings; i++)
            {
                varyingList[i] = varyingListPre[i];
                varyingTypeList[i] = varyingTypeListPre[i];
                //interpolatedVaryingList[i] = interpolatedVaryingListPre[i];
            }
        }
        protected int GetVertexNums()
        {
            int f = -1;
            foreach (string e in attributeList.Keys)
            {
                if (f == -1)
                {
                    f = attributeList[e].Length;
                }
                else
                {
                    if (attributeList[e].Length != f)
                    {
                        throw new ArvnShaderException("Length mismatches.");
                    }
                }
            }
            return f;
        }
        protected float[] Vec4f(float r, float g, float b, float a)
        {
            return new float[4] { r, g, b, a };
        }
        protected float[] Vec3f(float r, float g, float b)
        {
            return new float[3] { r, g, b };
        }
        protected float[] Vec2f(float x, float y)
        {
            return new float[2] { x, y };
        }
        protected ArvnShader()
        {
            DefineVariable("arPosition", tpVec4f, new float[4] { 0, 0, 0, 0 });
            DefineVariable("arFragColor", tpVec4f, new float[4] { 0, 0, 0, 0 });
        }
        public bool FindIsUniformChanged()
        {
            return uniformChanged;
        }
        public void SetUniformChangedState()
        {
            uniformChanged = false;
        }
        public void SetAttributeVariable(string varName, object[] value)
        {
            attributeList[varName] = value;
        }
        protected object GetAttributeVariable(string varName)
        {
            return attributeList[varName][activeIndex];
        }
        protected void DefineAttributeVariable(string varName, int typeName)
        {
            attributeList[varName] = null;
            attributeTypeList[varName] = typeName;
        }
        protected void DefineVaryingVariable(string varName, int typeName)
        {
            varyingKeyMap[varName] = nVaryings;
            varyingListPre[nVaryings] = new object[3];
            varyingTypeListPre[nVaryings] = typeName;
            nVaryings++;
        }
        protected object GetVaryingVariableByVertex(string varName, int idx)
        {
            return varyingList[varyingKeyMap[varName]][idx];
        }
        protected object GetVaryingVariable(string varName)
        {
            return interpolatedVaryingList[varyingKeyMap[varName]];
        }
        protected void SetVaryingVariable(string varName, object value)
        {
            int idx = activeVIndex;
            varyingList[varyingKeyMap[varName]][idx] = value;
        }
        protected void DefineVariable(string varName, int typeName, object value)
        {
            if (typeList.ContainsKey(varName))
            {
                throw new ArvnShaderException("Variable has already been defined");
            }
            varList.Add(varName, value);
            typeList.Add(varName, typeName);
        }
        public object GetVariable(string varName)
        {
            return varList[varName];
        }
        protected void SetVariableUnsafe(string varName, object value)
        {
            uniformChanged = true;
            varList[varName] = value;
        }
        public void SetVariable(string varName, object value)
        {
            uniformChanged = true;
            varList[varName] = value;
        }
        private void InterpolateVaryings(int id, float[] bc)
        {
            int tp = varyingTypeList[id];
            var tx = varyingList[id];
            object a = tx[0];
            object b = tx[1];
            object c = tx[2];
            float ta = bc[0];
            float tb = bc[1];
            float tc = bc[2];
            float dx, dy, dz, dw;
            switch (tp)
            {
                case tpInt:
                    interpolatedVaryingList[id] = ta * (int)a + tb * (int)b + tc * (int)c;
                    break;

                case tpFloat:
                    interpolatedVaryingList[id] = ta * (float)a + tb * (float)b + tc * (float)c;
                    break;

                case tpVec2f:
                    dx = ta * ((float[])a)[0] + tb * ((float[])b)[0] + tc * ((float[])c)[0];
                    dy = ta * ((float[])a)[1] + tb * ((float[])b)[1] + tc * ((float[])c)[1];
                    interpolatedVaryingList[id] = new float[] { dx, dy };
                    break;

                case tpVec3f:
                    float[] da = (float[])a;
                    float[] db = (float[])b;
                    float[] dc = (float[])c;
                    dx = ta * da[0] + tb * db[0] + tc * dc[0];
                    dy = ta * da[1] + tb * db[1] + tc * dc[1];
                    dz = ta * da[2] + tb * db[2] + tc * dc[2];
                    interpolatedVaryingList[id] = new float[] { dx, dy, dz };
                    break;

                case tpVec4f:
                    dx = ta * ((float[])a)[0] + tb * ((float[])b)[0] + tc * ((float[])c)[0];
                    dy = ta * ((float[])a)[1] + tb * ((float[])b)[1] + tc * ((float[])c)[1];
                    dz = ta * ((float[])a)[2] + tb * ((float[])b)[2] + tc * ((float[])c)[2];
                    dw = ta * ((float[])a)[3] + tb * ((float[])b)[3] + tc * ((float[])c)[3];
                    interpolatedVaryingList[id] = new float[] { dx, dy, dz, dw };
                    break;
            } 
        }
        private bool CheckVariable(int typeName, object value)
        {
            //Base
            if (typeName == tpInt)
            {
                if (value is int)
                {
                    return true;
                }
            }
            if (typeName == tpFloat)
            {
                if (value is float)
                {
                    return true;
                }
            }

            //Vectors
            if (typeName == tpVec4f)
            {
                if (value is float[] && ((float[])value).Length == 4)
                {
                    return true;
                }
            }
            if (typeName == tpVec3f)
            {
                if (value is float[] && ((float[])value).Length == 3)
                {
                    return true;
                }
            }
            if (typeName == tpVec2f)
            {
                if (value is float[] && ((float[])value).Length == 2)
                {
                    return true;
                }
            }
            //Matrix
            if (typeName == tpMat3f)
            {
                if (value is float[,] && ((float[,])value).GetLength(0) == 3 && ((float[,])value).GetLength(1) == 3)
                {
                    return true;
                }
            }
            if (typeName == tpMat4f)
            {
                if (value is float[,] && ((float[,])value).GetLength(0) == 4 && ((float[,])value).GetLength(1) == 4)
                {
                    return true;
                }
            }
            if (typeName == tpMat2f)
            {
                if (value is float[,] && ((float[,])value).GetLength(0) == 2 && ((float[,])value).GetLength(1) == 2)
                {
                    return true;
                }
            }
            //Texture
            if (typeName == tpSampler2d)
            {
                if (value is IArvnImage)
                {
                    return true;
                }
            }
            return false;
        }
        public void FragmentShader(float[] barycenterCoord, params object[] input)
        {
            for(int i=0;i<nVaryings;i++)
            {
                InterpolateVaryings(i, barycenterCoord);
            }
            FragmentShader();
        }
        public void VertexShader(int index, int vindex, params object[] input)
        {
            activeIndex = index;
            activeVIndex = vindex;
            ComputeDerivedVariables();
            VertexShader();
        }
        abstract public void VertexShader();
        abstract public void FragmentShader();
        abstract public void ComputeDerivedVariables();
    }
}
