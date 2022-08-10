using System;
using System.Collections.Generic;
using System.Text;
using TinyRenderer.Shaders;
using TinyRenderer.Core.Drawing;
using TinyRenderer.Core.Render;

namespace TinyRenderer.Legacy
{
    abstract class ArvnCompatibleShader : IArvnShaderCaller
    {
        class ArvnShaderException : ApplicationException
        {
            public ArvnShaderException(string message) : base(message) { }
        }

        protected Dictionary<string, object[]> varyingList = new Dictionary<string, object[]>();
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
        protected ArvnCompatibleShader()
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
        protected object[] GetAttributeVariable(string varName)
        {
            return attributeList[varName];
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
        protected object GetVaryingVariable(string varName, int idx)
        {
            return varyingList[varName][idx];
        }
        protected void SetVaryingVariable(string varName, int idx, object value)
        {
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
        abstract public void VertexShader(int index, int vindex, params object[] input);
        abstract public void FragmentShader(float[] barycenterCoord, params object[] input);
        abstract public void ComputeDerivedUniforms();
    }
}
