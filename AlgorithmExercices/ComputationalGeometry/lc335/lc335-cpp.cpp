class Line {
public:
	Line(int _id,int _v,int _s,int _t):id(_id),s(_s),t(_t),v(_v){}
	int s, t;
	int v;
	int id;
	bool operator ==(const Line& p)const {
		return (s == p.s && t == p.t && v == p.v && id == p.id);
	}
};

class Solution {
public:
    bool isSelfCrossing(vector<int>& distance) {
        const int GOLEFT = 1, GORIGHT = 3;
        const int GOTOP = 0, GOBOTTOM = 2;
        vector<int>& val = distance;
        vector<int> x;
        int size = val.size();
        list<Line> lst[4];
        int cur_x = 0, cur_y = 0;
        for (int i = 0; i < size; i++) {
            if (i%4 == GOTOP) {
                lst[GOLEFT].push_back(Line(i, cur_x, cur_y, cur_y + val[i]));
                lst[GORIGHT].push_back(Line(i, cur_x, cur_y, cur_y + val[i]));
                vector<Line> removeList;
                for (list<Line>::iterator j = lst[GOBOTTOM].begin(); j != lst[GOBOTTOM].end(); j++) {
                    if ((*j).id == i - 1) continue;
                    else {
                        if ((*j).s < cur_x && (*j).t < cur_x) { removeList.push_back(*j); continue; }
                        else if ((*j).s > cur_x && (*j).t > cur_x) { removeList.push_back(*j); continue; }
                        else if ((*j).v > cur_y + val[i]) { removeList.push_back(*j); continue; }
                        else if ((*j).v < cur_y) { removeList.push_back(*j); continue; }
                        else return true;
                    }
                }
                for (int j = 0; j < removeList.size(); j++) {
                    lst[GOBOTTOM].remove(removeList[j]);
                }
                cur_y += val[i];
            }
            else if (i%4 == GOBOTTOM) {
                lst[GOLEFT].push_back(Line(i, cur_x, cur_y, cur_y - val[i]));
                lst[GORIGHT].push_back(Line(i, cur_x, cur_y, cur_y - val[i]));
                vector<Line> removeList;
                for (list<Line>::iterator j = lst[GOTOP].begin(); j != lst[GOTOP].end(); j++) {
                    if ((*j).id == i - 1) continue;
                    else {
                        if ((*j).s < cur_x && (*j).t < cur_x) { removeList.push_back(*j); continue; }
                        else if ((*j).s > cur_x && (*j).t > cur_x) { removeList.push_back(*j); continue; }
                        else if ((*j).v < cur_y - val[i]) { removeList.push_back(*j); continue; }
                        else if ((*j).v > cur_y) { removeList.push_back(*j); continue; }
                        else return true;
                    }
                }
                for (int j = 0; j < removeList.size(); j++) {
                    lst[GOTOP].remove(removeList[j]);
                }
                cur_y -= val[i];
            }
            else if (i%4 == GOLEFT) {
                lst[GOTOP].push_back(Line(i, cur_y, cur_x, cur_x - val[i]));
                lst[GOBOTTOM].push_back(Line(i, cur_y, cur_x, cur_x - val[i]));
                vector<Line> removeList;
                for (list<Line>::iterator j = lst[GORIGHT].begin(); j != lst[GORIGHT].end(); j++) {
                    if ((*j).id == i - 1) continue;
                    else {
                        if ((*j).s < cur_y && (*j).t < cur_y) { removeList.push_back(*j); continue; }
                        else if ((*j).s > cur_y && (*j).t > cur_y) { removeList.push_back(*j); continue; }
                        else if ((*j).v < cur_x - val[i]) { removeList.push_back(*j); continue; }
                        else if ((*j).v > cur_x) { removeList.push_back(*j); continue; }
                        else return true;
                    }
                }
                for (int j = 0; j < removeList.size(); j++) {
                    lst[GORIGHT].remove(removeList[j]);
                }
                cur_x -= val[i];
            }
            else if (i%4 == GORIGHT) {
                lst[GOTOP].push_back(Line(i, cur_y, cur_x, cur_x + val[i]));
                lst[GOBOTTOM].push_back(Line(i, cur_y, cur_x, cur_x + val[i]));
                vector<Line> removeList;
                for (list<Line>::iterator j = lst[GOLEFT].begin(); j != lst[GOLEFT].end(); j++) {
                    if ((*j).id == i - 1) continue;
                    else {
                        if ((*j).s < cur_y && (*j).t < cur_y) { removeList.push_back(*j); continue; }
                        else if ((*j).s > cur_y && (*j).t > cur_y) { removeList.push_back(*j); continue; }
                        else if ((*j).v > cur_x + val[i]) { removeList.push_back(*j); continue; }
                        else if ((*j).v < cur_x ) { removeList.push_back(*j); continue; }
                        else return true;
                    }
                }
                for (int j = 0; j < removeList.size(); j++) {
                    lst[GOLEFT].remove(removeList[j]);
                }
                cur_x += val[i];
            }
            
        }
        if (cur_x == 0 && cur_y == 0) {
            return true;
        }
        return false;
    }
};