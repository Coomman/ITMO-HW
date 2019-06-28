#include <iostream>
#include <fstream>
#include <set>
#include <vector>

using namespace std;

vector<set<int>> list;
vector<bool> vis;
vector<int> compNumber;

void dfs(int v, int comp) {
	vis[v] = true;
	compNumber[v] = comp;
	for (set<int>::iterator i = list[v].begin(); i != list[v].end(); i++) {
		if (!vis[*i]) {
			dfs(*i, comp);
		}
	}
}

int main() {
	ifstream in("components.in");
	ofstream out("components.out");

	int v, e; in >> v >> e;
	list.resize(v + 1);
	vis.resize(v + 1);
	compNumber.resize(v + 1);

	int a, b;
	for (int i = 0; i < e; i++) {
		in >> a; a;
		in >> b; b;
		list[a].insert(b);
		list[b].insert(a);
	}

	int compNum = 0;
	for (int i = 1; i <= v; i++) {
		if (!vis[i]) {
			dfs(i, ++compNum);
		}
	}

	out << compNum << endl;
	for (int i = 1; i <= v; i++) {
		out << compNumber[i];
		if (i != v) {
			out << ' ';
		}
	}
}