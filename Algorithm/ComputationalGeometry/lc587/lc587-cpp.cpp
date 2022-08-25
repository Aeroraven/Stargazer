class Solution {
public:
	
	class point {
	public:
		static int standard_x, standard_y;
		int x, y;
		int gdist(const point &p) {
			return ((p.x - p.standard_x)*(p.x - p.standard_x) + (p.y - p.standard_y)*(p.y - p.standard_y));
		}
		double cosSquare(const point& p) {
			double dist = (double)((p.x - p.standard_x)*(p.x - p.standard_x) + (p.y - p.standard_y)*(p.y - p.standard_y));
			if (dist < 1e-9)return 1000000000.0;
			double ret= (double)(p.x - p.standard_x)*(double)(p.x - p.standard_x) / dist;
			if ((p.x - p.standard_x) < 0)ret = -ret;
			return ret;
		}
		bool operator<(const point& p) {
			int x1, x2, y1, y2;
			x1 = p.x - p.standard_x;
			y1 = p.y - p.standard_y;
			x2 = this->x - p.standard_x;
			y2 = this->y - p.standard_y;
			if (x1*y2 - x2 * y1 == 0) {
				return gdist(*this) < gdist(p);
			}

			return cosSquare(*this) > cosSquare(p);
		}
		
	};
	
	vector<vector<int>> outerTrees(vector<vector<int>>& points) {
		int psz = points.size();
		int m1 = 101, m2 = 101, mid;
		point *pt = new point[psz];
		bool *ptvis = new bool[psz];
		for (int i = 0; i < psz; i++) {
			pt[i].x = points[i][0];
			pt[i].y = points[i][1];
			ptvis[i] = false;
			if (pt[i].y < m2) {
				m1 = pt[i].x;
				m2 = pt[i].y;
				mid = i;
			}
			else if (pt[i].y == m2 && pt[i].x < m1) {
				m1 = pt[i].x;
				m2 = pt[i].y;
				mid = i;
			}
		}
		point stmp = pt[0];
		pt[0] = pt[mid]; pt[mid] = stmp;
		pt[0].standard_x = pt[0].x;
		pt[0].standard_y = pt[0].y;
		sort(pt + 1, pt + psz);
		int *st = new int[psz];

		//for (int i = 0; i < psz; i++) {
		//	cout << "SRT:" << pt[i].x << "," << pt[i].y << endl;
		//}

		int sTop = 0,last;
		int x1, y1, x2, y2, cr;
		st[0] = 0;
		ptvis[0] = true;
		for (int i = 1; i < psz; i++) {
			while (sTop) {
				last = sTop - 1;
				x2 = pt[i].x - pt[st[sTop]].x;
				y2 = pt[i].y - pt[st[sTop]].y;
				x1 = pt[st[sTop]].x - pt[st[last]].x;
				y1 = pt[st[sTop]].y - pt[st[last]].y;
				if (x1*y2 - x2 * y1 <= 0) {
					sTop--;
				}
				else {
					break;
				}
			}
			st[++sTop] = i;
		}
		vector<vector<int>> ret;
		for (int i = 0; i <= sTop; i++) {
			ret.push_back({ pt[st[i]].x,pt[st[i]].y });
			ptvis[st[i]] = true;
			//cout << "ORG:" << pt[st[i]].x << "," << pt[st[i]].y << endl;
		}
		//Line Check
		if (sTop >= 1) {
			int csTop = 0;
			for (int i = 0; i < psz; i++) {
				if (ptvis[i])continue;
				for (int j = 0; j <= sTop; j++) {
					x1 = pt[i].x - pt[st[j]].x;
					y1 = pt[i].y - pt[st[j]].y;
					x2 = pt[st[(j + 1) % (sTop + 1)]].x - pt[st[j]].x;
					y2 = pt[st[(j + 1) % (sTop + 1)]].y - pt[st[j]].y;
					if (x1*y2 - x2 * y1 == 0) {
						ret.push_back({ pt[i].x,pt[i].y });
						break;
					}
				}
			}
		}


		delete[]pt;
		delete[]st;
		delete[]ptvis;

		return ret;
	}
};
int Solution::point::standard_x = 0;
int Solution::point::standard_y = 0;
