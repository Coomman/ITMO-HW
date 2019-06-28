#include <iostream>
#include <fstream>
#include <queue>
#include <stack>
#include <vector>

using namespace std;

enum coord {U, L, R, D};

struct Vertex {
	int x, y;
	int num;
	int* paths;
	int from;
	int side;
	int dist = -1;

	Vertex(int tx, int ty, int tnum) {
		x = tx;
		y = ty;
		num = tnum;
	}
};

vector<Vertex*> list(1);

int main() {
	ifstream in("input.txt");
	ofstream out("output.txt");
	
	int n, m; in >> n >> m;
	vector<vector<int>> lab(n, vector<int>(m));
	Vertex* START;
	Vertex* FIN;
	int count = 0;
	char temp;

	//Make lab
	for (int i = 0; i < n; i++) {
		for (int j = 0; j < m; j++) {
			in >> temp;
			if (temp != '#') {
				Vertex* ver = new Vertex(i, j, ++count);
				list.push_back(ver);
				if (temp == 'S') {
					START = ver;
				}
				if (temp == 'T') {
					FIN = ver;
				}
				lab[i][j] = count;
			}
		}
	}

	//Check paths
	for (int i = 1; i < (int)list.size(); i++) {
		int x = list[i]->x;
		int y = list[i]->y;
		int* paths = new int[4]{};

		if (x) {
			if (lab[x - 1][y]) {
				paths[U] = lab[x - 1][y];
			}
		}

		if (y) {
			if (lab[x][y - 1]) {
				paths[L] = lab[x][y - 1];
			}
		}

		if (x != n - 1) {
			if (lab[x + 1][y]) {
				paths[D] = lab[x + 1][y];
			}
		}

		if (y != m - 1) {
			if (lab[x][y + 1]) {
				paths[R] = lab[x][y + 1];
			}
		}

		list[i]->paths = paths;
	}

	START->dist = 0;
	START->from = 0;
	START->side = -1;

	bool flag = false;
	queue<int> bfs;
	bfs.push(START->num);
	while (!bfs.empty()) {
		int u = bfs.front();//num of Vertex
		bfs.pop();
		for (int i = U; i <= D; i++) {
			if (list[u]->paths[i]) {
				if (list[list[u]->paths[i]]->dist == -1) {
					list[list[u]->paths[i]]->dist = list[u]->dist + 1;
					list[list[u]->paths[i]]->from = list[u]->num;
					list[list[u]->paths[i]]->side = i;
					if (list[list[u]->paths[i]]->num == FIN->num) {
						flag = true;
						break;
					}
					else {
						bfs.push(list[list[u]->paths[i]]->num);
					}
				}
			}
		}
		if (flag) {
			break;
		}
	}

	if (flag) {
		stack<int> sides;
		int i = FIN->side;
		int j = FIN->from;
		while (j) {
			sides.push(i);
			i = list[j]->side;
			j = list[j]->from;
		}

		out << FIN->dist << endl;
		while (!sides.empty()) {
			switch (sides.top()) {
			case U:
				out << 'U';
				break;
			case L:
				out << 'L';
				break;
			case R:
				out << 'R';
				break;
			case D:
				out << 'D';
				break;
			}
			sides.pop();
		}
	}
	else {
		out << -1;
	}
}