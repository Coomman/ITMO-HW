#include <fstream>
#include <vector>

using namespace std;

ifstream in("game.in");
ofstream out("game.out");

struct Vertex {
	vector<int> list;
};

int START;
bool goodWay = false;
int stepsToFin = INT_MAX;

vector<Vertex> ver;

void dfs(int n, int steps) {
	if (!ver[n].list.empty()) {
		if (steps < stepsToFin) {// ограничение на кол-во шагов вглубь
			for (int i : ver[n].list) {
				dfs(i, steps + 1);
				if (goodWay) {
					if (n == START) {
						out << "First player wins";
						exit(0);
					}

					if (steps % 2 == 0) {// if 1st player turn
						return;
					}
				}
			}
		}
	}
	else {
		goodWay = steps % 2 ? true : false;

		if (steps < stepsToFin) {	
			stepsToFin = steps;
		}
	}
}

int main() {
	int v, e; in >> v >> e >> START;
	ver.resize(v + 1);

	int a, b;
	for (int i = 0; i < e; i++) {
		in >> a >> b;
		ver[a].list.push_back(b);
	}

	dfs(START, 0);
	out << "Second player wins";
}