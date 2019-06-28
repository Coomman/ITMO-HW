#include <fstream>
#include <vector>
#include <set>
#include <stack>

using namespace std;

ifstream in("cond.in");
ofstream out("cond.out");

struct Vertex {
	int compNum;
	bool vis = false;
	set<int> list;
	set<int> from;
};

vector<Vertex> ver;
stack<int> order;

void revfs(int n) {
	ver[n].vis = true;
	for (int i : ver[n].from) {
		if (!ver[i].vis) {
			revfs(i);
		}
	}
	order.push(n);
}

void dfs(int n, int compNum) {
	ver[n].vis = true;
	ver[n].compNum = compNum;
	for (int i : ver[n].list) {
		if (!ver[i].vis) {
			dfs(i, compNum);
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
		ver[b].from.insert(a);
	}

	for (int i = 1; i < (int)ver.size(); i++) {
		if (!ver[i].vis) {
			revfs(i);
		}
	}

	for (int i = 1; i <= v; i++) {
		ver[i].vis = false;
	}

	int compNum = 0;
	while (!order.empty()) {
		if (!ver[order.top()].vis) {
			dfs(order.top(), ++compNum);
		}
		order.pop();
	}
	

	out << compNum++ << endl;
	for (int i = 1; i < (int)ver.size(); i++) {
		out << compNum - ver[i].compNum << ' ';
	}
}