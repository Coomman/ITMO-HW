#include <fstream>
#include <vector>
#include <algorithm>
#include <stack>

using namespace std;

#define big long long

int v, e;

struct Edge {
	int from, to;
	int w;

	Edge(int t_f, int t_t, int t_w) :
		from(t_f), to(t_t), w(t_w) {
	}
};

int countVer;
vector<bool> vis(v);

void dfs(vector<Edge> &list, int n) {
	countVer++;
	vis[n] = true;
	for (Edge i : list) {
		if (i.from == n && !vis[i.to]) {
			dfs(list, i.to);
		}
	}
}


stack<int> order;
void cond_revfs(int n, vector<Edge> &zeroEdges) {
	vis[n] = true;
	for (Edge i : zeroEdges) {
		if (i.to == n && !vis[i.from]) {
			cond_revfs(i.from, zeroEdges);
		}
	}
	order.push(n);
}

void cond_dfs(int n, vector<Edge> &zeroEdges, vector<int> &compNum, int num) {
	compNum[n] = num;
	vis[n] = true;
	for (Edge i : zeroEdges) {
		if (i.from == n && !vis[i.to]) {
			cond_dfs(i.to, zeroEdges, compNum, num);
		}
	}
}

int condensation(vector<int> &compNum, vector<Edge> &zeroEdges, int root, int size) {
	vis.clear();
	vis.resize(size);
	for (int i = 0; i < size; i++) {
		if (!vis[i]) {
			cond_revfs(i, zeroEdges);
		}
	}
	vis.clear();
	vis.resize(size);

	int comp_num = 0;
	while (!order.empty()) {
		if (!vis[order.top()]) {
			cond_dfs(order.top(), zeroEdges, compNum, comp_num++);
		}
		order.pop();
	}

	return comp_num;
}

big findMST(vector<Edge> &list, int compCount, int root) {
	big MST = 0;
	vector<int> minEdge(compCount, INT_MAX);
	for (Edge e : list) {
		minEdge[e.to] = min(e.w, minEdge[e.to]);
	}

	for (int i = 0; i < compCount; i++) {
		if (i != root) {
			MST += minEdge[i];
		}
	}

	vector<Edge> zeroEdges;
	for (Edge e : list) {
		if (e.w == minEdge[e.to]) {
			zeroEdges.push_back(Edge(e.from, e.to, 0));
		}
	}

	countVer = 0;
	vis.clear();
	vis.resize(compCount);
	dfs(zeroEdges, root);
	if (countVer == compCount) {
		return MST;
	}

	//if Zero-MST doesn't exist
	vector<int> compNum(compCount);
	int new_comps_count = condensation(compNum, zeroEdges, root, compCount);

	vector<Edge> newEdges;
	for (Edge e : list) {
		if (compNum[e.from] != compNum[e.to]) {
			newEdges.push_back(Edge(compNum[e.from],compNum[e.to], e.w - minEdge[e.to]));		
		}
	}

	MST += findMST(newEdges, new_comps_count, compNum[root]);
	return MST;
}

int main() {
	ifstream in("chinese.in");
	ofstream out("chinese.out");

	in >> v >> e; vis.resize(v);
	vector<Edge> list;
	for (int i = 0; i < e; i++) {
		int a, b, w; in >> a >> b >> w;		
		list.push_back(Edge(--a, --b, w));
	}

	dfs(list, 0);
	if (countVer == v) {
		out << "YES" << endl << findMST(list, v, 0);
	}
	else {
		out << "NO";
	}
}