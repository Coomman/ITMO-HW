#include "Circle.h"
constexpr auto M_PI = 3.14159265358979323846;

Circle::Circle(double t_x, double t_y, double t_radius, double t_mass) :
	m_coord(Vector2D(t_x, t_y)), m_radius(t_radius), m_mass(t_mass) {
	m_name = new char[7];
	strcpy_s(m_name, 7, "Circle");
}

Circle::~Circle() {
	delete m_name;
}

double Circle::x(){
	return m_coord.x;
}

double Circle::y(){
	return m_coord.y;
}

double Circle::r(){
	return m_radius;
}

double Circle::square() {
	return M_PI * m_radius * m_radius / 2;
}

double Circle::perimeter() {
	return 2 * M_PI * m_radius;
}

double Circle::mass() {
	return m_mass;
}

Vector2D Circle::position() {
	return m_coord;
}

const char* Circle::classname() {
	return m_name;
}

unsigned int Circle::size() {
	return sizeof(*this);
}

void Circle::draw() {
	cout << m_name << ":\n";
	cout << "Coordinates: X = " << m_coord.x << " Y = " << m_coord.y << endl;
	cout << "Radius: " << m_radius << endl;
	cout << "Mass: " << m_mass << endl;
	cout << "Square: " << square() << endl;
	cout << "Perimeter: " << perimeter() << endl;
	cout << "Center: X = " << position().x << " Y = " << position().y << endl;
	cout << "Size: " << size() << endl;
}

bool Circle::operator== (const PhysObject& ob) const {
	const Circle *o = dynamic_cast<const Circle*> (&ob);
	if (o == NULL) {
		return false;
	}
	return m_mass == o->m_mass;
}

bool Circle::operator< (const PhysObject& ob) const {
	const Circle *o = dynamic_cast<const Circle*> (&ob);
	if (o == NULL) {
		return false;
	}
	return m_mass < o->m_mass;
}
