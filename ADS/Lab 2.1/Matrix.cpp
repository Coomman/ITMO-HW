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
	for (int i = 0; i < e; i++) {
		in >> a >> b;
		a--;
		b--;
		matrix[a][b] = 1;
	}

	for (int i = 0; i < v; i++) {
		for (int j = 0; j < v; j++) {
			out << matrix[i][j];
			if (j != v - 1) {
				out << ' ';
			}
		}
		if (i != v - 1) {
			out << endl;
		}
	}

}