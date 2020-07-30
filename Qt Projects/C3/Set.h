#pragma once

#include <iostream>
using namespace std;

class Set{
	int m_size = 0;
	int m_maxSize;
	int* m_set;
	
public:
	Set(int size);
	~Set();

	void add(int n);
	void erase(int n);
	int doesExist(int l, int r, int n);

	void intersection(Set* s);
	void association(Set* s);
	void addFromSecond(Set* s);
	void eraseFromSecond(Set* s);

	int size();
	int* set();
	void print();
};
