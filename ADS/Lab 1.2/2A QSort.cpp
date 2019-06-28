#include <iostream>
#include <algorithm>
#include <vector>
#include <ctime>
#include <fstream>
using namespace std;

int randPart(int left, int right) {
	srand(time(NULL));
	return (left + rand() % (right - left + 1));
}

int countInv;

void quickSort(vector<int> &arr, int left, int right){
	int i = left, j = right;
	//int pivot = arr[randPart(left, right)];
	int pivot = arr[(left + right) / 2];

	while (i <= j) {
		while (arr[i] < pivot) {
			i++;
		}

		while (arr[j] > pivot) {
			j--;
		}

		if (i <= j) {
			swap(arr[i], arr[j]);
			i++;
			j--;
		}
	}

	if (left < j) {
		quickSort(arr, left, j);
	}
	if (i < right) {
		quickSort(arr, i, right);
	}
}

int main() {
	ifstream input("sort.in");
	ofstream output("sort.out");

	int size;
	input >> size;
	
	
	vector<int> am(size);
	for (int i = 0; i < size; i++) {
		input >> am[i];
	}
	size--;

	for (int i = 0; i <= size; i++) {
		cout << am[i] << ' ';
	}
	quickSort(am, 0, size);

	/*for (int i = 0; i <= size; i++) {
		cout << am[i] << ' ';
	}*/
	cout << endl << countInv;
	getchar();
}