#include <iostream>
#include <vector>
#include <queue>

using namespace std;

int v, e, k, s;

struct Edge {
	int to;
	long long w;

	Edge(int b, long long c) {
		to = b;
		w = c;
	}
};

struct Vertex {
	vector<Edge> list;
};

vector<Vertex> ver;

int main() {
	cin >> v >> e >> k >> s;
	ver.resize(v + 1);
	for (int i = 0; i < e; i++) {
		int from, to;
		long long w; cin >> from >> to >> w;
		ver[from].list.push_back(Edge(to, w));
	}

	vector<vector<long long>> dist(k + 1, vector<long long>(v + 1, INT64_MAX));
	queue<int> bfs; bfs.push(s);
	dist[0][s] = 0;

	for (int step = 1; step <= k; step++) {
		queue<int> temp;
		while (!bfs.empty()) {
			int cur = bfs.front();
			bfs.pop();

			for (Edge e : ver[cur].list) {
				if (dist[step][e.to] > dist[step - 1][cur] + e.w) {
					dist[step][e.to] = dist[step - 1][cur] + e.w;
					temp.push(e.to);
				}
			}
		}
		bfs = temp;
	}

	for (int i = 1; i < v + 1; i++) {
		if (dist[k][i] < INT64_MAX) {
			cout << dist[k][i];
		}
		else {
			cout << -1;
		}
		cout << endl;
	}

	system("pause");
}