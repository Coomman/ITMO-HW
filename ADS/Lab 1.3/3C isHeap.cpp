#include <iostream>
#include <fstream>
#include <vector>
#include <algorithm>

using namespace std;

int isHeap(vector<int> vec) {
	int n = vec.size() - 1;
	int i = 1;
	while (2 * i + 1 <= n) {
		if (vec[i] > vec[2 * i] || vec[i] > vec[2 * i + 1]) {
			return 1;
		}
		i++;
	}
	return 0;
}

int main() {
	ifstream input("isheap.in");
	ofstream output("isheap.out");

	int size; input >> size;
	vector<int> am(size + 1);

	for (int i = 1; i <= size; i++) {
		input >> am[i];
	}

	if (isHeap(am)) {
		output << "NO";
	}
	else {
		output << "YES";
	}
}