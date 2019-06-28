#include <fstream>
#include <vector>
#include <algorithm>

using namespace std;

struct Node {
	int value;

	int h = 0, bal = 0;
	int lch = 0, rch = 0;
};

struct AvlNode {
	int value;
	int lch = 0, rch = 0;
};

vector<Node> tree;
vector<AvlNode> AVL;

int dfs(int n) {
	if (tree[n].lch && tree[n].rch) {
		tree[n].h = max(dfs(tree[n].lch), dfs(tree[n].rch)) + 1;
		tree[n].bal = tree[tree[n].rch].h - tree[tree[n].lch].h;
		return tree[n].h;
	}

	if (tree[n].lch) { // no rch
		tree[n].h = dfs(tree[n].lch) + 1;
		tree[n].bal = -tree[n].h;
		return tree[n].h;
	}

	if (tree[n].rch) { // no lch
		tree[n].h = dfs(tree[n].rch) + 1;
		tree[n].bal = tree[n].h;
		return tree[n].h;
	}

	return tree[n].h;
}

int curNode = 1;
void reorder(int n) {
	AVL[curNode].value = tree[n].value;
	
	int temp = curNode;
	if (tree[n].lch) {	
		AVL[temp].lch = ++curNode;
		reorder(tree[n].lch);
	}

	if (tree[n].rch) {
		AVL[temp].rch = ++curNode;
		reorder(tree[n].rch);
	}
}

void SLR() {
	int b = tree[1].rch;
	tree[1].rch = tree[b].lch;
	tree[b].lch = 1;

	reorder(b);
}

void BLR() {
	int b = tree[1].rch;
	int c = tree[b].lch;
	tree[1].rch = tree[c].lch;
	tree[b].lch = tree[c].rch;
	tree[c].lch = 1;
	tree[c].rch = b;

	reorder(c);
}

int main() {
	ifstream in("rotation.in");
	ofstream out("rotation.out");

	int v; in >> v;
	tree.resize(v + 1);
	AVL.resize(v + 1);
	
	for (int i = 1; i <= v; i++) {
		int val, lch, rch; in >> val >> lch >> rch;
		tree[i].value = val;
		tree[i].lch = lch;
		tree[i].rch = rch;
	}

	dfs(tree[1].rch);

	if (tree[tree[1].rch].bal != -1) { //Small Left Rotation
		SLR();
	}
	else { //Big Left Rotation
		BLR();
	}

	out << v << endl;
	for (int i = 1; i <= v; i++) {
		out << AVL[i].value << ' ' << AVL[i].lch << ' ' << AVL[i].rch << endl;
	}
}