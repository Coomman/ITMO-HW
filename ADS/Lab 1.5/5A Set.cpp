#include <fstream>
#include <string>

using namespace std;

struct node {
	int key;
	node* prev = nullptr;
	node* next = nullptr;

	node(int value) {
		key = value;
	}
};

class list{
public:
	node* head = nullptr;
	node* tail = nullptr;
	int size = 0;

	void insert_node(int value);
	void delete_node(int value);
	bool isExist(int value);
};

bool list::isExist(int value) {
	node* temp = head;

	while (temp != nullptr) {
		if (temp->key == value) {
			return true;
		}
			temp = temp->next;
	}
	return false;
}

void list::insert_node(int value) {
	if (!isExist(value)) {
		size++;
		node* temp = new node(value);
		temp->next = nullptr;
		temp->key = value;

		if (head) {
			temp->prev = tail;
			tail->next = temp;
			tail = temp;
		}
		else {
			temp->prev = nullptr;
			head = tail = temp;
		}
	}
}

void list::delete_node(int value) {
	node* temp = head;
	int index = 0;

	while (temp != nullptr) {
		if (temp->key == value) {
			if ((index == 0) && (temp->next)) {
				head = head->next;
				head->prev = nullptr;
				delete temp;
				size--;
				return;
			}
			else {
				if ((index == 0) && (head == tail)) {
					head->next = nullptr;
					head = nullptr;
					delete head;
					size = 0;
					return;
				}
			}

			if (index == size - 1) {
				tail = tail->prev;
				tail->next = nullptr;
				delete temp;
				size--;
				return;
			}

			temp->next->prev = temp->prev;
			temp->prev->next = temp->next;
			delete temp;
			size--;

			return;
		}
		index++;
		temp = temp->next;
	}
}

int makehash(int x) {
	return abs(x % 131071);
}

list hash_table[131071];

int main() {
	ifstream input("set.in");
	ofstream output("set.out");

	string command;
	int x, hash;
	while (input >> command) {
		input >> x;
		hash = makehash(x);
		if (command == "insert") {
			hash_table[hash].insert_node(x);
		}
		else {
			if (command == "exists") {
				if (hash_table[hash].isExist(x)) {
					output << "true" << endl;
				}
				else {
					output << "false" << endl;
				}
			}
			else { //delete
				hash_table[hash].delete_node(x);
			}
		}
	}

	return 0;
}