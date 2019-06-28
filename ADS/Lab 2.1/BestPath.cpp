#include <iostream>
#include <fstream>
#include <queue>
#include <set>
#include <vector>

using namespace std;

int main() {
	ifstream in("pathbge1.in");
	ofstream out("pathbge1.out");

	int v, e; in >> v >> e;
	vector<set<int>> list(v + 1);
	vector<int> dist(v + 1, -1);

	int a, b;
	for (int i = 0; i < e; i++) {
		in >> a; a;
		in >> b; b;
		list[a].insert(b);
		list[b].insert(a);
	}
	
	dist[1] = 0;

	queue<int> bfs;
	bfs.push(1);
	while (!bfs.empty()) {
		int u = bfs.front();
		bfs.pop();
		for (set<int>::iterator i = list[u].begin(); i != list[u].end(); i++) {
			if (dist[*i] == -1) {
				dist[*i] = dist[u] + 1;
				bfs.push(*i);
			}
		}
	}

	for (int i = 1; i <= v; i++) {
		out << dist[i];
		if (i != v) {
			out << ' ';
		}
	}
}