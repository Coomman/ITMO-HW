#include "widget.h"
#include "ui_widget.h"

Set set1(100);
Set set2(100);

int W = 100;
int INDENT = 200;

Widget::Widget(QWidget *parent) :
	QWidget(parent),
	ui(new Ui::Widget)
{
	ui->setupUi(this);
	first = &set1;
	second = &set2;

	this->setWindowTitle("Sets");
}

Widget::~Widget()
{
	delete ui;
}

void Widget::paintEvent(QPaintEvent *){
	QPainter painter(this);
	QPen pen(Qt::red);
	painter.setPen(pen);

	for(int i = 0; i < set1.size(); i++){
		QRect rect(i*W + 10, 10, W, W);
		painter.drawRect(rect);
		QString str; str.setNum(set1.set()[i]);
		painter.drawText(i*W + W/2, 10 + W/2, str);
	}

	pen.setColor(Qt::blue);
	painter.setPen(pen);
	for(int i = 0; i < set2.size(); i++){
		QRect rect(i*W + 10, INDENT + 10, W, W);
		painter.drawRect(rect);
		QString str; str.setNum(set2.set()[i]);
		painter.drawText(i*W + W/2, 10 + INDENT + W/2, str);
	}
}

void Widget::toFirst(){
	first = &set1;
	second = &set2;
}

void Widget::toSecond(){
	first = &set2;
	second = &set1;
}

void Widget::Add(){
	first->add(ui->add_field->text().toInt());
	repaint();
}

void Widget::Erase(){
	first->erase(ui->erase_field->text().toInt());
	repaint();
}

void Widget::Intersection(){
	first->intersection(second);
}

void Widget::Association(){
	first->association(second);
}

void Widget::AFA(){
	first->addFromSecond(second);
	repaint();
}

void Widget::EFA(){
	first->eraseFromSecond(second);
	repaint();
}



