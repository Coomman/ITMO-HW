#include <fstream>
#include <string>

using namespace std;

int makeHash(string x) {
	int h = 0;
	for (int i = 0; i < (int)x.length(); i++) {
		h += x[i] * (i + 1);
	}
	return h % 1000;
}

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
	void put(string key, string value, int hashx);
	void delete_Node(string key, string value, int hashx);
	void delete_by_ptr(Node* temp);
};

struct Table {
	List list[1000];
public:
	void get_all(string key, ofstream &out, int hashx);
	void delete_all(string key, int hashx);
};

Node* last_put = nullptr;
long sizex[1000];

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

void List::put(string key, string value, int hashx) {
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
	sizex[hashx]++;
}

void List::delete_Node(string key, string value, int hashx) {
	Node* temp = head;

	while (temp != nullptr) {
		if (temp->key == key && temp->value == value) {
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
			if (temp->key == last_put->key && temp->value == last_put->value) {
				last_put = last_put->prev_put;
			}
			delete temp;
			sizex[hashx]--;
			return;
		}
		else {
			temp = temp->next;
		}
	}
}

void Table::get_all(string key, ofstream &out, int hashx) {
	Node* temp = last_put;
	out << sizex[hashx];
	while (temp != nullptr) {
		if (temp->key == key) {
			out << ' ' << temp->value;
		}
		temp = temp->prev_put;
	}
	out << endl;
}

void List::delete_by_ptr(Node* temp) {
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
	if (temp->key == last_put->key && temp->value == last_put->value) {
		last_put = last_put->prev_put;
	}
	delete temp;
	return;
}

void Table::delete_all(string key, int hashx) {
	Node* temp = last_put;
	Node* p;
	while (temp != nullptr) {
		if (temp->key == key) {
			p = temp->prev_put;
			int hash = makeHash(key);
			list[hash].delete_by_ptr(temp);
			temp = p;
		}
		else {
			temp = temp->prev_put;
		}
	}
	sizex[hashx] = 0;
}

Table hash_table[1000];

int main() {
	ifstream input("multimap.in");
	ofstream output("multimap.out");
	
	string str;
	string command;
	string key;
	string value;
	int hashx, hashy;
	
	while (getline(input, str, '\n')) {
		command = str.substr(0, str.find(' '));
		str.erase(0, str.find(' ') + 1);
		key = str.substr(0, str.find(' '));
		hashx = makeHash(key);
		if (command == "put") {
			value = str.substr(str.find(' ') + 1, str.size() - str.find(' ') - 1);
			hashy = makeHash(value);
			hash_table[hashx].list[hashy].put(key, value, hashx);
		}
		else if (command == "delete") {
			value = str.substr(str.find(' ') + 1, str.size() - str.find(' ') - 1);
			hashy = makeHash(value);
			hash_table[hashx].list[hashy].delete_Node(key, value, hashx);
		}
		else if (command == "deleteall") {
			hash_table[hashx].delete_all(key, hashx);
		}
		else { // get_all
			hash_table[hashx].get_all(key, output, hashx);
		}
	}
}