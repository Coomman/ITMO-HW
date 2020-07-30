#include "Set.h"

#include <iostream>
using namespace std;

Set::Set(int size){
	m_set = new int[size];
	m_maxSize = size;
}

int Set::size() {
	return m_size;
}

int* Set::set() {
	return m_set;
}

int Set::doesExist(int l, int r, int n){
	if (r >= l) {
		int mid = l + (r - l) / 2;

		if (m_set[mid] == n) {
			return mid;
		}

		if (m_set[mid] > n) {
			return doesExist(l, mid - 1, n);
		}

		return doesExist(mid + 1, r, n);
	}

	return -1;
}

void Set::add(int n) {
	if (doesExist(0, m_size, n) == -1 && m_size != m_maxSize) {
		m_set[m_size] = n;
		for (int i = m_size; i > 0; i--) {
			if (m_set[i] < m_set[i - 1]) {
				swap(m_set[i], m_set[i - 1]);
			}
			else {
				break;
			}
		}

		m_size++;
	}
}

void Set::erase(int n) {
	int temp = doesExist(0, m_size, n);
	if (temp != -1) {
		for (int i = temp + 1; i < m_size; i++) {
			m_set[i - 1] = m_set[i];
			m_set[i] = 0;
		}

		m_size--;
	}
}

void Set::intersection(Set* s) {
	int max = (this->size() > s->size() ? this->size() : s->size());
	Set tempSet(max);

	int tempSize = 0;
	for (int i = 0; i < s->size(); i++) {
		if (tempSize != this->size()) {
			if (doesExist(0, this->size(), s->set()[i]) != -1) {
				tempSet.add(s->set()[i]);
				tempSize++;
			}
		}
		else {
			break;
		}
	}

	cout << "Intersection: ";
	tempSet.print();
}
void Set::association(Set* s) {
	Set tempSet(s->size() + this->size());

	for (int i = 0; i < m_size; i++) {
		tempSet.add(m_set[i]);
	}
	for (int i = 0; i < s->size(); i++) {
		tempSet.add(s->set()[i]);
	}
	
	cout << "Association: ";
	tempSet.print();
}

void Set::addFromSecond(Set* s) {
	for (int i = 0; i < s->size(); i++) {
		this->add(s->set()[i]);
	}
}

void Set::eraseFromSecond(Set* s) {
	for (int i = 0; i < s->size(); i++) {
		this->erase(s->set()[i]);
	}
}

void Set::print() {
	for (int i = 0; i < m_size; i++) {
		cout << m_set[i] << ' ';
	}
	cout << endl;
}

Set::~Set() {
	delete[] m_set;
}
