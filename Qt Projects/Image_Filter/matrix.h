#ifndef MATRIX_H
#define MATRIX_H

#include <QWidget>
#include <cmath>

using namespace std;

namespace Ui {
class Matrix;
}

class Matrix : public QWidget
{
    Q_OBJECT

public:
    explicit Matrix(QWidget *parent = 0);

    void myLinear(QImage &img);
    ~Matrix();
private:
    Ui::Matrix *ui;
};

void MatrixProcessing(QImage &pic);
void module(QImage img1, QImage img2);

#endif // MATRIX_H
