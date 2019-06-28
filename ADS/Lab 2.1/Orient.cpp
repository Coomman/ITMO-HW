#include <iostream>
#include <fstream>
#include <vector>

using namespace std;

int main() {
	ifstream in("input.txt");
	ofstream out("output.txt");

	int v; in >> v;
	vector<vector<int>> matrix(v, vector<int>(v));

	for (int i = 0; i < v; i++) {
		for (int j = 0; j < v; j++) {
			in >> matrix[i][j];
		}
	}

	bool flag = true;
	for (int i = 0; i < v; i++) {
		for (int j = 0; j <= i; j++) {
			if (i == j) {
				if (matrix[i][i]) {
					flag = false;
					break;
				}
			}
			else {
				if (matrix[i][j] != matrix[j][i]) {
					flag = false;
					break;
				}
			}
		}
		if (!flag) {
			break;
		}
	}

	if (flag) {
		out << "YES";
	}
	else {
		out << "NO";
	}
}