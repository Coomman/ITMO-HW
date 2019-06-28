#include <fstream>
#include <vector>
#include <set>

using namespace std;

struct Vertex {
	long long dist = INT64_MAX;
	vector<pair<long long, int>> list; // Edges
};

vector<Vertex> ver;

int main() {
	ifstream in("pathbgep.in");
	ofstream out("pathbgep.out");

	int v, e; in >> v >> e;
	ver.resize(v + 1);
	for (int i = 1; i <= e; i++) {
		int from, to, w; in >> from >> to >> w;
		ver[from].list.push_back(make_pair(w, to));
		ver[to].list.push_back(make_pair(w, from));
	}

	set<pair<long long, int>> stack;
	stack.insert(make_pair(0, 1));
	ver[1].dist = 0;
	
	while (!stack.empty()) {
		int minVer = (*stack.begin()).second;
		stack.erase(stack.begin());

		for (auto i = ver[minVer].list.begin(); i != ver[minVer].list.end(); i++) {
			int tempVer = (*i).second;
			long long tempW = (*i).first;
			if (ver[tempVer].dist > tempW + ver[minVer].dist) {
				if (ver[tempVer].dist != INT64_MAX) {
					stack.erase(stack.find(make_pair(ver[tempVer].dist, tempVer)));
				}
				ver[tempVer].dist = tempW + ver[minVer].dist;
				stack.insert(make_pair(ver[tempVer].dist, tempVer));
			}
		}
	}

	for (int i = 1; i <= v; i++) {
		out << ver[i].dist << ' ';
	}
}