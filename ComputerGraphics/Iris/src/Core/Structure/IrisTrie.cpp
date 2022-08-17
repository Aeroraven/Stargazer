#include "../../../include/Core/Structure/IrisTrie.h"

using namespace Iris::Core::Structure;

IrisTrie::IrisTrie() {
    root = new IrisTrieNode();
    root->key = ' ';
    root->next = new IrisTrieNode*[128];
    for (int i = 0; i < 128; i++) {
        root->next[i] = nullptr;
    }
    root->value = 0;
    root->valid = true;
    root->valCnts = 0;
}

void IrisTrie::Insert(string x, int val) {
    IrisTrieNode* cur = root;
    for (int i = 0; i < x.size(); i++)
    {
        char d = x[i];
        if (cur->next[d] == nullptr)
        {
            cur->next[d] = new IrisTrieNode();
            cur->next[d]->key = d;
            cur->next[d]->next = new IrisTrieNode*[128];
            for (int j = 0; j < 128; j++) {
                cur->next[d]->next[j] = nullptr;
            }
            cur->next[d]->value = val;
            cur->next[d]->valid = true;
            cur->next[d]->valCnts = 1;
        }
        cur->value = val;
        cur->valCnts++;
        cur = cur->next[d];
    }
}

int IrisTrie::Find(string x) {
    IrisTrieNode* cur = root;
    for (int i = 0; i < x.size(); i++)
    {
        cur = cur->next[x[i]];
        if (cur->valCnts == 1)
        {
            return cur->value;
        }
    }
    return -1;
}