#include <iostream>
#include <fstream>
#include <vector>
using namespace std;

int main() {
	ifstream in("input.txt");
	ofstream out("output.txt");

	int v, e; in >> v >> e;
	vector<vector<int>> matrix(v, vector<int>(v));
	int a, b;
	bool flag = true;
	for (int i = 0; i < e; i++) {
		in >> a >> b;
		a--;
		b--;

		if (!matrix[a][b]) {
			matrix[a][b] = 1;
			matrix[b][a] = 1;
		}
		else {
			flag = false;
			break;
		}
	}

	if (flag) {
		out << "NO";
	}
	else {
		out << "YES";
	}
}