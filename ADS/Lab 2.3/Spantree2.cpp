#include <fstream>
#include <vector>

using namespace std;

ifstream in("spantree2.in");
ofstream out("spantree2.out");

struct Edge {
	int to; 
	int w;

	Edge(int t_to, int t_w) {
		to = t_to;
		w = t_w;
	}
};

struct Vertex {
	int dist = INT_MAX;
	bool inTree = false;
	vector<Edge> list;
};

vector<Vertex> ver;

int countVertex = 1;
long long MST;

int main() {
	int v, e; in >> v >> e;
	ver.resize(v + 1);

	for (int i = 0; i < e; i++) {
		int from, to, w; in >> from >> to >> w;
		ver[from].list.push_back(Edge(to, w));
		ver[to].list.push_back(Edge(from, w));
	}

	ver[1].dist = 0;
	ver[1].inTree = true;
	for (auto i = ver[1].list.begin(); i != ver[1].list.end(); i++) {
		ver[i->to].dist = i->w;
	}

	while (countVertex < v) {
		int minVertex;
		int minWeight = INT_MAX;
		for (int i = 2; i <= v; i++) {
			if (ver[i].dist < minWeight && !ver[i].inTree) {
				minVertex = i;
				minWeight = ver[i].dist;
			}
		}

		ver[minVertex].inTree = true;
		countVertex++;
		for (auto i = ver[minVertex].list.begin(); i != ver[minVertex].list.end(); i++) {
			if (ver[i->to].dist > i->w && !ver[i->to].inTree) {
				ver[i->to].dist = i->w;
			}
		}
	}

	for (int i = 2; i <= v; i++) {
		MST += ver[i].dist;
	}

	out << MST;
}