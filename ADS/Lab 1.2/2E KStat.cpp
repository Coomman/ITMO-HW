#include <iostream>
#include <vector>
#include <fstream>
#include <algorithm>

using namespace std;

int partition(vector<int> &vec, int l, int r) {
	int i = l - 1;
	int j = r;
	int m = vec[r];
	while (i < j) {
		while (vec[++i] < m) {
		}
		while (vec[--j] > m && j >= 0) {
		}
		if (i < j) {
			swap(vec[i], vec[j]);
		}
	}
	swap(vec[i], vec[r]);
	return i;
}

int kthStat(vector<int> &vec, int n, int size) {
	int l = 0, r = size;
	while (1) {
		int pivot = partition(vec, l, r);
		if (pivot == n) {
			return vec[pivot];
		}
		else
			if (n < pivot) {
				r = pivot - 1;
			}
			else {
				l = pivot + 1;
			}
	}
}


int main() {
	ifstream input("kth.in");
	ofstream output("kth.out");
	
	int size, kNum;
	input >> size >> kNum;

	int A, B, C;
	input >> A >> B >> C;

	vector<int> am(size);
	input >> am[0] >> am[1];
	

	for (int i = 2; i < size; i++) {
		am[i] = A*am[i - 2] + B*am[i - 1] + C;		
	}

	size--;
	output << kthStat(am, kNum - 1, size);
}