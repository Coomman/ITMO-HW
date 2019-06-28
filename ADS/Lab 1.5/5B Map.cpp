#include <fstream>
#include <string>
#include <iostream>

using namespace std;

struct Node {
	string key;
	string value;
	Node* prev = nullptr;
	Node* next = nullptr;

	Node(string key, string value) {
		this->key = key;
		this->value = value;
	}
};

class List {
	Node* head = nullptr;

public:
	void insert_Node(string key, string value);
	void delete_Node(string value);
	Node* isExist(string value);
};

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

void List::insert_Node(string key, string value) {
	Node* p = isExist(key);

	if (p == nullptr) {
		Node* temp = new Node(key, value);
		temp->next = head;

		if (head != nullptr) {
			head->prev = temp;
		}
		head = temp;
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
			delete temp;
			break;
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
	ifstream input("map.in");
	ofstream output("map.out");

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
			hash_table[hash].insert_Node(key, value);
		}
		else if (command == "delete") {
			hash_table[hash].delete_Node(key);
		}
		else { //get		
			p = hash_table[hash].isExist(key);
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