#include <iostream>
#include <vector>
#include <algorithm>
#include <fstream>

using namespace std;

int main() {
	ifstream input("antiqs.in");
	ofstream output("antiqs.out");
	int size;
	input >> size;

	vector<int> am(size);

	for (int i = 0; i < size; i++) {
		am[i] = i + 1;
	}

	for (int i = 2; i < size; i++) {
		swap(am[i], am[i / 2]);
	}

	for (int i = 0; i < size; i++) {
		output << am[i] << ' ';
	}

}