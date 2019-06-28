#include <iostream>
#include <fstream>
#include <vector>
#include <algorithm>
#include <cmath>

int dCount;

using namespace std;

int countDigits(int n) {
	int count = 0;
	while (n) {
		n /= 10;
		count++;
	}
	return count;
}

void digitSorting(vector<int> &vec, int offset) {
	vector <vector <int>> dMas(10);
	int size = vec.size();
	int off = pow(10, offset);
	for (int i = 0; i < vec.size(); i++) {		
		dMas[(vec[i]/off) % 10].push_back(vec[i]);
	}

	vec.clear();

	int elNum = 0;
	for (int i = 0; i < 10; i++) {
		while (elNum < dMas[i].size()) {
			vec.push_back(dMas[i][elNum]);
			elNum++;
		}
		elNum = 0;
	}
}

void radixSort(vector<int> &vec) {
	for (int i = 0; i < dCount; i++) {
		digitSorting(vec, i);
	}
}

int main() {
	ifstream input("sort.in");
	ofstream output("sort.out");

	int size; input >> size;
	vector<int> am(size);

	for (int i = 0; i < size; i++) {
		input >> am[i];
	}

	int max = *max_element(am.begin(), am.end());
	dCount = countDigits(max);

	radixSort(am);

	for (int i = 0; i < size; i++) {
		output << am[i] << ' ';
	}
}