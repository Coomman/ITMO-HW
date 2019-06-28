#include <iostream>
#include <vector>
#include <fstream>
#include <iomanip>

using namespace std;

double eps = 0.00001; // = 0.01/1000; Погрешность
double hN;

double makeGarland(double h1, double h2, int size) {
	vector<double> garland(size);
	garland[0] = h1;
	garland[1] = h2;
	double min = h1;

	for (int i = 2; i < size; i++) {
		garland[i] = 2 * garland[i - 1] - garland[i - 2] + 2;
		if (garland[i] < min) {
			min = garland[i]; //Нахождение лампочки касающейся земли
		}
	}

	hN = garland[size - 1];
	return min;
}

void binarySearch(double h1, int size) {
	double l = 0, r = h1;
	while (r - l > eps) {
		double h2 = (r + l) / 2;
		if (makeGarland(h1, h2, size) >=0 ){
			r = h2;
		}
		else {
			l = h2;
		}
	}
}

int main() {
	ifstream input("garland.in");
	ofstream output("garland.out");
	double h1;
	int size;
	input >> size >> h1;

	binarySearch(h1, size);
	hN = static_cast<int>(hN * 100 + 0.5) / 100.00;
	output << fixed << setprecision(2) << hN;
}