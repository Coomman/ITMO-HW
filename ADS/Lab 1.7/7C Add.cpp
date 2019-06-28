#include <fstream>
#include <vector>
#include <algorithm>

using namespace std;

struct Node {
	int value;

	int par = 0;
	int h = 0, bal = 0;
	int lch = 0, rch = 0;
};

struct AvlNode {
	int value;
	int lch = 0, rch = 0;
};

int v, root = 1;

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

	tree[n].h = 0;
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

int SLR(int a) {
	int b = tree[a].rch;
	tree[a].rch = tree[b].lch;
	tree[b].lch = a;

	tree[tree[b].rch].par = a;
	tree[b].par = tree[a].par;

	if (tree[a].value < tree[tree[a].par].value) {
		tree[tree[a].par].lch = b;
	}
	else {
		tree[tree[a].par].rch = b;
	}
	tree[a].par = b;

	if (a == 1) {
		root = b;
	}

	return b;
}

int SRR(int a) {
	int b = tree[a].lch;
	tree[a].lch = tree[b].rch;
	tree[b].rch = a;

	tree[tree[b].lch].par = a;
	tree[b].par = tree[a].par;

	if (tree[a].value < tree[tree[a].par].value) {
		tree[tree[a].par].lch = b;
	}
	else {
		tree[tree[a].par].rch = b;
	}
	tree[a].par = b;

	if (a == 1) {
		root = b;
	}

	return b;
}

int BLR(int a) {
	int b = tree[a].rch;
	SRR(b);
	SLR(a);

	return tree[b].lch;
}

int BRR(int a) {
	int b = tree[a].lch;
	SLR(b);
	SRR(a);

	return tree[b].rch;
}

int findPlace(int n) {
	int cur = 1;
	while (1) {
		if (n < tree[cur].value) {
			if (tree[cur].lch) {
				cur = tree[cur].lch;
			}
			else {
				tree[cur].lch = v;
				break;
			}
		}
		else {// n >
			if (tree[cur].rch) {
				cur = tree[cur].rch;
			}
			else {
				tree[cur].rch = v;
				break;
			}
		}
	}

	tree[v].value = n;
	tree[v].par = cur;
	dfs(1);

	return cur;
}

void add(int n) {
	int cur = findPlace(n);
	while (cur) {
		if (tree[cur].bal > 1) {// LR
			if (tree[tree[cur].rch].bal == -1) {// BLR
				cur = BLR(cur);
			}
			else {// SLR
				cur = SLR(cur);
			}
			dfs(root);
			continue;
		}
		else if (tree[cur].bal < -1) {// RR
			if (tree[tree[cur].lch].bal == 1) {// BRR
				cur = BRR(cur);
			}
			else {// SRR
				cur = SRR(cur);
			}
			dfs(root);
			continue;
		}
		cur = tree[cur].par;
	}

	reorder(root);
}

int main() {
	ifstream in("addition.in");
	ofstream out("addition.out");

	in >> v; v++;
	tree.resize(v + 1);
	AVL.resize(v + 1);

	for (int i = 1; i < v; i++) {
		int val, lch, rch; in >> val >> lch >> rch;
		tree[i].value = val;
		tree[i].lch = lch;
		tree[lch].par = i;
		tree[i].rch = rch;
		tree[rch].par = i;
	}

	int newNode; in >> newNode;
	if (v - 1) {
		add(newNode);
		
		out << v << endl;
		for (int i = 1; i <= v; i++) {
			out << AVL[i].value << ' ' << AVL[i].lch << ' ' << AVL[i].rch << endl;
		}
	}
	else {
		out << 1 << endl << newNode << " 0 0";
	}
}