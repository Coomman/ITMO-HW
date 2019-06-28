#include <fstream>
#include <vector>
#include <algorithm>

using namespace std;

struct Node {
	int key;

	int par = 0;
	int h = 1;
	int lch = 0, rch = 0;
};

struct AvlNode {
	int key;
	int lch = 0, rch = 0;
};

int root = 1;

vector<Node> tree;
vector<AvlNode> AVL;

int _rebalance(int n) {
	if (tree[n].lch && tree[n].rch) {
		tree[n].h = max(_rebalance(tree[n].lch), _rebalance(tree[n].rch)) + 1;
		return tree[n].h;
	}

	if (tree[n].lch) { // no rch
		tree[n].h = _rebalance(tree[n].lch) + 1;
		return tree[n].h;
	}

	if (tree[n].rch) { // no lch
		tree[n].h = _rebalance(tree[n].rch) + 1;
		return tree[n].h;
	}

	return tree[n].h;
}

int height(int cur) {
	return cur ? tree[cur].h : 0;
}

void fixHeight(int cur) {
	int h_lch = height(tree[cur].lch);
	int h_rch = height(tree[cur].rch);
	tree[cur].h = (h_lch > h_rch ? h_lch : h_rch) + 1;
}

int balance(int cur) {
	return height(tree[cur].rch) - height(tree[cur].lch);
}

int curNode = 1;
void reorder(int n) {
	AVL[curNode].key = tree[n].key;

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

int getPrev(int cur) {
	cur = tree[cur].lch;
	
	while (tree[cur].rch) {
		cur = tree[cur].rch;
	}

	return cur;
}

int findPlace(int n) {
	int cur = root;
	while (tree[cur].key != n) {//find node
		if (n < tree[cur].key) {
			cur = tree[cur].lch;
		}
		else {
			cur = tree[cur].rch;
		}
	}
	
	return cur;
}

void changeChild(int cur, int ch) {
	if (tree[cur].key < tree[tree[cur].par].key) {
		tree[tree[cur].par].lch = ch;
	}
	else {
		tree[tree[cur].par].rch = ch;
	}

	if (ch) {
		tree[ch].par = tree[cur].par;
	}
}

int SLR(int a) {
	int b = tree[a].rch;
	tree[a].rch = tree[b].lch;
	if (tree[a].rch) {
		tree[tree[a].rch].par = a;
	}

	tree[b].lch = a;
	tree[b].par = tree[a].par;
	tree[a].par = b;

	fixHeight(a);
	fixHeight(b);

	return b;
}

int SRR(int a) {
	int b = tree[a].lch;
	tree[a].lch = tree[b].rch;
	if (tree[a].lch) {
		tree[tree[a].lch].par = a;
	}

	tree[b].rch = a;
	tree[b].par = tree[a].par;
	tree[a].par = b;

	fixHeight(a);
	fixHeight(b);

	return b;
}

void rebalance(int cur) {
	while (cur) {
		fixHeight(cur);
		if (balance(cur) == 2 || balance(cur) == -2) {
			if (balance(cur) == 2) {
				if (balance(tree[cur].rch) < 0) {
					tree[cur].rch = SRR(tree[cur].rch);
				}

				cur = SLR(cur);
				if (tree[cur].lch == root) {
					root = cur;
				}
				else {
					changeChild(cur, cur);
				}
			}
			else {
				if (balance(tree[cur].lch) > 0) {
					tree[cur].lch = SLR(tree[cur].lch);
					tree[tree[cur].lch].par = cur;
				}

				cur = SRR(cur);
				if (tree[cur].rch == root) {
					root = cur;
				}
				else {
					changeChild(cur, cur);
				}
			}

			continue;
		}
		cur = tree[cur].par;
	}
}

void erase(int n) {
	int cur = findPlace(n);
	
	int parent = tree[cur].par;
	if (tree[cur].lch && tree[cur].rch) {
		int temp = getPrev(cur);
		int key = tree[temp].key;
		erase(key);
		tree[cur].key = key;
	}
	else if (tree[cur].lch || tree[cur].rch) {
		int temp = tree[cur].lch ? tree[cur].lch : tree[cur].rch;
		if (cur != root) {
			changeChild(cur, temp);
		}
		else {
			root = temp;
			tree[temp].par = 0;
		}
	}
	else {
		if (cur != root) {
			changeChild(cur, 0);
		}
		else {
			root = 0;
		}
	}

	if (parent) {
		rebalance(parent);
	}
}

int main() {
	ifstream in("deletion.in");
	ofstream out("deletion.out");

	int v; in >> v;
	tree.resize(v + 1);
	AVL.resize(v);

	for (int i = 1; i <= v; i++) {
		int val, lch, rch; in >> val >> lch >> rch;
		tree[i].key = val;
		tree[i].lch = lch;
		tree[i].rch = rch;
		tree[lch].par = tree[rch].par = i;
	}

	int newNode; in >> newNode;
	if (v - 1) {
		_rebalance(root);
		erase(newNode);
		reorder(root);

		out << v - 1 << endl;
		for (int i = 1; i < v; i++) {
			out << AVL[i].key << ' ' << AVL[i].lch << ' ' << AVL[i].rch << endl;
		}
	}
	else {
		out << 0;
	}
}