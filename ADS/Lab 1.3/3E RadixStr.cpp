#include <iostream>
#include <fstream>
#include <vector>
#include <string>

using namespace std;

void digitSorting(vector<string> &vec, int off) {
	vector <vector <string>> dMas(26);
	int size = vec.size();

	for (int i = 0; i < vec.size(); i++) {
		dMas[vec[i][off] - 97].push_back(vec[i]);
	}

	vec.clear();

	int elNum = 0;
	for (int i = 0; i < 26; i++) {
		while (elNum < dMas[i].size()) {
			vec.push_back(dMas[i][elNum]);
			elNum++;
		}
		elNum = 0;
	}
}

void radixSort(vector<string> &vec, int times, int prolong) {
	for (int i = 0; i < times; i++) {
		digitSorting(vec, prolong - i);
	}
}

int main() {
	ifstream input("radixsort.in");
	ofstream output("radixsort.out");

	int size; input >> size;
	int prolong; input >> prolong; prolong--;
	int times; input >> times;
	vector<string> am(size);

	for (int i = 0; i < size; i++) {
		input >> am[i];
	}

	radixSort(am, times, prolong);

	for (int i = 0; i < size; i++) {
		output << am[i] << endl;
	}
}