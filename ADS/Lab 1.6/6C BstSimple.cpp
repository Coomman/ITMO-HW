#include <iostream>
#include <fstream>
#include <string>

using namespace std;

ifstream in("bstsimple.in");
ofstream out("bstsimple.out");

struct Node {
	int key;
	Node *prev;
	Node *lch, *rch;
};

class Tree {
public:
	Node *root = nullptr;
	void add(int x) {
		Node *last;

		last = root;

		if (root == nullptr) {
			Node *temp = new Node;
			temp->rch = nullptr;
			temp->lch = nullptr;
			temp->lch = nullptr;
			temp->key = x;
			root = temp;
			return;
		}

		bool torch = true;

		while (last) {
			if (x >= last->key) {
				if (last->rch) {
					last = last->rch;
				}
				else {
					torch = true;
					break;
				}
			}
			else {
				if (last->lch) {
					last = last->lch;
				}
				else {
					torch = false;
					break;
				}
			}
		}

		Node *temp = new Node;
		temp->key = x;
		temp->prev = last;
		temp->lch = nullptr;
		temp->rch = nullptr;
		if (torch)
			last->rch = temp;
		else
			last->lch = temp;

	}

	bool isExist(int x) {
		Node *temp = root;
		bool ans = false;
		while (temp) {
			if (temp->key == x) {
				ans = true;
				break;
			}

			if (x >= temp->key) {
				if (temp->rch) {
					temp = temp->rch;
				}
				else
					break;
			}
			else {
				if (temp->lch) {
					temp = temp->lch;
				}
				else
					break;
			}

		}
		return ans;
	}


	void getNext(int x) {
		int ans;
		bool flag = false;
		Node *temp = root;

		while (temp) {
			if (temp->key <= x) {
				temp = temp->rch;
			}
			else {
				ans = temp->key;
				flag = true;
				temp = temp->lch;
			}
		}

		if (flag)
			out << ans << endl;
		else
			out << "none" << endl;

	}


	void getPrev(int x) {
		int ans;
		bool flag = false;
		Node *temp = root;

		while (temp) {
			if (temp->key >= x) {
				temp = temp->lch;
			}
			else {
				ans = temp->key;
				flag = true;
				temp = temp->rch;
			}
		}

		if (flag)
			out << ans << endl;
		else
			out << "none" << endl;

	}


	void deleteNode(int x) {
		Node *temp = root;
		bool ans = false;
		while (temp) {
			if (temp->key == x) {
				ans = true;
				break;
			}

			if (x >= temp->key) {
				if (temp->rch) {
					temp = temp->rch;
				}
				else
					break;
			}
			else {
				if (temp->lch) {
					temp = temp->lch;
				}
				else
					break;
			}
		}

		if (!ans)
			return;

		if (!temp->lch && !temp->rch) {

			if (temp == root) {
				delete temp;
				root = nullptr;
			}
			else {
				if (temp->prev->lch == temp) {
					temp->prev->lch = nullptr;
					delete temp;
				}
				else {
					temp->prev->rch = nullptr;
					delete temp;
				}
			}
		}
		else {
			if ((temp->lch && !temp->rch) || (!temp->lch && temp->rch)) {
				if (temp == root) {
					if (temp->lch) {
						root = temp->lch;
						root->prev = nullptr;
						delete temp;
					}
					else {
						root = temp->rch;
						root->prev = nullptr;
						delete temp;
					}
				}
				else {

					if (temp->lch) {
						temp->lch->prev = temp->prev;
						if (temp->prev->lch == temp) {
							temp->prev->lch = temp->lch;
							delete temp;
						}
						else {
							temp->prev->rch = temp->lch;
							delete temp;
						}
					}
					else {
						temp->rch->prev = temp->prev;
						if (temp->prev->lch == temp) {
							temp->prev->lch = temp->rch;
							delete temp;
						}
						else {
							temp->prev->rch = temp->rch;
							delete temp;
						}
					}

				}

			}
			else {
				Node *next = temp->rch;

				while (next->lch)
					next = next->lch;

				temp->key = next->key;

				if (next->prev->lch == next) {
					next->prev->lch = nullptr;
				}
				else {
					next->prev->rch = nullptr;
				}

				delete next;

			}
		}
	}
};

Tree tree;

int main() {
	string command;
	int x;

	while (in >> command) {
		if (command[0] == 'i') {
			in >> x;
			if (!tree.isExist(x))
				tree.add(x);
		}
		else {
			if (command[0] == 'e') {
				in >> x;
				if (tree.isExist(x))
					out << "true" << endl;
				else
					out << "false" << endl;

			}
			else {
				if (command[0] == 'n') {
					in >> x;
					tree.getNext(x);
				}
				else {
					if (command[0] == 'p') {
						in >> x;
						tree.getPrev(x);
					}
					else {
						if (command[0] == 'd') {
							in >> x;
							tree.deleteNode(x);
						}
					}
				}
			}
		}

	}
}