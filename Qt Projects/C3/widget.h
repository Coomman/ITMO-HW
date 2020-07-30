#ifndef WIDGET_H
#define WIDGET_H

#include <QWidget>
#include <QPainter>

#include "Set.h"
using namespace std;

namespace Ui {
	class Widget;
}

class Widget : public QWidget
{
	Q_OBJECT

public:
	explicit Widget(QWidget *parent = 0);
	~Widget();

	void paintEvent(QPaintEvent *);

private slots:
	void toFirst();
	void toSecond();

	void Add();
	void Erase();
	void Intersection();
	void Association();
	void AFA();
	void EFA();
private:
	Ui::Widget *ui;

	Set* first;
	Set* second;
};

#endif // WIDGET_H
