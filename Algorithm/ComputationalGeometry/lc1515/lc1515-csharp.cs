public class Solution {
    public double GetMinDistSum(int[][] positions)
    {
        double cx = 0, cy = 0;
        for (int i = 0; i < positions.Length; i++)
        {
            cx += positions[i][0];
            cy += positions[i][1];
        }
        cx /= (double)positions.Length;
        cy /= (double)positions.Length;
        double dist = 0;
        for (int i = 0; i < positions.Length; i++)
        {
            dist += Math.Sqrt((cx - positions[i][0]) * (cx - positions[i][0]) + (cy - positions[i][1]) * (cy - positions[i][1]));
        }
        double bestDist = dist;
        double Ts = 20, Te = 1e-9, Cd = 0.992;
        int iter = 1;
        double T = Ts;
        while (T > Te)
        {
            double distK = 1e100;
            double px = 0;
            double py = 0;
            for (int k = 0; k < iter; k++)
            {
                double rnd = new Random().Next(1,65536)/65536.0*3.1415926*2;
                px = cx + Math.Cos(rnd)*T;
                py = cy + Math.Sin(rnd)*T;
                double distR = 0;
                for (int i = 0; i < positions.Length; i++)
                {
                    distR += Math.Sqrt((px - positions[i][0]) * (px - positions[i][0]) + (py - positions[i][1]) * (py - positions[i][1]));
                }
                if (distR < distK)
                {
                    distK = distR;
                }
            }
            if (distK < bestDist)
            {
                bestDist = distK;
            }
            if (distK < dist)
            {
                dist = distK;
                cx = px;
                cy = py;
            }
            T = T * Cd;
        }
        //Console.WriteLine(bestDist);
        return bestDist;
    }
}