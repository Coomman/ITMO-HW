#pragma once

#include <string.h>
#include <iostream>

using namespace std;

class GeoFig {
public:
	// Площадь.
	virtual double square() = 0;
	// Периметр.
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
	// Масса, кг.
	virtual double mass() = 0;
	// Координаты центра масс, м.
	virtual Vector2D position() = 0;
	// Сравнение по массе.
	virtual bool operator== (const PhysObject& ob) const = 0;
	// Сравнение по массе.
	virtual bool operator< (const PhysObject& ob) const = 0;
};

class Printable {
public:
	// Отобразить на экране (выводить в текстовом виде параметры фигуры).
	virtual void draw() = 0;
};

class BaseCObject {
public:
	// Имя класса (типа данных).
	virtual const char* classname() = 0;
	// Размер занимаемой памяти.
	virtual unsigned int size() = 0;
};
