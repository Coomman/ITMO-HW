#include "matrix.h"
#include "ui_matrix.h"

#include <fstream>

ofstream out("out.out");

QImage orig("res/parrot.jpg");

QImage pic1("res/pic1.png");
QImage pic2("res/pic2.png");

QColor temp[3][3];

Matrix::Matrix(QWidget *parent) :
    QWidget(parent),
    ui(new Ui::Matrix)
{
    ui->setupUi(this);

    QImage pic(orig);
    this->myLinear(pic);
    pic.save("result/myLinear.png");

    MatrixProcessing(orig);

    module(pic1, pic2);
}

double k=0.24;

double Sharp[3][3]={{-k, k-1, -k},
                    {k-1, k+5, k-1},
                    {-k, k-1, -k}};

float Box[3][3]={{1/9.0, 1/9.0, 1/9.0},
                 {1/9.0, 1/9.0, 1/9.0},
                 {1/9.0, 1/9.0, 1/9.0}};

int GX[3][3]=   {{-1, -2, -1},
                {0, 0, 0},
                {1, 2, 1}};

int GY[3][3]=   {{-1, 0, 1},
                {-2, 0, 2},
                {-1, 0, 1}};

void blur(int &r, int &g, int &b){
     int red, green, blue;
     for(int k =0;k<3;k++){
         for(int l=0;l<3;l++){
             temp[k][l].getRgb(&red, &green, &blue);
             r+=red*Box[k][l];
             g+=green*Box[k][l];
             b+=blue*Box[k][l];
         }
     }
}

void sharp(int &r, int &g, int &b){
    int red, green, blue;
    for(int k =0;k<3;k++){
        for(int l=0;l<3;l++){
            temp[k][l].getRgb(&red, &green, &blue);
            r+=red*Sharp[k][l];
            g+=green*Sharp[k][l];
            b+=blue*Sharp[k][l];
        }
    }

    r = r > 255 ? 255 : (r < 0 ? 0 : r);
    g = g > 255 ? 255 : (g < 0 ? 0 : g);
    b = b > 255 ? 255 : (b < 0 ? 0 : b);
}

void Matrix::myLinear(QImage &img){
    for(int i = 0;i< img.width()/2;i++){
        for(int j = 0; j < img.height(); j++){
            QColor tempL = img.pixel(i,j);
            QColor tempR = img.pixel(img.width()- i - 1,j);
            int lr, lg, lb, rr, rg, rb;
            tempL.getRgb(&lr, &lg, &lb);
            tempR.getRgb(&rr, &rg, &rb);

            swap(lr,rr);
            swap(lg, rg);
            swap(lb, rb);

            tempL.setRgb(lr, lg, lb);
            tempR.setRgb(rr, rg, rb);
            img.setPixel(i,j,tempL.rgb());
            img.setPixel(img.width() - i - 1,j,tempR.rgb());
        }
    }
}

void myMatrix(int &r, int &g, int &b){
    int sumX = 0, sumY = 0;
    for(int k =0;k<3;k++){
        for(int l=0;l<3;l++){
            temp[k][l].getRgb(&r, &g, &b);
            int NC = (r + g + b)* 0.1 + 100;
            sumX += NC*GX[k][l];
            sumY += NC*GY[k][l];
        }
    }
    int  SUM = sqrt(sumX*sumX + sumY*sumY);
    r = g = b = SUM;
}

void (*matrix_filters[3])(int &r, int &g, int &b) = {blur, sharp, myMatrix};

void MatrixProcessing(QImage &pic){
    QImage img;
    for(int d = 0; d < 3; d++){
        img = pic;
        for(int i = 0; i< img.width(); i++){
            for(int j = 0; j< img.height(); j++){
                QColor result;

                temp[0][0] = img.pixel(abs(i-1),abs(j-1));
                temp[0][1] = img.pixel(i, abs(j-1));
                temp[1][0] = img.pixel(abs(i-1), j);
                temp[1][1] = img.pixel(i, j);
                if(i+1 == img.width()){
                    temp[0][2] = img.pixel(i-1, abs(j-1));
                    temp[1][2] = img.pixel(i-1, j);

                if(j + 1 == img.height()){
                    temp[2][0] = img.pixel(i-1, j-1);
                    temp[2][1] = img.pixel(i, j-1);
                    temp[2][2] = img.pixel(i-1, j-1);
               }else{
                   temp[2][0] = img.pixel(i-1, j+1);
                   temp[2][1] = img.pixel(i, j+1);
                   temp[2][2] = img.pixel(i-1, j+1);
               }
            }
                else{
                temp[0][2] = img.pixel(i+1, abs(j-1));
                temp[1][2] = img.pixel(i+1, j);

                    if(j + 1 == img.height()){
                     temp[2][0] = img.pixel(abs(i-1), j-1);
                     temp[2][1] = img.pixel(i, j-1);
                     temp[2][2] = img.pixel(abs(i-1), j-1);
                }else{
                    temp[2][0] = img.pixel(abs(i-1), j+1);
                    temp[2][1] = img.pixel(i, j+1);
                    temp[2][2] = img.pixel(abs(i-1), j+1);
                }
            }

            int r = 0, g = 0, b = 0;

            matrix_filters[d](r, g, b);

            result.setRgb(r, g, b);
            img.setPixel(i, j, result.rgb());
            }
        }
        img.save("result/matrix" + QString::number(d) + ".png");
    }
}

void module(QImage img1, QImage img2){
    int mode = 0;
    for(int i = 0; i< img1.width(); i++){
        for(int j = 0; j< img1.height(); j++){
            QColor temp1 = img1.pixel(i, j);
            QColor temp2 = img2.pixel(i, j);

            int r1, r2;
            r1 = temp1.red();
            r2 = temp2.red();

            mode+= abs(r1 - r2);
        }
    }
    out <<  mode << ' ' << img1.width() << ' ' << img1.height();

}

Matrix::~Matrix()
{
    delete ui;
}


