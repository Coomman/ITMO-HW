#include <iostream>
#include <fstream>
#include <string>
#include <vector>

using namespace std;

struct el {
	int key;
	int push;

	el(int k, int p) {
		key = k;
		push = p;
	}
};


void lift(vector<el> &heap, int pos) {
	while (pos != 1) {
		if (heap[pos].key < heap[pos / 2].key) {
			swap(heap[pos], heap[pos / 2]);
			pos /= 2;
		}
		else {
			break;
		}
	}
}

void drown(vector<el> &heap, int size) {// >=?
	int cur = 1;

	while (2 * cur <= size) {
		if (cur * 2 == size) {
			if (heap[cur].key > heap[cur * 2].key) {
				swap(heap[cur], heap[2 * cur]);
				cur *= 2;
				continue;
			}
			else {
				break;
			}
		}

		int lch = cur * 2;
		int rch = cur * 2 + 1;
		if (heap[lch].key < heap[rch].key) {
			if (heap[cur].key > heap[lch].key) {
				swap(heap[cur], heap[lch]);
				cur = lch;
			}
			else {
				break;
			}
		}
		else {
			if (heap[cur].key > heap[rch].key) {
				swap(heap[cur], heap[rch]);
				cur = rch;
			}
			else {
				break;
			}
		}
	}
}

void decreaseKey(vector<el> &heap, int size, int x, int y) {
	int index;
	for (index = 1; index <= size; index++) {
		if (heap[index].push == x)
			break;
	}
	heap[index].key = y;
	lift(heap, index);
}

int main() {
	freopen("priorityqueue.in", "r", stdin);
	ofstream output("priorityqueue.out");

	vector<el> heap;
	heap.push_back(el::el(0,0));
	int	count = 1;
	int tempvalue, size = 0, x, y;
	string command;

	while (cin >> command) {
		if (command == "push") {
			cin >> tempvalue;
			heap.push_back(el::el(tempvalue, count));
			size++;
			lift(heap, size);
		}
		else if (command == "extract-min") {
			if (size != 0) {
				output << heap[1].key << endl;
				swap(heap[1], heap[size]);
				heap.pop_back();
				size--;
				drown(heap, size);
			}
			else {
				output << '*' << endl;
			}
		}
		else { //decrease-key
			cin >> x;
			cin >> y;
			decreaseKey(heap, size, x, y);
		}
		count++;
	}

}