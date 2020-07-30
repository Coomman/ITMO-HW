#ifndef WIDGET_H
#define WIDGET_H

#include <QMainWindow>
#include "Circle.h"
#include "Rect.h"

#include <vector>
#include <QPainter>
#include <QMouseEvent>

using namespace std;

namespace Ui {
class Widget;
}

class Widget : public QMainWindow
{
    Q_OBJECT

public:
    explicit Widget(QWidget *parent = 0);
    ~Widget();

    void paintEvent(QPaintEvent *);
	void mouseMoveEvent(QMouseEvent *event);
	void mousePressEvent(QMouseEvent *event);

	void incSquare(double s);
	void incPerimeter(double p);
	void incMemory(int memory);
	void relocateCenter();

private slots:
    void addRect();
	void addCircle();
	void changePaint();

private:
    Ui::Widget *ui;

	bool paintOn = true;
	double sumMass = 0;
	QPointF centerMass;

	vector<pair<Rect*, QColor>> m_rects;
	vector<pair<Circle*, QColor>> m_circles;
};

#endif // WIDGET_H
