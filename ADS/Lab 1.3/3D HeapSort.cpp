#include <iostream>
#include <vector>
#include <algorithm>
#include <fstream>

using namespace std;

void heapify(vector<int> &vec, int size, int i){
	int root = i;
	int lch = 2 * i + 1;
	int rch = 2 * i + 2;

	if (lch < size && vec[lch] > vec[root]) {
		root = lch;
	}

	if (rch < size && vec[rch] > vec[root]) {
		root = rch;
	}

	if (root != i){
		swap(vec[i], vec[root]);
		heapify(vec, size, root);
	}
}

void heapSort(vector<int> &vec, int size){
	for (int i = size / 2 - 1; i >= 0; i--) {
		heapify(vec, size, i);
	}

	// Heap sort
	for (int i = size - 1; i >= 0; i--)	{
		swap(vec[0], vec[i]);
		heapify(vec, i, 0);
	}
}

int main(){
	ifstream input("sort.in");
	ofstream output("sort.out");

	int size; input >> size;
	vector<int> am(size);
	for (int i = 0; i < size; i++) {
		input >> am[i];
	}
	
	heapSort(am, size);

	for (int i = 0; i < size; i++) {
		output << am[i]<<' ';
	}
}