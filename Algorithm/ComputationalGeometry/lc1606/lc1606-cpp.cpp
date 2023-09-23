#include <iostream>
#include <algorithm>
#include <vector>
#include <deque>
#include <cmath>

class Solution {
public:
    int visiblePoints(std::vector<std::vector<int>>& points, int angle, std::vector<int>& location) {
        //Find points at the origin
        int initSize = points.size();
        points.erase(std::remove_if(points.begin(),points.end(),[&](auto x){
            return (x[0] == location[0] && x[1] == location[1]);
        }),points.end());

        auto calculateAngle = [=](std::vector<int>& x){
            int ax = x[0] - location[0];
            int ay = x[1] - location[1];
            double a1 = atan2(ay,ax)*180/M_PI;
            if(a1<0) a1+=360.0;
            return a1;
        };
        //Sort points
        std::sort(points.begin(),points.end(),[=](auto& x,auto& y){
            return calculateAngle(x)<calculateAngle(y);
        });
        //Iterate Points
        std::vector<double> angles = {};
        std::for_each(points.begin(),points.end(),[&](auto & x){
            angles.push_back(calculateAngle(x));
        });
        int head=0;
        int ans=0;
        int size = static_cast<int>(points.size());
        for(int i=0;i<size*2;i++){
            while(true){
                if(i==head)break;
                double angleRange = angles[i%size]-angles[head%size];
                if(angleRange<0){
                    angleRange+=360;
                }
                if(angleRange-angle>0){
                    head++;
                    continue;
                }
                break;
            }
            if(i-head+1>ans) ans = i-head+1;
            if(ans==size||head==size)break;
        }
        if(ans>size){
            ans=size;
        }
        return ans+(initSize-size);
    }
};

int main(){
    Solution a;
    std::vector<std::vector<int>> x = {{33,86},{99,81},{76,20},{12,7},{56,74},{25,41},{15,65},{54,9},{0,74},{82,31},{58,23},{52,60},{82,82},{60,31},{66,66},{52,66},{54,59},{48,41},{0,1},{45,83},{36,63},{32,40},{54,45},{56,50},{2,15},{81,45},{10,83},{54,25},{84,4},{14,76},{56,47},{59,8},{24,39},{77,18},{64,78},{98,29},{92,96}};
    std::vector<int> l = {82,82};
    auto f = a.visiblePoints(x,0,l);
    std::cout<<f<<std::endl;
    return 0;
}
