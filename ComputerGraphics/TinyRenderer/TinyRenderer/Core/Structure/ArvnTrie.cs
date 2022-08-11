using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TinyRenderer.Core.Structure
{
    class ArvnTrie
    {
        struct ArvnTrieNode
        {
            public char key;
            public ArvnTrieNode[] next;
            public bool[] nextMask;
            public int value;
            public bool valid;
            public int valCnts;
        }
        ArvnTrieNode root;
        public ArvnTrie()
        {
            root = new ArvnTrieNode();
            root.key = ' ';
            root.next = new ArvnTrieNode[128];
            root.nextMask = new bool[128];
            for(int i = 0; i < 128; i++)
            {
                root.nextMask[i] = false;
            }
            root.value = 0;
            root.valid = true;
            root.valCnts = 0;
        }

        public void Insert(string x,int val)
        {
            ArvnTrieNode cur = root;
            for(int i = 0; i < x.Length; i++)
            {
                char d = x[i];
                if(cur.nextMask[d] == false)
                {
                    cur.next[d] = new ArvnTrieNode();
                    cur.next[d].key = d;
                    cur.next[d].next = new ArvnTrieNode[128];
                    cur.next[d].nextMask = new bool[128];
                    for (int i2 = 0; i2 < 128; i2++)
                    {
                        root.nextMask[i2] = false;
                    }
                    cur.next[d].value = val;
                    cur.next[d].valid = true;
                    cur.next[d].valCnts = 1;
                }
                cur.nextMask[d] = true;
                cur.value = val;
                cur.valCnts++;
                cur = cur.next[d];
            }
        }
        public int Find(string x)
        {
            unsafe
            {
                ArvnTrieNode cur = root;
                int l = x.Length;
                fixed(char* tp = x)
                {
                    for (int i = 0; i < l; i++)
                    {
                        cur = cur.next[tp[i]];
                        if (cur.valCnts == 1)
                        {
                            return cur.value;
                        }
                    }
                }
                return -1;
            }
            
        }
    }
}
