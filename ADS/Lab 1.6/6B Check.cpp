#include <iostream>
#include <fstream>

using namespace std;

struct Node {
	int key;
	int par = 0;
	int lch = 0, rch = 0;
};

bool check(Node* tree, int ind, int min, int max) {
	if (ind != 0) {
		if (tree[ind].key >= max || tree[ind].key <= min) {
			return false;
		}
		else {
			return check(tree, tree[ind].lch, min, tree[ind].key) && check(tree, tree[ind].rch, tree[ind].key, max);
		}
	}
	else {
		return true;
	}
}

int main() {
	ifstream in("check.in");
	ofstream out("check.out");

	int count; in >> count;
	Node* tree;
	if (count != 0) {
		tree = new Node[count + 1];
		int key, l, r;

		for (int i = 1; i <= count; i++) {
			in >> key; in >> l; in >> r;
			tree[i].lch = l;
			tree[i].rch = r;
			tree[i].key = key;

			if (l != 0) {
				tree[l].par = i;
			}
			if (r != 0) {
				tree[r].par = i;
			}
		}
		
		if (check(tree, 1, -2000000000, 2000000000)) {
			out << "YES";
		}
		else {
			out << "NO";
		}
	}
	else {
		out << "YES";
	}
}