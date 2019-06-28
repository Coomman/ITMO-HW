#include <fstream>
#include <string>

using namespace std;

struct Node {
	string key;
	string value;
	Node* prev = nullptr;
	Node* next = nullptr;

	Node* prev_put = nullptr;
	Node* next_put = nullptr;

	Node(string key, string value) {
		this->key = key;
		this->value = value;
	}
};

class List {
	Node* head = nullptr;

public:
	Node* isExist(string key);
	Node* getPrev(string key);
	Node* getNext(string key);
	void put(string key, string value);
	void delete_Node(string key);
};

Node* last_put = nullptr;

Node* List::isExist(string key) {
	Node* temp = head;

	while (temp != nullptr) {
		if (temp->key == key) {
			break;
		}
		else {
			temp = temp->next;
		}
	}
	return temp;
}

Node* List::getPrev(string key) {
	Node* temp = isExist(key);
	if (temp != nullptr) {
		return temp->prev_put;
	}
	return nullptr;
}

Node* List::getNext(string key) {
	Node* temp = isExist(key);
	if (temp != nullptr) {
		return temp->next_put;
	}
	return nullptr;
}

void List::put(string key, string value) {
	Node* p = isExist(key);

	if (p == nullptr) {
		Node* temp = new Node(key, value);
		temp->next = head;

		if (head != nullptr) {
			head->prev = temp;
		}
		head = temp;

		if (last_put != nullptr) {
			last_put->next_put = temp;
			temp->prev_put = last_put;
			last_put = temp;
		}
		else {
			last_put = temp;
		}
	}
	else {
		p->value = value;
	}
}

void List::delete_Node(string key) {
	Node* temp = head;

	while (temp != nullptr) {
		if (temp->key == key) {
			if (temp->prev == nullptr) {
				if (temp->next == nullptr) {
					head = nullptr;
				}
				else {
					head = head->next;
					head->prev = nullptr;
				}
			}
			else if (temp->next == nullptr) {
				temp->prev->next = nullptr;
			}
			else {
				temp->prev->next = temp->next;
				temp->next->prev = temp->prev;
			}
			//put change
			if (temp->prev_put != nullptr) {
				if (temp->next_put != nullptr) {
					temp->next_put->prev_put = temp->prev_put;
					temp->prev_put->next_put = temp->next_put;
				}
				else {
					temp->prev_put->next_put = nullptr;
				}
			}
			else {
				if (temp->next_put != nullptr) {
					temp->next_put->prev_put = nullptr;
				}
			}
			//last_put change
			if (temp->key == last_put->key) {
					last_put = last_put->prev_put;
			}
			delete temp;
			return;
		}
		else {
			temp = temp->next;
		}
	}
}

int makeHash(string x) {
	int h = 0;
	for (int i = 0; i < (int)x.length(); i++) {
		h += x[i] * (i + 1);
	}
	return h;
}

int main() {
	List hash_table[131071];
	ifstream input("linkedmap.in");
	ofstream output("linkedmap.out");

	string command;
	string key;
	string value;
	int hash;
	Node* p;

	while (input >> command) {
		input >> key;
		hash = makeHash(key);
		if (command == "put") {
			input >> value;
			hash_table[hash].put(key, value);
		}
		else if (command == "delete") {
			hash_table[hash].delete_Node(key);
		}
		else {
			if (command == "get") {
				p = hash_table[hash].isExist(key);
			}
			else if (command == "prev") {
				p = hash_table[hash].getPrev(key);
			}
			else {
				p = hash_table[hash].getNext(key);
			}

			if (p == nullptr) {
				output << "none" << endl;
			}
			else {
				output << p->value << endl;
			}
		}
	}

	return 0;
}