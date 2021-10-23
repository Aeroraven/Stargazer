public class Solution {
    public long VectorProduct(long x1, long y1, long x2, long y2)
    {
        return x1 * y2 - x2 * y1;
    }
    public long SameLine(int[] a,int[] b,int[] c)
    {
        long vecx1 = b[0] - a[0];
        long vecy1 = b[1] - a[1];
        long vecx2 = c[0] - a[0];
        long vecy2 = c[1] - a[1];
        return VectorProduct(vecx1, vecy1, vecx2, vecy2);
    }
    public int MaxPoints(int[][] points)
    {
        int ans = 0;
        if (points.Length <= 2)
        {
            return points.Length;
        }

        for(int i = 0; i < points.Length; i++)
        {
            for(int j = 0; j < points.Length; j++)
            {
                if (i == j)
                {
                    continue;
                }
                int t = 2;
                for(int k = 0; k < points.Length; k++)
                {
                    if (k == i || k == j)
                    {
                        continue;
                    }
                    if (SameLine(points[i], points[j], points[k]) == 0)
                    {
                        t++;
                    }
                }
                if (t > ans)
                {
                    ans = t;
                }
            }
        }
        return ans;
    }
}