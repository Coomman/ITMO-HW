#include <iostream>
#include <vector>
#include <fstream>

using namespace std;

int main() {
	ifstream input("stack.in");
	ofstream output("stack.out");
	int count; input >> count;
	char tempsyb;
	int tempnum;
	int* stack = new int[count];
	for (int i = 0; i < count; i++) {
		input >> tempsyb;
		if (tempsyb == '+') {
			input >> tempnum;
			*stack = tempnum;
			stack++;
		}
		else {
			stack--;
			output << *stack << endl;
		}
	}
}