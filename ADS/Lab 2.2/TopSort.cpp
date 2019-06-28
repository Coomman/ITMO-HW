#include <fstream>
#include <vector>
#include <set>
#include <stack>

using namespace std;

ifstream in("topsort.in");
ofstream out("topsort.out");

enum colors{WHITE, GRAY, BLACK};

struct Vertex {
	int color = WHITE;
	set<int> list;
};

vector<Vertex> ver;
stack<int> current;

void dfs(int n) {
	ver[n].color = GRAY;
	for (set<int>::iterator i = ver[n].list.begin(); i != ver[n].list.end(); i++) {
		if (!ver[*i].color) {
			dfs(*i);
		}
		else if (ver[*i].color == GRAY) {
			out << "-1";
			exit(0);
		}
	}
	ver[n].color = BLACK;
	current.push(n);
}

int main() {

	int v, e; in >> v >> e;
	ver.resize(v + 1);

	int a, b;
	for (int i = 1; i <= e; i++) {
		in >> a >> b;
		ver[a].list.insert(b);
	}

	for (int i = 1; i < (int)ver.size(); i++) {
		if (!ver[i].color) {
			dfs(i);
		}
	}

	while (!current.empty()) {
		out << current.top() << ' ';
		current.pop();
	}
}