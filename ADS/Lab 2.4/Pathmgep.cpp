#include <fstream>
#include <vector>

using namespace std;

int main() {
	ifstream in("pathmgep.in");
	ofstream out("pathmgep.out");

	int v, S, F; in >> v >> S >> F; S--; F--;
	vector<vector<long long>> adjMat(v, vector<long long>(v));

	for (int i = 0; i < v; i++) {
		for (int j = 0; j < v; j++) {
			in >> adjMat[i][j];
		}
	}

	vector<long long> dist(v, INT64_MAX);
	vector<bool> inPath(v);
	dist[S] = 0;

	for (int i = 0; i < v; i++) {
		int minVer = -1;
		for (int j = 0; j < v; j++) {
			if (!inPath[j] && (minVer == -1 || dist[j] < dist[minVer])) {
				minVer = j;
			}
		}

		if (dist[minVer] == INT64_MAX) {
			break;
		}

		inPath[minVer] = true;
		for (int e = 0; e < v; e++) {
			if (minVer != e && adjMat[minVer][e] != -1 && dist[e] > dist[minVer] + adjMat[minVer][e]) {
				dist[e] = dist[minVer] + adjMat[minVer][e];
			}
		}
	}

	if (dist[F] != INT64_MAX) {
		out << dist[F];
	}
	else {
		out << -1;
	}
}