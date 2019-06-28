#include <fstream>
#include <vector>
#include <algorithm>
#include <queue>

using namespace std;

int V1, V2, E, V;

struct Edge {
	int from, to;
	int cf;
	Edge* rev;

	Edge(int t_from, int t_to, int t_cf) {
		from = t_from;
		to = t_to;
		cf = t_cf;
	}
};

struct Vertex {
	vector<Edge*> list;
};

vector<Vertex> ver;

Edge* bfs(vector<Edge*> &bfs_res) {
	bfs_res.clear();
	bfs_res.resize(V + 1);

	queue<int> q;
	q.push(1);

	while (!q.empty()) {
		int cur = q.front();
		q.pop();

		for (Edge* e : ver[cur].list) {
			if (!bfs_res[e->to] && e->cf && e->to != 1) {
				q.push(e->to);
				bfs_res[e->to] = e;

				if (e->to == V) {
					break;
				}
			}
		}
	}

	return bfs_res[V];
}

void EdmondsKarp() {
	vector<Edge*> bfs_res;
	while (bfs(bfs_res)) {
		vector<Edge*> order;
		Edge* cur = bfs_res[V];

		int minCf = INT_MAX;
		while (cur) {
			minCf = min(cur->cf, minCf);
			order.push_back(cur);
			cur = bfs_res[cur->from];
		}

		for (Edge* e : order) {
			e->cf -= minCf;
			e->rev->cf += minCf;
		}
	}
}

int main() {
	ifstream in("matching.in");
	ofstream out("matching.out");

	in >> V1 >> V2 >> E;
	V = V1 + V2 + 2;
	ver.resize(V + 1);

	for (int i = 0; i < E; i++) {
		int from, to; in >> from >> to;
		to += V1 + 1; from++;
		Edge* e1 = new Edge(from, to, 1);
		Edge* e2 = new Edge(to, from, 0);
		e1->rev = e2; e2->rev = e1;
		ver[from].list.push_back(e1);
		ver[to].list.push_back(e2);
	}

	for (int i = 2; i <= V1 + 1; i++) {
		Edge* e1 = new Edge(1, i, 1);
		Edge* e2 = new Edge(i, 1, 0);
		e1->rev = e2; e2->rev = e1;
		ver[1].list.push_back(e1);
		ver[i].list.push_back(e2);
	}

	for (int i = V1 + 2; i < V; i++) {
		Edge* e1 = new Edge(i, V, 1);
		Edge* e2 = new Edge(V, i, 0);
		e1->rev = e2; e2->rev = e1;
		ver[i].list.push_back(e1);
		ver[V].list.push_back(e2);
	}

	EdmondsKarp();

	long long maxFlow = 0;
	for (Edge* e : ver[V].list) {
		maxFlow += e->cf;
	}

	out << maxFlow;
}