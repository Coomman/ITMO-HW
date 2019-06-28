#include <iostream>
#include <fstream>

using namespace std;

int main() {
	ifstream input("queue.in");
	ofstream output("queue.out");

	int count; input >> count;
	int* queue = new int[count];
	int head = 0, tail = 0;

	char tempsym;
	for (int i = 0; i < count; i++) {
		input >> tempsym;
		if (tempsym == '+') {
			input >> queue[tail];
			tail++;
			if (tail == count - 1) {
				tail = 0;
			}
		}
		else {
			output << queue[head] << endl;
			head++;
			if (head == count - 1) {
				head = 0;
			}
		}
	}

	
}