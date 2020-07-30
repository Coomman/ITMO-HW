#ifndef WIDGET_H
#define WIDGET_H

#include "matrix.h"
#include <QWidget>
#include <ctime>

namespace Ui {
class Widget;
}

class Widget : public QWidget
{
    Q_OBJECT

public:
    explicit Widget(QWidget *parent = 0);
    ~Widget();
private:
    Ui::Widget *ui;
};

void LinearProcessing(QImage &pic);

#endif // WIDGET_H
