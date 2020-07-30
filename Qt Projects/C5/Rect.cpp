#include "Rect.h"

Rect::Rect(double t_x, double t_y, double t_width, double t_height, double t_mass) :
	m_coord(Vector2D(t_x, t_y)), m_width(t_width), m_height(t_height), m_mass(t_mass) {
	m_name = new char[10];
	strcpy_s(m_name, 10, "Rectangle");
}

double Rect::x(){
    return m_coord.x;
}

double Rect::y(){
    return m_coord.y;
}

double Rect::w(){
    return m_width;
}

double Rect::h(){
    return m_height;
}

Rect::~Rect() {
	delete m_name;
}

double Rect::square() {
	return m_width * m_height;
}

double Rect::perimeter() {
	return 2 * (m_width + m_height);
}

double Rect::mass() {
	return m_mass;
}

Vector2D Rect::position() {
	return Vector2D(m_coord.x + m_width / 2, m_coord.y + m_height / 2);
}

const char* Rect::classname() {
	return m_name;
}

unsigned int Rect::size() {
	return sizeof(*this);
}

void Rect::draw() {
	cout << m_name << ":\n";
	cout << "Coordinates: X = " << m_coord.x << " Y = " << m_coord.y << endl;
	cout << "Width: " << m_width << endl;
	cout << "Height: " << m_height << endl;
	cout << "Mass: " << m_mass << endl;
	cout << "Square: " << square() << endl;
	cout << "Perimeter: " << perimeter() << endl;
	cout << "Center: X = " << position().x << " Y = " << position().y << endl;
	cout << "Size: " << size() << endl;
}

bool Rect::operator== (const PhysObject& ob) const {
	const Rect *o = dynamic_cast<const Rect*> (&ob);
	if (o == NULL) {
		return false;
	}
	return m_mass == o->m_mass;
}

bool Rect::operator< (const PhysObject& ob) const {
	const Rect *o = dynamic_cast<const Rect*> (&ob);
	if (o == NULL) {
		return false;
	}
	return m_mass < o->m_mass;
}
