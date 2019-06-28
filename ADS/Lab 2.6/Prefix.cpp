#include <fstream>
#include <vector>
#include <string>

using namespace std;

vector<int> prefix;
void buildPrefix(string &str) {
	for (int i = 1; i < (int)str.length(); i++) {
		int k = prefix[i - 1];
		while (k > 0 && str[i] != str[k]) {
			k = prefix[k - 1];
		}

		if (str[i] == str[k]) {
			k++;
		}

		prefix[i] = k;
	}
}

int main() {
	ifstream in("prefix.in");
	ofstream out("prefix.out");

	string str; in >> str;
	prefix.resize(str.length());
	buildPrefix(str);

	for (int i = 0; i < (int)str.length(); i++) {
		out << prefix[i] << ' ';
	}
}