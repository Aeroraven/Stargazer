using System;
using System.Collections.Generic;
using System.Text;
using TinyRenderer.Core.Drawing;
using TinyRenderer.Shaders;

namespace TinyRenderer.Core.Render
{
    abstract class ArvnShader : IArvnShaderCaller
    {
        public static string arFragColor = "arFragColor";
        public static string arPosition = "arPosition";
        class ArvnShaderException : ApplicationException
        {
            public ArvnShaderException(string message) : base(message) { }
        }
        protected int activeIndex = 0;
        protected int activeVIndex = 0;

        protected Dictionary<string, object[]> varyingList = new Dictionary<string, object[]>();
        protected Dictionary<string, object> interpolatedVaryingList = new Dictionary<string, object>();
        protected Dictionary<string, string> varyingTypeList = new Dictionary<string, string>();

        protected Dictionary<string, object> varList = new Dictionary<string, object>();
        protected Dictionary<string, string> typeList = new Dictionary<string, string>();
        protected List<string> vsTypeList = new List<string>();
        protected List<string> fsTypeList = new List<string>();

        protected Dictionary<string, object[]> attributeList = new Dictionary<string, object[]>();
        protected Dictionary<string, string> attributeTypeList = new Dictionary<string, string>();

        private bool uniformChanged = false;
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
            DefineVariable("arPosition", "vec4f", new float[4] { 0, 0, 0, 0 });
            DefineVariable("arFragColor", "vec4f", new float[4] { 0, 0, 0, 0 });
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
        protected void DefineAttributeVariable(string varName, string typeName)
        {
            attributeList[varName] = null;
            attributeTypeList[varName] = typeName;
        }
        protected void DefineVaryingVariable(string varName, string typeName)
        {
            varyingList[varName] = new object[3];
            varyingTypeList[varName] = typeName;
        }
        protected object GetVaryingVariableByVertex(string varName, int idx)
        {
            return varyingList[varName][idx];
        }
        protected object GetVaryingVariable(string varName)
        {
            return interpolatedVaryingList[varName];
        }
        protected void SetVaryingVariable(string varName, object value)
        {
            int idx = activeVIndex;
            if (CheckVariable(varyingTypeList[varName], value))
            {
                varyingList[varName][idx] = value;
            }
            else
            {
                throw new ArvnShaderException("Incorrect type.");
            }
        }
        protected void DefineVertexShaderInput(params string[] typeName)
        {
            vsTypeList.Clear();
            foreach (string i in typeName)
            {
                vsTypeList.Add(i);
            }
        }
        protected void DefineFragmentShaderInput(params string[] typeName)
        {
            fsTypeList.Clear();
            foreach (string i in typeName)
            {
                fsTypeList.Add(i);
            }
        }
        protected void DefineVariable(string varName, string typeName, object value)
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
            if (CheckVariable(typeList[varName], value))
            {
                varList[varName] = value;
            }
            else
            {
                throw new ArvnShaderException("Incorrect type.");
            }
        }
        private void InterpolateVaryings(string varName, float[] bc)
        {
            string tp = varyingTypeList[varName];
            object a = GetVaryingVariableByVertex(varName, 0);
            object b = GetVaryingVariableByVertex(varName, 1);
            object c = GetVaryingVariableByVertex(varName, 2);
            float ta = bc[0];
            float tb = bc[1];
            float tc = bc[2];
            if (tp == "int")
            {
                interpolatedVaryingList[varName] = ta * (int)a + tb * (int)b + tc * (int)c;
            }
            else if (tp == "float")
            {
                interpolatedVaryingList[varName] = ta * (float)a + tb * (float)b + tc * (float)c;
            }
            else if (tp == "vec2f")
            {
                float dx = ta * ((float[])a)[0] + tb * ((float[])b)[0] + tc * ((float[])c)[0];
                float dy = ta * ((float[])a)[1] + tb * ((float[])b)[1] + tc * ((float[])c)[1];
                interpolatedVaryingList[varName] = new float[] { dx, dy };
            }
            else if (tp == "vec3f")
            {
                float dx = ta * ((float[])a)[0] + tb * ((float[])b)[0] + tc * ((float[])c)[0];
                float dy = ta * ((float[])a)[1] + tb * ((float[])b)[1] + tc * ((float[])c)[1];
                float dz = ta * ((float[])a)[2] + tb * ((float[])b)[2] + tc * ((float[])c)[2];
                interpolatedVaryingList[varName] = new float[] { dx, dy, dz };
            }
            else if (tp == "vec4f")
            {
                float dx = ta * ((float[])a)[0] + tb * ((float[])b)[0] + tc * ((float[])c)[0];
                float dy = ta * ((float[])a)[1] + tb * ((float[])b)[1] + tc * ((float[])c)[1];
                float dz = ta * ((float[])a)[2] + tb * ((float[])b)[2] + tc * ((float[])c)[2];
                float dw = ta * ((float[])a)[3] + tb * ((float[])b)[3] + tc * ((float[])c)[3];
                interpolatedVaryingList[varName] = new float[] { dx, dy, dz, dw };
            }
            else
            {
                throw new ArvnShaderException("Unsupported type");
            }
        }
        private bool CheckVariable(string typeName, object value)
        {
            //Base
            if (typeName == "int")
            {
                if (value is int)
                {
                    return true;
                }
            }
            if (typeName == "float")
            {
                if (value is float)
                {
                    return true;
                }
            }

            //Vectors
            if (typeName == "vec4f")
            {
                if (value is float[] && ((float[])value).Length == 4)
                {
                    return true;
                }
            }
            if (typeName == "vec3f")
            {
                if (value is float[] && ((float[])value).Length == 3)
                {
                    return true;
                }
            }
            if (typeName == "vec2f")
            {
                if (value is float[] && ((float[])value).Length == 2)
                {
                    return true;
                }
            }
            //Matrix
            if (typeName == "mat3f")
            {
                if (value is float[,] && ((float[,])value).GetLength(0) == 3 && ((float[,])value).GetLength(1) == 3)
                {
                    return true;
                }
            }
            if (typeName == "mat4f")
            {
                if (value is float[,] && ((float[,])value).GetLength(0) == 4 && ((float[,])value).GetLength(1) == 4)
                {
                    return true;
                }
            }
            if (typeName == "mat2f")
            {
                if (value is float[,] && ((float[,])value).GetLength(0) == 2 && ((float[,])value).GetLength(1) == 2)
                {
                    return true;
                }
            }
            //Texture
            if (typeName == "sampler2d")
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
            foreach (string i in varyingList.Keys)
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
