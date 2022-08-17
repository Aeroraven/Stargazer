#pragma once

#include "../IrisCore.h"

namespace Iris {
    namespace Core {
        namespace Structure {

            struct IrisTrieNode
            {
                char key;
                IrisTrieNode** next;
                int value;
                bool valid;
                int valCnts;
            };

            class IrisTrie
            {
            private:
                IrisTrieNode* root;
            public:
                IrisTrie();
                void Insert(string x, int val);
                int Find(string x);
            };
            
        }
    }
}
