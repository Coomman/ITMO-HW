#include <fstream>
#include <vector>
#include <algorithm>
#include <queue>

using namespace std;

#define endl '\n';

int V, E;

struct Edge {
	int from, to;
	int cf;
	Edge* rev;

	Edge(int t_from, int t_to, int t_cf)
		:from(t_from), to(t_to), cf(t_cf) { }
};

struct Vertex {
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
			if (ver[e->to].dist == INT_MAX && e->cf) {
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
		if (ver[(*e)->to].dist == ver[cur].dist + 1) {
			int delta = dfs((*e)->to, min(minCf, (*e)->cf));
			if (delta) {
				(*e)->cf -= delta;
				(*e)->rev->cf += delta;
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

vector<Edge*> check;
bool circulation() {
	long long maxFlow = Dinica();
	for (Edge* e : check) {
		if (e->cf) {
			return false;
		}
	}

	return true;
}

vector<Edge*> answer;
vector<int> max_caps;
int main() {
	ifstream in("circulation.in");
	ofstream out("circulation.out");

	in >> V >> E;
	V += 2;
	ver.resize(V + 1);

	for (int i = 0; i < E; ++i) {
		int from, to, minCap, maxCap; in >> from >> to >> minCap >> maxCap;
		++from; ++to;

		Edge* g = new Edge(1, to, minCap);
		Edge* g1 = new Edge(to, 1, 0);
		g->rev = g1; g1->rev = g;
		ver[1].list.push_back(g);
		ver[to].list.push_back(g1);
		check.push_back(g);

		Edge* h = new Edge(from, to, maxCap - minCap);
		Edge* h1 = new Edge(to, from, 0);
		h->rev = h1; h1->rev = h;
		ver[from].list.push_back(h);
		ver[to].list.push_back(h1);
		answer.push_back(h);
		max_caps.push_back(maxCap);

		Edge* k = new Edge(from, V, minCap);
		Edge* k1 = new Edge(V, from, 0);
		k->rev = k1; k1->rev = k;
		ver[from].list.push_back(k);
		ver[V].list.push_back(k1);
	}

	if (circulation()) {
		out << "YES" << endl;
		for (size_t i = 0; i < answer.size(); ++i) {
			out << max_caps[i] - answer[i]->cf << endl;
		}
	}
	else {
		out << "NO";
	}
}