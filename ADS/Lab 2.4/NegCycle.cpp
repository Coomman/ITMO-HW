#include <fstream>
#include <vector>
#include <stack>

using namespace std;

struct Edge {
	int from, to;
	int w;
	
	Edge(int a, int b, int c) {
		from = a;
		to = b;
		w = c;
	}
};

int v;
vector<Edge> edges;
vector<int> dist;
vector<int> par;
stack<int> cycle;

bool BellmanFord() {
	int flag;

	for (int i = 0; i < v; i++) {
		flag = 0;
		for (Edge e : edges) {
			if (dist[e.to] > dist[e.from] + e.w) {
				dist[e.to] = dist[e.from] + e.w;
				par[e.to] = e.from;
				flag = e.to;
			}
		}

		if (!flag) {
			break;
		}
	}

	if (flag) { //negCycle
		int start = flag;
		for (int i = 0; i < v; i++) {
			start = par[start];
		}
		
		int cur = start;
		while (1) {
			cycle.push(cur);
			cur = par[cur];
			if (cur == start) {
				cycle.push(cur);
				break;
			}
		}

		return true;
	}
	else {
		return false;
	}
}

int main() {
	ifstream in("negcycle.in");
	ofstream out("negcycle.out");

	in >> v;
	dist.resize(v + 1, INT_MAX / 2);
	dist[1] = 0;
	par.resize(v + 1);

	for (int i = 1; i <= v; i++) {
		for (int j = 1; j <= v; j++) {
			int w; in >> w;
			if (w <= 10000) {
				edges.push_back(Edge(i, j, w));
				out << i << ' ' << j << ' ' << w << endl;
			}
		}
	}
	exit(0);

	if (BellmanFord()) {
		out << "YES" << endl << cycle.size() << endl;
		while (!cycle.empty()) {
			out << cycle.top() << ' ';
			cycle.pop();
		}
	}
	else {
		out << "NO";
	}
}