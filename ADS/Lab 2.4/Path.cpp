#define _CRT_SECURE_NO_WARNINGS

#include <iostream>
#include <fstream>
#include <vector>

using namespace std;

struct Edge {
	int from, to;
	long long w;

	Edge(int a, int b, long long c) {
		from = a;
		to = b;
		w = c;
	}
};

struct Vertex {
	bool checked = false;
	long double dist = (long double)INT64_MAX;
	vector<Edge*> list;
};

int v, e, START;
vector<Vertex> ver;
vector<Edge*> edges;

void dfs(int n) {
	ver[n].checked = true;
	for (Edge* e : ver[n].list) {
		if (!ver[e->to].checked && ver[n].dist < INT64_MAX) {
			dfs(e->to);
		}
	}
}

void BellmanFord() {
	bool flag;

	for (int i = 0; i < v - 1; i++) {
		flag = false;
		for (Edge* e : edges) {
			if (ver[e->from].dist < INT64_MAX && ver[e->to].dist > ver[e->from].dist + e->w) {
				ver[e->to].dist = ver[e->from].dist + e->w;
				flag = true;
			}
		}

		if (!flag) {
			break;
		}
	}

	if (flag) {
		for (Edge* e : edges) {
			if (ver[e->to].dist > ver[e->from].dist + e->w && !ver[e->to].checked) {
				dfs(e->to);
			}
		}
	}
}

int main() {
	ifstream in("path.in");
	freopen("path.out", "w", stdout);

	in >> v >> e >> START;
	ver.resize(v + 1);
	ver[START].dist = 0;

	for (int i = 0; i < e; i++) {
		int a, b; long long c; in >> a >> b >> c;
		Edge* e = new Edge(a, b, c);
		ver[a].list.push_back(e);
		edges.push_back(e);
	}

	BellmanFord();

	for (int i = 1; i <= v; i++) {
		if (ver[i].dist == INT64_MAX) {
			cout << '*';
		}
		else {
			if (ver[i].checked) {
				cout << '-';
			}
			else {
				printf("%.0f", ver[i].dist);
			}
		}
		cout << endl;
	}
}