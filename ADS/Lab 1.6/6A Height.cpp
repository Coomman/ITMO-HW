#include <iostream>
#include <fstream>

using namespace std;

struct Node {
	int key, h = 0;
	int par = 0;
	int lch = 0, rch = 0;
};

int main() {
	ifstream in("height.in");
	ofstream out("height.out");

	int max = 0;
	int count; in >> count;
	Node* tree;
	if (count != 0) {
		tree = new Node[count + 1];
		int key, l, r;
		in >> key; in >> l; in >> r;
		tree[1].key = key;
		tree[1].h = 1;
		tree[1].lch = l;
		tree[1].rch = r;
		max++;
		if (l != 0) {
			tree[l].par = 1;
			tree[l].h = 2;
		}
		if (r != 0) {
			tree[r].par = 1;
			tree[r].h = 2;
		}

		for (int i = 2; i <= count; i++) {
			in >> key; in >> l; in >> r;
			tree[i].lch = l;
			tree[i].rch = r;
			tree[i].key = key;
			
			if (l != 0) {
				tree[l].h = tree[i].h + 1;
				if (tree[l].h > max) {
					max = tree[l].h;
				}
				tree[l].par = i;
			}
			if (r != 0) {
				tree[r].h = tree[i].h + 1;
				if (tree[r].h > max) {
					max = tree[r].h;
				}
				tree[r].par = i;
			}
		}
		
		out << max;
	}
	else {
		out << 0;
	}
}