#include <fstream>
#include <vector>
#include <algorithm>
#include <queue>

using namespace std;

#define endl '\n'

int V, E;

struct Edge {
	int from, to;
	int flow, cap;
	int num;
	Edge* rev;

	Edge(int t_from, int t_to, int t_flow, int t_cap, int t_num = 0)
		: from(t_from), to(t_to), flow(t_flow), cap(t_cap), num(t_num) { }
};

struct Vertex {
	bool vis;
	int dist;
	int offset;
	vector<Edge*> list;
};

vector<Vertex> ver;

bool bfs() {
	for (Vertex &v : ver) {
		v.dist = INT_MAX;
	}

	queue<int> q;
	q.push(1);
	ver[1].dist = 0;

	while (!q.empty()) {
		int cur = q.front();
		q.pop();

		for (Edge* e : ver[cur].list) {
			if (ver[e->to].dist == INT_MAX && e->flow < e->cap) {
				q.push(e->to);
				ver[e->to].dist = ver[cur].dist + 1;

				if (e->to == V) {
					return true;
				}
			}
		}
	}

	return false;
}

int dfs(int cur, int minCf) {
	if (cur == V || minCf == 0) {
		return minCf;
	}

	for (auto e = ver[cur].list.begin() + ver[cur].offset; e != ver[cur].list.end(); ++e) {
		if ((*e)->flow < (*e)->cap && ver[(*e)->to].dist == ver[cur].dist + 1) {
			int delta = dfs((*e)->to, min(minCf, (*e)->cap - (*e)->flow));
			if (delta) {
				(*e)->flow += delta;
				(*e)->rev->flow -= delta;
				return delta;
			}
		}
		++ver[cur].offset;
	}

	return 0;
}

long long Dinica() {
	long long maxFlow = 0;
	while (bfs()) {
		for (Vertex &v : ver) {
			v.offset = 0;
		}

		int curFlow = 0;
		do {
			curFlow = dfs(1, INT_MAX);
			maxFlow += curFlow;
		} while (curFlow);
	}

	return maxFlow;
}

int sDecomp(int s, vector<Edge*> &edges) {
	for (Vertex &v : ver) {
		v.vis = false;
	}

	edges.clear();

	int cur = s;
	while (!ver[cur].vis) {
		if (cur == V) {
			break;
		}

		Edge* edge = nullptr;
		for (Edge* e : ver[cur].list) {
			if (e->flow && e->num) {
				edge = e;
				break;
			}
		}

		if (!edge) {
			return 0;
		}

		ver[cur].vis = true;
		edges.push_back(edge);
		cur = edge->to;
	}

	if (ver[cur].vis) {
		while ((*edges.begin())->from != cur) {
			edges.erase(edges.begin());
		}
	}

	int minFlow = INT_MAX;
	for (Edge* e : edges) {
		minFlow = min(e->flow, minFlow);
	}

	for (Edge* e : edges) {
		e->flow -= minFlow;
	}
	return minFlow;
}

vector<vector<Edge*>> answer;
vector<long long> flow;
void fDecomp() {
	vector<Edge*> curPath;
	long long curFlow = sDecomp(1, curPath);
	while (curFlow) {
		flow.push_back(curFlow);
		answer.push_back(curPath);
		curFlow = sDecomp(1, curPath);
	}
}

int main() {
	ifstream in("decomposition.in");
	ofstream out("decomposition.out");

	in >> V >> E;
	ver.resize(V + 1);

	for (int i = 0; i < E; i++) {
		int from, to, cap; in >> from >> to >> cap;
		if (from != V && to != 1) {
			Edge* e1 = new Edge(from, to, 0, cap, i + 1);
			Edge* e2 = new Edge(to, from, cap, cap);
			e1->rev = e2; e2->rev = e1;
			ver[from].list.push_back(e1);
			ver[to].list.push_back(e2);
		}
	}

	Dinica();
	fDecomp();

	out << answer.size() << endl;
	for (size_t i = 0; i < answer.size(); i++) {
		out << flow[i] << ' ' << answer[i].size();
		for (Edge* e : answer[i]) {
			out << ' ' << e->num;
		}
		out << endl;
	}
}