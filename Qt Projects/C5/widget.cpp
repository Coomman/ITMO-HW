#include "widget.h"
#include "ui_widget.h"

Widget::Widget(QWidget *parent) :
    QMainWindow(parent),
    ui(new Ui::Widget)
{
	ui->setupUi(this);
	this->setMouseTracking(true);
	ui->centralWidget->setMouseTracking(true);

	setWindowTitle("LongNose");
	setWindowIcon(QIcon("./images/icon.png"));
}

Widget::~Widget()
{
	m_rects.clear();
	m_circles.clear();
    delete ui;
}

void Widget::paintEvent(QPaintEvent *){
	if(paintOn){
		QPainter painter(this);
		QPen pen(10);

		for(pair<Rect*,QColor>  i : m_rects){
			pen.setColor(i.second);
			painter.setPen(pen);

			painter.drawRect(i.first->x(), i.first->y(), i.first->w(), i.first->h());
			QString mass = QString::number(i.first->mass());
			painter.drawText(i.first->position().x - mass.size()*5, i.first->position().y + mass.size()*2, mass);
		}

		for(pair<Circle*,QColor>  i : m_circles){
			pen.setColor(i.second);
			painter.setPen(pen);

			painter.drawEllipse(QRectF(i.first->x() - i.first->r(), i.first->y() - i.first->r(), i.first->r()*2, i.first->r()*2));
			QString mass = QString::number(i.first->mass());
			painter.drawText(i.first->position().x - mass.size()*5, i.first->position().y + mass.size()*2, mass);
		}

		pen.setColor(Qt::black);
		pen.setWidth(10);
		painter.setPen(pen);
		painter.drawPoint(centerMass);
	}
}

void Widget::mouseMoveEvent(QMouseEvent *event){
	ui->mouse_coord->setText(QString::number(event->x()) + " ; " + QString::number(event->y()));
}

void Widget::mousePressEvent(QMouseEvent *event){
	QString x = QString::number(event->x());
	QString y = QString::number(event->y());
	ui->rect_x->setText(x);
	ui->rect_y->setText(y);
	ui->circle_x->setText(x);
	ui->circle_y->setText(y);

	if(ui->rect_check->isChecked()){
		addRect();
	}

	if(ui->circle_check->isChecked()){
		addCircle();
	}
}

void Widget::incPerimeter(double p){
	double per = ui->perimeter->text().toDouble() + p;
	ui->perimeter->setText(QString::number(per));
}

void Widget::incSquare(double s){
	double squ = ui->square->text().toDouble() + s;
	ui->square->setText(QString::number(squ));
}

void Widget::incMemory(int memory){
	int mem = ui->memory->text().toInt() + memory;
	ui->memory->setText(QString::number(mem));
}

void Widget::relocateCenter(){
	qreal x = 0, y = 0;
	for(pair<Rect*,QColor>  i : m_rects){
		x += i.first->position().x*i.first->mass();
		y += i.first->position().y*i.first->mass();
	}

	for(pair<Circle*,QColor>  i : m_circles){
		x += i.first->x()*i.first->mass();
		y += i.first->y()*i.first->mass();
	}

	centerMass.setX(x/=sumMass);
	centerMass.setY(y/=sumMass);
	ui->center->setText(QString::number(x) + " ; " + QString::number(y));
}

void Widget::addRect(){
	Rect* r = new Rect(ui->rect_x->text().toDouble(), ui->rect_y->text().toDouble(), ui->rect_w->text().toDouble(), ui->rect_h->text().toDouble(), ui->rect_mass->text().toDouble());
	m_rects.push_back(make_pair(r, QColor(rand()%256, rand()%256, rand()%256)));
	sumMass+=r->mass();
	incPerimeter(r->perimeter());
	incSquare(r->square());
	incMemory(r->size());
	relocateCenter();

    repaint();
}

void Widget::addCircle(){
	Circle* c = new Circle(ui->circle_x->text().toDouble(), ui->circle_y->text().toDouble(), ui->circle_r->text().toDouble(), ui->circle_mass->text().toDouble());
	m_circles.push_back(make_pair(c, QColor(rand()%256, rand()%256, rand()%256)));
	sumMass += c->mass();
	incPerimeter(c->perimeter());
	incSquare(c->square());
	incMemory(c->size());
	relocateCenter();

	repaint();
}

void Widget::changePaint(){
	paintOn = ui->paintOn->isChecked();
	repaint();
}
