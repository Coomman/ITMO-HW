#include <fstream>
#include <vector>
#include <cmath>
#include <stack>

using namespace std;

int v, begMood, totalVars;
int meetCount = 0;

struct Meeting {
	int minMood;
	int maxMood;
	int moodChange;

	Meeting(int a, int b, int c) {
		minMood = a;
		maxMood = b;
		moodChange = c;
	}
};

vector<Meeting> meet;
vector<bool> pos;
vector<int> total_change;

bool doesExist(int mask, int num) {
	return (mask & (1 << (num - 1))) > 0;
}

int add(int mask, int num) {
	return mask | (1 << (num - 1));
}

int erase(int mask, int num) {
	return mask ^ (1 << (num - 1));
}

int count(int mask) {
	int res = 0;
	while (mask) {
		res += (mask & 1);
		mask = mask >> 1;
	}
	return res;
}

void changeCount(int mask) {
	int temp = mask;
	for (int i = 1; i <= v; i++) {
		if (temp & 1) {
			total_change[mask] += meet[i].moodChange;
		}

		temp = temp >> 1;
		if (!temp) {
			break;
		}
	}
}

int main() {
	ifstream in("meetings.in");
	ofstream out("meetings.out");

	in >> v >> begMood;
	totalVars = (int)pow(2, v);
	meet.push_back(Meeting(0, 0, 0));

	for (int i = 1; i <= v; i++) {
		int a, b, c; in >> a >> b >> c;
		meet.push_back(Meeting(a, b, c));
	}

	pos.resize(totalVars);
	pos[0] = true;
	total_change.resize(totalVars);

	vector<int> last(totalVars);
	for (int m = 0; m < totalVars; m++) {
		if (!pos[m]) {
			continue;
		}

		changeCount(m);
		for (int i = 1; i <= v; i++) {
			if (doesExist(m, i)) {
				continue;
			}

			if (begMood + total_change[m] < meet[i].minMood) {
				continue;
			}

			if (begMood + total_change[m] > meet[i].maxMood) {
				continue;
			}

			pos[add(m, i)] = true;
			last[add(m, i)] = i;
		}

		if (count(m) > count(meetCount)) {
			meetCount = m;
		}
	}

	out << count(meetCount) << endl;

	int mask = meetCount;
	stack<int> order;
	for (int i = count(mask); i > 0; i--) {
		order.push(last[mask]);
		mask = erase(mask, last[mask]);
	}

	while (!order.empty()) {
		out << order.top() << ' ';
		order.pop();
	}

}