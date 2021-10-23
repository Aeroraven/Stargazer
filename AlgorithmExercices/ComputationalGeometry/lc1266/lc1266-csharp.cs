public class Solution {
    public int getAbs(int x){
        return x>0?x:-x;
    }
    public int getMax(int x,int y){
        return x>y?x:y;
    }
    public int getMin(int x,int y){
        return x>y?y:x;
    }
    public int MinTimeToVisitAllPoints(int[][] points) {
        int dist = 0;
        for(int i=0;i<points.Length-1;i++){
            int deltax = getAbs(points[i][0]-points[i+1][0]);
            int deltay = getAbs(points[i][1]-points[i+1][1]);
            dist += getMax(deltax,deltay);
        }
        return dist;
    }
}