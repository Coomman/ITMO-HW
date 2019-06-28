#include <fstream>
#include <set>
#include <vector>

using namespace std;

ifstream in("bipartite.in");
ofstream out("bipartite.out");

struct Vertex {
	bool vis = false;
	bool color;
	set<int> list;
};

vector<Vertex> ver;
vector<vector<int>> comps(1);

void dfs(int v, int comp, bool c) {
	ver[v].vis = true;
	ver[v].color = c;
	comps[comp].push_back(v);
	for (set<int>::iterator i = ver[v].list.begin(); i != ver[v].list.end(); i++) {
		if (!ver[*i].vis) {
			dfs(*i, comp, !c);
		}
		else {
			if (ver[v].color == ver[*i].color) {
				out << "NO";
				exit(0);
			}
		}
	}
}

int main() {
	int v, e; in >> v >> e;
	ver.resize(v + 1);

	int a, b;
	for (int i = 0; i < e; i++) {
		in >> a >> b;
		ver[a].list.insert(b);
		ver[b].list.insert(a);
	}

	int compNum = 0;
	for (int i = 1; i <= v; i++) {
		if (!ver[i].vis) {
			comps.resize(comps.size() + 1);
			dfs(i, ++compNum, 0);
		}
	}

	out << "YES";
}