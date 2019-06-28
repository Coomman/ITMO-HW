#include <fstream>
#include <vector>
#include <set>
#include <stack>

using namespace std;

ifstream in("cycle.in");
ofstream out("cycle.out");

enum colors { WHITE, GRAY, BLACK };

struct Vertex {
	int color = WHITE;
	set<int> list;
};

vector<Vertex> ver;
stack<int> current;
stack<int> reverses;

void dfs(int n) {
	ver[n].color = GRAY;
	current.push(n);
	for (set<int>::iterator i = ver[n].list.begin(); i != ver[n].list.end(); i++) {
		if (!ver[*i].color) {
			dfs(*i);
		}
		else if (ver[*i].color == GRAY) {// cycle
			out << "YES" << endl;
			while (current.top() != *i) {
				reverses.push(current.top());
				current.pop();
			}
			reverses.push(current.top());

			while (!reverses.empty()) {
				out << reverses.top() << ' ';
				reverses.pop();
			}
			exit(0);
		}
	}
	ver[n].color = BLACK;
	current.pop();
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

	out << "NO";
}