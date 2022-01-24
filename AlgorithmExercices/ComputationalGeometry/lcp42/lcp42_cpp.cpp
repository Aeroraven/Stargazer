#include <iostream>
#include <vector>
#include <cmath>
#include <algorithm>
using namespace std;
class Solution {
public:
    static bool srt(vector<int> a, vector<int> b) {
        if (a[0] < b[0])return true;
        if (a[0] == b[0] && a[1] < b[1])return true;
        return false;
    }
    double dist(long long x1, long long y1, long long x2, long long y2,long long trad,long long prad) {
        return sqrt((double)((x1 - x2) * (x1 - x2) + (y1 - y2) * (y1 - y2)));
    }
    int circleGame(vector<vector<int>>& toys, vector<vector<int>>& circles, int rad) {
        
        int ans = 0;
        int tl = toys.size();
        int cl = circles.size();
        int pivot = 0;
        sort(circles.begin(), circles.begin() + cl, srt);
        vector<vector<int>*> idx_list;
        vector<int> x_list;
        int curx = -1;
        int idxlen = 0;
        for (int i = 0; i < cl; i++) {
            if (circles[i][0] != curx) {
                idxlen++;
                x_list.push_back(circles[i][0]);
                idx_list.push_back(new vector<int>());
                curx = circles[i][0];
            }
            if (i == 0) {
                (*idx_list[idxlen - 1]).push_back(i);
                continue;
            }
            if (circles[i][0] == circles[i - 1][0] && circles[i][1] == circles[i - 1][1]) {

            }
            else {
                (*idx_list[idxlen - 1]).push_back(i);
            }
            
        }
        int l = 0, r = idxlen-1, mid;
        int lb = 0, rb = 0;
        for (int i = 0; i < tl; i++) {
            if (toys[i][2] > rad) {
                continue;
            }
            //Determine Left Bound
            l = 0;
            r = idxlen - 1;
            while (l < r) {
                mid = (l + r) >> 1;
                if (x_list[mid] + rad < toys[i][0] - toys[i][2]) {
                    l = mid + 1;
                }
                else if (x_list[mid] + rad > toys[i][0] - toys[i][2]) {
                    r = mid - 1;
                }
                else {
                    l = mid;
                    break;
                }
            }
            lb = l;
            l = 0;
            r = idxlen - 1;
            //Determine Right Bound
            while (l < r) {
                mid = (l + r) >> 1;
                if (x_list[mid] - rad< toys[i][0] + toys[i][2] ) {
                    l = mid + 1;
                }
                else if (x_list[mid] - rad> toys[i][0] + toys[i][2] ) {
                    r = mid - 1;
                }
                else {
                    r = mid;
                    break;
                }
            }
            rb = r;
            //Find the most close one
            int idx, best_idx = 0;
            int w = 0x7fffffff;
            int midr;
            int idxmid, idxmidr;
            bool flag = false;
            for (int j = lb; j <= rb; j++) {
                l = 0;
                r = (*idx_list[j]).size() - 1;
                while (l < r) {
                    mid = (l + r) / 2;
                    midr = (mid + r) / 2;
                    idxmid = (*idx_list[j])[mid];
                    idxmidr = (*idx_list[j])[midr];
                    if (mid == midr) {
                        break;
                    }
                    else if (abs(circles[idxmid][1] - toys[i][1]) < abs(circles[idxmidr][1] - toys[i][1])) {
                        r = midr - 1;
                    }
                    else {
                        l = mid + 1;
                    }
                }
                if (r < l) {
                    int t = l;
                    l = r;
                    r = t;
                }
                for (int k = l; k <= r; k++) {
                    if (k < 0) {
                        continue;
                    }
                    idx = (*idx_list[j])[k];
                    if (dist(circles[idx][0], circles[idx][1], toys[i][0], toys[i][1],0,0) + toys[i][2] <= rad) {
                        ans++;
                        flag = true;
                        break;
                    }
                }
                if (flag) {
                    break;
                }
                
            }
        }
        return ans;
    }
};
