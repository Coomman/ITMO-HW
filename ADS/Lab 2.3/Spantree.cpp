#include <fstream>
#include <vector>
#include <iomanip>
#include <list>
#include <cmath>

using namespace std;

ifstream in("spantree.in");
ofstream out("spantree.out");

struct Vertex {
	int x, y;
	double dist = INFINITY;
	bool inTree = false;
};

vector<Vertex> ver;

int countVertex = 1;
double MST;

double countWeight(int v1, int v2) {
	return sqrt((ver[v2].x - ver[v1].x)*(ver[v2].x - ver[v1].x) + (ver[v2].y - ver[v1].y)*(ver[v2].y - ver[v1].y));
}

int main() {
	int v; in >> v;
	ver.resize(v);

	for (int i = 0; i < v; i++) {
		in >> ver[i].x >> ver[i].y;
	}

	ver[0].dist = 0;
	ver[0].inTree = true;
	for (int i = 1; i < v; i++) {
		ver[i].dist = countWeight(0, i);
	}

	while (countVertex < v) {
		int minVertex;
		double minWeight = INFINITY;
		for (int i = 1; i < v; i++) {
			if (ver[i].dist < minWeight && !ver[i].inTree) {
				minVertex = i;
				minWeight = ver[i].dist;
			}
		}

		ver[minVertex].inTree = true;
		countVertex++;
		for (int i = 0; i < v; i++) {
			double tempWeight = countWeight(minVertex, i);
			if (minVertex != i && ver[i].dist > tempWeight && !ver[i].inTree) {
				ver[i].dist = tempWeight;
			}
		}
	}

	for (int i = 1; i < v; i++) {
		MST += ver[i].dist;
	}

	out << setprecision(15) << MST;
}