#include <iostream>
#include <fstream>
#include <string>

using namespace std;

int main() {
	ifstream input("brackets.in");
	ofstream output("brackets.out");

	string str;
	bool flag = true;

	while (getline(input, str)) {
		char* brack_stack = new char[str.length() + 1];
		flag = true;
		int p = 0;
		for (int i = 0; i < str.length(); i++) {
			if (str[i] == '(' || str[i] == '[') {
				brack_stack[p] = str[i];
				p++;
			}
			else {
				if (str[i] == ')' && brack_stack[p - 1] == '(' || str[i] == ']' && brack_stack[p - 1] == '[') {
					p--;
					continue;
				}
				flag = false;
				break;
			}
		}

		if (flag && p == 0) {
			output << "YES" << endl;
		}
		else {
			output << "NO" << endl;
		}
	}
	
}