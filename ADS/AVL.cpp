#include <algorithm>
using namespace std;

template <class T>
class AVL {
	struct Node {
		T key;
		int h = 1;

		Node* par = nullptr;
		Node* lch = nullptr;
		Node* rch = nullptr;

		Node(const T& v)
			: key(v) {}

		Node*& child(const bool& side) {
			return side ? lch : rch;
		}
	};

private: // Fields
	Node* root = nullptr;

private: // Get height and balance
	int height(Node* cur) {
		return cur ? cur->h : 0;
	}

	void fixHeight(Node* cur) {
		cur->h = max(height(cur->lch), height(cur->rch)) + 1;
	}

	int balance(Node* cur) {
		return height(cur->rch) - height(cur->lch);
	}

private: // Helper methods
	void changeChild(Node* cur, Node* ch) {
		cur->par->child(cur->key < cur->par->key) = ch;

		if (ch) {
			ch->par = cur->par;
		}
	}

	Node* findParent(const T& n) {
		Node* ch = new Node(n);
		Node* cur = root;
		while (cur) {
			if (n == cur->key) {
				return nullptr;
			}

			Node* temp = cur->child(n < cur->key);
			if (!temp) {
				cur->child(n < cur->key) = ch;
				break;
			}

			cur = temp;
		}

		ch->par = cur;
		return cur;
	}

	Node* getPrev(Node* cur) {
		for (cur = cur->lch; cur->rch; cur = cur->rch);
		return cur;
	}

private: // Rebalance
	Node* rotate(Node* a, const bool& side) {
		Node* b = a->child(side);
		a->child(side) = b->child(!side);
		if (a->child(side)) {
			a->child(side)->par = a;
		}

		b->child(!side) = a;
		b->par = a->par;
		a->par = b;

		fixHeight(a);
		fixHeight(b);

		return b;
	}

	void rebalance(Node* cur) {
		while (cur) {
			fixHeight(cur);
			if (abs(balance(cur)) == 2) {
				bool side = (balance(cur) == 2);

				if (side && balance(cur->rch) < 0) {
					cur->rch = rotate(cur->rch, side);
				}
				else if (!side && balance(cur->lch) > 0) {
					cur->lch = rotate(cur->lch, side);
				}

				cur = rotate(cur, !side);

				if (cur->child(side) == root) {
					root = cur;
				}
				else {
					changeChild(cur, cur);
				}

				continue;
			}

			cur = cur->par;
		}
	}

public: // Methods
	Node* find(const T& n) {
		Node* temp = root;
		while (temp) {
			if (n == temp->key) {
				break;
			}

			temp = temp->child(n < temp->key);
		}

		return temp;
	}

	void add(const T& n) {
		if (root) {
			Node* par = findParent(n);
			if (par) {
				rebalance(par);
			}
		}
		else {
			root = new Node(n);
		}
	}

	void erase(const T& n) {
		Node* cur = find(n);
		if (cur) {
			Node* parent = cur->par;
			if (cur->lch && cur->rch) {
				Node* temp = getPrev(cur);
				T key = temp->key;
				erase(key);
				cur->key = key;
				return;
			}
			else {
				if (cur->lch || cur->rch) {
					Node* temp = cur->child(cur->lch);
					if (cur != root) {
						changeChild(cur, temp);
					}
					else {
						root = temp;
						temp->par = nullptr;
					}
				}
				else {
					if (cur != root) {
						changeChild(cur, nullptr);
					}
					else {
						root = nullptr;
					}
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