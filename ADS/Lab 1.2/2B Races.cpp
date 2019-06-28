#include <iostream>
#include <vector>
#include <fstream>
#include <algorithm>
#include <string>
using namespace std;

struct pret {
	string m_country;
	string m_name;
};

int main() {
	ifstream input("race.in");
	ofstream output("race.out");
	int size;
	input >> size;

	vector<pret> preds(size);

	for (int i = 0; i < size; i++) {
		input >> preds[i].m_country;
		input >> preds[i].m_name;
	}
	
	stable_sort(preds.begin(), preds.end(), [](const pret& a, const pret& b) {
		return a.m_country < b.m_country; });

	string tempcountry="";
	for (int i = 0; i < size; i++) {
		if (tempcountry != preds[i].m_country) {
			output << "=== " << preds[i].m_country << " ===" << endl;
			tempcountry = preds[i].m_country;
		}
		output << preds[i].m_name<<endl;
	}

}