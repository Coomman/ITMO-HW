#include <iostream>
#include <vector>
#include <fstream>
using namespace std;

long long countInv = 0;

void merge(vector<int> &A, int p, int q, int r){
	int n1 = q - p + 1;
	int n2 = r - q;
	vector<int> L(n1), R(n2);

	for (int i = 0; i < n1; i++) {
		L[i] = A[p + i];
	}
	for (int j = 0; j < n2; j++) {
		R[j] = A[q + 1 + j];
	}

	int i = 0, j = 0, k = p;

	while (i < n1 && j < n2)
	{
		if (L[i] <= R[j]){
			A[k] = L[i];
			i++;
		}else{
			A[k] = R[j];
			j++;
			countInv += (L.size() - i);
		}
		k++;
	}

	while (i < n1) {
		A[k] = L[i];
		i++;
		k++;
	}

	while (j < n2) {
		A[k] = R[j];
		j++;
		k++;
	}
}

void mergeSort(vector<int> &A, int p, int r) {
	if(p < r) {
		int q = (p + r) / 2;
		mergeSort(A, p, q);
		mergeSort(A, q + 1, r);
		merge(A, p, q, r);
	}
}

int main() {
	ifstream input("inversions.in");
	ofstream output("inversions.out");
	int size;
	input >> size;
	vector<int> am(size);
	for (int i = 0; i < size; i++) {
		input >> am[i];
	}

	mergeSort(am, 0, size-1);
	output << countInv;
}