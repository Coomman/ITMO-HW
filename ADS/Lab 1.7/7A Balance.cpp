#include <fstream>
#include <vector>
#include <algorithm>
using namespace std;

struct Node {
	int value;
	int h = 0, bal = 0;
	int lch = 0, rch = 0;
};

vector<Node> tree;

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

int main() {
	ifstream in("balance.in");
	ofstream out("balance.out");

	int v; in >> v;
	tree.resize(v + 1);

	for (int i = 1; i <= v; i++) {
		int val, lch, rch; in >> val >> lch >> rch;
		tree[i].value = val;

		if (lch) {
			tree[i].lch = lch;
		}

		if (rch) {
			tree[i].rch = rch;
		}
	}
	
	dfs(1);

	for (int i = 1; i <= v; i++) {
		out << tree[i].bal << endl;
	}
}