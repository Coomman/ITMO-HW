#include <fstream>
#include <string>
#include <vector>

using namespace std;

int main() {
	ifstream in("search1.in");
	ofstream out("search1.out");

	string pattern; in >> pattern;
	string text; in >> text;

	vector<int> index;
	if (pattern.length() <= text.length()) {
		for (int i = 0; i <= text.length() - pattern.length(); i++) {
			bool flag = true;
			for (int j = 0; j < pattern.length(); j++) {
				if (text[i + j] != pattern[j]) {
					flag = false;
					break;
				}
			}

			if (flag) {
				index.push_back(i + 1);
			}
		}
	}

	out << index.size() << endl;
	for (int i = 0; i < index.size(); i++) {
		out << index[i] << ' ';
	}
}