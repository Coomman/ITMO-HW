#include <fstream>
#include <vector>

using namespace std;

int main() {
	ifstream in("pathsg.in");
	ofstream out("pathsg.out");

	int v, e; in >> v >> e;
	vector<vector<int>> adjMat(v, vector<int>(v, -1));

	for (int j = 0; j < e; j++) {
		int from, to; in >> from >> to;
		in >> adjMat[--from][--to];
	}

	for (int k = 0; k < v; k++) {
		vector<int> dist(v, INT_MAX);
		vector<bool> inPath(v);
		dist[k] = 0;

		for (int i = 0; i < v; i++) {
			int minVer = -1;
			for (int j = 0; j < v; j++) {
				if (!inPath[j] && (minVer == -1 || dist[j] < dist[minVer])) {
					minVer = j;
				}
			}

			if (dist[minVer] == INT_MAX) {
				break;
			}

			inPath[minVer] = true;
			for (int e = 0; e < v; e++) {
				if (minVer != e && adjMat[minVer][e] != -1 && dist[e] > dist[minVer] + adjMat[minVer][e]) {
					dist[e] = dist[minVer] + adjMat[minVer][e];
				}
			}
		}

		for (int i = 0; i < v; i++) {
			out << dist[i];
			if (i != v - 1) {
				out << ' ';
			}
		}
		out << endl;
	}
}