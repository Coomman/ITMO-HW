#include <iostream>
#include <vector>
#include <algorithm>
#include <fstream>
using namespace std;

int main() {
	ifstream input("inversions.in");
	ofstream output("inversions.out");
	int size;
	input >> size;
	vector<int> am(size), sortAm(size);

	for (int i = 0; i < size; i++) {
		input >> am[i];
		sortAm[i] = am[i];
	}
	int countInv = 0;
	sort(sortAm.begin(), sortAm.end());


	for (int i = 0; i < size; i++) {
		auto temp = find(sortAm.begin(), sortAm.end(), am[0]);
		countInv += (temp - sortAm.begin());
		am.erase(am.begin());
		sortAm.erase(temp);
	}
	output << countInv;
}