#include <fstream>
#include <vector>

using namespace std;

int main() {
	ifstream in("input.txt");
	ofstream out("output.txt");

	int v, e; in >> v >> e;
	vector<int> deg(v + 1);
	
	int a, b;
	for (int i = 0; i < e; i++) {
		in >> a >> b;
		deg[a]++;
		deg[b]++;
	}

	for (int i = 1; i < (int)deg.size(); i++) {
		out << deg[i] << ' ';
	}
}