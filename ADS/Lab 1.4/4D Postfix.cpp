#include <iostream>
#include <fstream>
#include <string>

using namespace std;

int main() {
	ifstream input("postfix.in");
	ofstream output("postfix.out");

	int  numbers[100];
	char ch;
	int temp, p = 0;
	while (input >> ch) {
		switch (ch) {
			case '+':
				p--;
				temp = numbers[p - 1];
				temp += numbers[p];
				numbers[p - 1] = temp;
				continue;
			case '-':
				p--;
				temp = numbers[p - 1];				
				temp -= numbers[p];
				numbers[p - 1] = temp;
				continue;
			case '*':
				p--;
				temp = numbers[p - 1];
				temp *= numbers[p];
				numbers[p - 1] = temp;
				continue;
			default:
				numbers[p] = static_cast<int>(ch - '0');
				p++;
				continue;
		}
	}

	output << numbers[p - 1];
}