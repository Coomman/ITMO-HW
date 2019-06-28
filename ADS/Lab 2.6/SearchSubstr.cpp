#include <fstream>
#include <vector>
#include <string>
#include <cmath>

using namespace std;

typedef long long ll;

const int X = 47;
const ll k = LLONG_MAX;
const char til = '@';

string pattern, text;
int p_len, t_len;

ll makeHash(string str, vector<ll> &powers) {
	ll h = 0;
	for (int i = 0; i < (int)str.length(); i++) {
		h += ((str[i] - til) * powers[str.length() - i - 1]) % k;
	}
	return h;
}

vector<int> index;
void RabinKarp() {
	vector<ll> powers(p_len, 1);
	for (int i = 1; i < (int)powers.size(); i++) {
		powers[i] = powers[i - 1] * X;
	}

	ll pattern_hash = makeHash(pattern, powers);
	ll h = makeHash(text.substr(0, p_len), powers);

	ll power = powers[p_len - 1];
	powers.clear();
	for (int i = 0; i <= t_len - p_len; i++) {
		if (h == pattern_hash) {
			if (text.substr(i, p_len) == pattern) {
				index.push_back(i + 1);
			}
		}
		h = ((h - power * (text[i] - til)) * X + (text[i + p_len] - til)) % k;
	}
}

int main() {
	ifstream in("search2.in");
	ofstream out("search2.out");

	in >> pattern >> text;
	p_len = pattern.length(); t_len = text.length();

	if (p_len <= t_len) {
		RabinKarp();
	}

	out << index.size() << endl;
	for (int i : index) {
		out << i << ' ';
	}
}