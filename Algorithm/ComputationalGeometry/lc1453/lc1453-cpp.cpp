class Solution {
public:
    void getCenter(int x1, int y1, int x2, int y2, int r, double& rx, double& ry,int p) {
        double cx = ((double)x1 + (double)x2) / 2.0;
        double cy = ((double)y1 + (double)y2) / 2.0;
        double f = ((double)y2 - (double)y1) * ((double)y2 - (double)y1) + ((double)x2 - (double)x1) * ((double)x2 - (double)x1);
        f = f / 4.0;
        double d = sqrt(r * r - f);
        int dx = x2 - x1;
        int dy = y2 - y1;
        double dsx = (double)dx / sqrt(f * 4.0);
        double dsy = (double)dy / sqrt(f * 4.0);
        if (p) {
            rx = cx + dsy * d;
            ry = cy - dsx * d;
        }
        else {
            rx = cx + dsy * d;
            ry = cy - dsx * d;
        }

    }
    int numPoints(vector<vector<int>>& points, int r) {
        int sz = points.size();
        int ans = -1;
        double dx, dy;
        int stacks=0;
        for (int i = 0; i < sz; i++) {
            for (int j = 0; j < sz; j++) {
                int t=0;
                if (points[i][0] == points[j][0] && points[i][1] == points[j][1]) {
                    t++;
                }
                if (t > ans) {
                    ans = t;
                }
            }
        }
        for (int i = 0; i < sz; i++) {
            for (int j = 0; j < sz; j++) {
                if (points[i][0] == points[j][0] && points[i][1] == points[j][1]) {
                    continue;
                }
                if((points[i][0]-points[j][0])*(points[i][0]-points[j][0])+
                (points[i][1]-points[j][1])*(points[i][1]-points[j][1])>r*r*4
                ){
                    continue;
                }
                
                int t = 0;
                getCenter(points[i][0], points[i][1], points[j][0], points[j][1], r, dx, dy, 0);
                for (int p = 0; p < sz; p++) {
                    double px = ((double)points[p][0] - dx);
                    double py = ((double)points[p][1] - dy);
                    double dist = px * px + py * py;
                    if ((double)((double)r * (double)r)-dist>=-(1e-10)) {
                        t++;
                    }
                }
                if (t > ans) {
                    ans = t;
                }
                
            }
        }
        if (ans == -1) {
            ans = sz;
        }
        return ans;
    }
};