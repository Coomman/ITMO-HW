#include <fstream>
#include <algorithm>
using namespace std;

struct Node {
	int key;
	int h = 1;

	Node* par = nullptr;
	Node* lch = nullptr;
	Node* rch = nullptr;

	Node(int v) {
		key = v;
	}
};

class AVL {
Node* root = nullptr;

private: // Get height and balance
	int height(Node* cur) {
		return cur ? cur->h : 0;
	}

	void fixHeight(Node* cur) {
		int h_lch = height(cur->lch);
		int h_rch = height(cur->rch);
		cur->h = (h_lch > h_rch ? h_lch : h_rch) + 1;
	}

	int balance(Node* cur) {
		return height(cur->rch) - height(cur->lch);
	}

private: // Helper methods
	void changeChild(Node* cur, Node* ch) {
		if (cur->key < cur->par->key) {
			cur->par->lch = ch;
		}
		else {
			cur->par->rch = ch;
		}

		if (ch) {
			ch->par = cur->par;
		}
	}

	Node* findParent(int n) {
		Node* ch = new Node(n);
		Node* temp = root;
		while (temp) {
			if (n == temp->key) {
				delete ch;
				return nullptr;
			}
			else {
				if (n < temp->key) {
					if (temp->lch) {
						temp = temp->lch;
					}
					else {
						temp->lch = ch;
						break;
					}
				}
				else {
					if (temp->rch) {
						temp = temp->rch;
					}
					else {
						temp->rch = ch;
						break;
					}
				}
			}
		}

		ch->par = temp;
		return temp;
	}

	Node* getPrev(Node* cur) {
		cur = cur->lch;
		while (cur->rch) {
			cur = cur->rch;
		}

		return cur;
	}

private: // Rebalance
	Node* SLR(Node* a) {
		Node* b = a->rch;
		a->rch = b->lch;
		if (a->rch) {
			a->rch->par = a;
		}

		b->lch = a;
		b->par = a->par;
		a->par = b;

		fixHeight(a);
		fixHeight(b);

		return b;
	}

	Node* SRR(Node* a) {
		Node* b = a->lch;
		a->lch = b->rch;
		if (a->lch) {
			a->lch->par = a;
		}

		b->rch = a;
		b->par = a->par;
		a->par = b;

		fixHeight(a);
		fixHeight(b);

		return b;
	}

	void rebalance(Node* cur) {	
		while (cur) {
			fixHeight(cur);
			if (balance(cur) == 2 || balance(cur) == -2) {
				if (balance(cur) == 2) {
					if (balance(cur->rch) < 0) {
						cur->rch = SRR(cur->rch);
					}

					cur = SLR(cur);
					if (cur->lch == root) {
						root = cur;
					}
					else {
						changeChild(cur, cur);
					}
				}
				else {
					if (balance(cur->lch) > 0) {
						cur->lch = SLR(cur->lch);
						cur->lch->par = cur;
					}
					
					cur = SRR(cur);
					if (cur->rch == root) {
						root = cur;
					}
					else {
						changeChild(cur, cur);
					}
				}
				
				continue;
			}
			cur = cur->par;
		}
	}

public: // Methods
	Node* find(int n) {
		Node* temp = root;
		while (temp) {
			if (n == temp->key) {
				return temp;
			}
			else {
				temp = n < temp->key ? temp->lch : temp->rch;
			}
		}

		return nullptr;
	}

	void add(int n) {
		if (root) {
			Node* par = findParent(n);
			if (par) {
				rebalance(par);
			}
		}
		else {
			Node* ch = new Node(n);
			root = ch;
		}
	}

	void erase(int n) {
		Node* cur = find(n);
		if (cur) {
			Node* parent = cur->par;
			if (cur->lch && cur->rch) {
				Node* temp = getPrev(cur);
				int key = temp->key;
				erase(key);
				cur->key = key;
			}
			else if (cur->lch || cur->rch) {
				Node* temp = cur->lch ? cur->lch : cur->rch;
				if (cur != root) {
					changeChild(cur, temp);
				}
				else {
					root = temp;
					temp->par = nullptr;
				}
				delete cur;
			}
			else {
				if (cur != root) {
					changeChild(cur, nullptr);
				}
				else {
					root = nullptr;
				}
				delete cur;
			}

			if (parent) {
				rebalance(parent);
			}
		}
	}

	int getRootBalance() {
		return root ? balance(root) : 0;
	}
};

AVL tree;

int main() {
	ifstream in("avlset.in");
	ofstream out("avlset.out");

	int count; in >> count;

	char command; int num;
	for (int i = 0; i < count; ++i) {
		in >> command; in >> num;

		switch (command) {
		case 'A':
			tree.add(num);
			out << tree.getRootBalance() << endl;
			break;

		case 'D':
			tree.erase(num);
			out << tree.getRootBalance() << endl;
			break;

		case 'C':
			tree.find(num) ? out << 'Y' : out << 'N';
			out << endl;
			break;

		default:
			break;
		}
	}
}