#pragma once

#include "Interfaces.h"

class Rect : public PhysObject , public GeoFig, public Printable, public BaseCObject {
	Vector2D m_coord;
	char* m_name;
	double m_width, m_height;
	double m_mass;

public:
	Rect(double t_x, double t_y, double t_width, double t_height, double t_mass);
	~Rect();

    double x();
    double y();
    double w();
    double h();

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
