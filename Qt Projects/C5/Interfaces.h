#pragma once

#include <string.h>
#include <iostream>

using namespace std;

class GeoFig {
public:
	// �������.
	virtual double square() = 0;
	// ��������.
	virtual double perimeter() = 0;
};

class Vector2D {
public:
	double x, y;
	Vector2D(double t_x, double t_y) :
		x(t_x), y(t_y) {
	}
};

class PhysObject {
public:
	// �����, ��.
	virtual double mass() = 0;
	// ���������� ������ ����, �.
	virtual Vector2D position() = 0;
	// ��������� �� �����.
	virtual bool operator== (const PhysObject& ob) const = 0;
	// ��������� �� �����.
	virtual bool operator< (const PhysObject& ob) const = 0;
};

class Printable {
public:
	// ���������� �� ������ (�������� � ��������� ���� ��������� ������).
	virtual void draw() = 0;
};

class BaseCObject {
public:
	// ��� ������ (���� ������).
	virtual const char* classname() = 0;
	// ������ ���������� ������.
	virtual unsigned int size() = 0;
};
