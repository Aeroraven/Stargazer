class Solution {
public:
    double largestTriangleArea(std::vector<std::vector<int>>& points) {
        int ans = 0;
        for(const auto& x:points){
            for(const auto& y:points){
                for(const auto& z:points){
                    auto ax = y[0] - x[0];
                    auto ay = y[1] - x[1];
                    auto bx = z[0] - x[0];
                    auto by = z[1] - x[1];
                    ans = std::max(ans,abs(ax*by-ay*bx));
                }
            }
        }
        return static_cast<double>(ans);
    }
};
