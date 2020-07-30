#pragma once

#include "Interfaces.h"

class Circle : public PhysObject, public GeoFig, public Printable, public BaseCObject {
	Vector2D m_coord;
	char* m_name;
	double m_radius;
	double m_mass;

public:
	Circle(double t_x, double t_y, double t_radius, double t_mass);
	~Circle();

	double x();
	double y();
	double r();

	//GeoFig
	double square() override;
	double perimeter() override;

	//PhysObject
	double mass() override;
	Vector2D position() override;
	bool operator== (const PhysObject& ob) const override;
	bool operator< (const PhysObject& ob) const override;

	//BaseCObject
	const char* classname() override;
	unsigned int size() override;

	//Printable
	void draw() override;
};
