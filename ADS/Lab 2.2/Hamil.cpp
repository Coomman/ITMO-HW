#include <fstream>
#include <vector>

using namespace std;

ifstream in("hamiltonian.in");
ofstream out("hamiltonian.out");

struct Vertex {
	bool vis = false;
	vector<int> list;
	int countSteps = 0;
};

vector<Vertex> ver;

int dfs(int n) {
	ver[n].vis = true;

	for (int i = 0; i < (int)ver[n].list.size(); i++) {
		int curSteps = ver[n].countSteps;
		if (!ver[ver[n].list[i]].vis) {
			ver[ver[n].list[i]].countSteps = dfs(ver[n].list[i]);
		}

		if (ver[ver[n].list[i]].countSteps > curSteps) {
			ver[n].countSteps = ver[ver[n].list[i]].countSteps;
		}
	}

	return ++ver[n].countSteps;
}

int main() {
	int v, e; in >> v >> e;
	ver.resize(v + 1);

	int a, b;
	for (int i = 0; i < e; i++) {
		in >> a >> b;
		ver[a].list.push_back(b);
	}

	for (int i = 1; i < (int)ver.size(); i++) {
		if (!ver[i].vis) {
			ver[i].countSteps = dfs(i);
			if (ver[i].countSteps == v) {
				out << "YES";
				exit(0);
			}
		}
	}

	out << "NO";
}