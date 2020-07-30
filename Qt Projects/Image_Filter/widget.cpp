#include "widget.h"
#include "ui_widget.h"

Widget::Widget(QWidget *parent) :
    QWidget(parent),
    ui(new Ui::Widget)
{
    ui->setupUi(this);

    QImage orig("res/parrot.jpg");
    LinearProcessing(orig);

    Matrix* m = new Matrix;
    delete m;
}

void redComp(int &r, int &g, int &b){
     r = r;
     g = 0;
     b = 0;
}

void greenComp(int &r, int &g, int &b){
     r = 0;
     g = g;
     b = 0;
}

void blueComp(int &r, int &g, int &b){
     r = 0;
     g = 0;
     b = b;
}

void grayScale(int &r, int &g, int &b){
     r *= 0.299;
     g *= 0.587;
     b *= 0.114;
     r = g = b = r + g + b;
}

void sepia(int &r, int &g, int &b){
     int blue = r*0.272 + g*0.534 + b*0.131;
     int green = r*0.349 + g*0.686 + b*0.168;
     int red = r*0.393 + g*0.769 + b*0.189;
     blue = blue > 255 ? 255 : (blue < 0 ? 0 : blue);
     green = green > 255 ? 255 : (green < 0 ? 0 : green);
     red = red > 255 ? 255 : (red < 0 ? 0 : red);

     r = red;
     g = green;
     b = blue;
}

void sepiaTurquoise(int &r, int &g, int &b){
     int red = r*0.272 + g*0.534 + b*0.131;
     int blue = r*0.349 + g*0.686 + b*0.168;
     int green = r*0.393 + g*0.769 + b*0.189;
     blue = blue > 255 ? 255 : (blue < 0 ? 0 : blue);
     green = green > 255 ? 255 : (green < 0 ? 0 : green);
     red = red > 255 ? 255 : (red < 0 ? 0 : red);

     r = red;
     g = green;
     b = blue;
}

void invert(int &r, int &g, int &b){
     r = 255 - r;
     g = 255 - g;
     b = 255 - b;
}

void noise(int &r, int &g, int &b){
     int intence = 25;

     int temp_noise = -intence + rand()%(2*intence + 1);
     r += temp_noise;
     temp_noise = -intence + rand()%(2*intence + 1);
     g += temp_noise;
     temp_noise = -intence + rand()%(2*intence + 1);
     b += temp_noise;

     r = r > 255 ? 255 : (r < 0 ? 0 : r);
     g = g > 255 ? 255 : (g < 0 ? 0 : g);
     b = b > 255 ? 255 : (b < 0 ? 0 : b);
}

void BrightHigh(int &r, int &g, int &b){
     int bright = 65;
     r += bright;
     g += bright;
     b += bright;

     r = r > 255 ? 255 : (r < 0 ? 0 : r);
     g = g > 255 ? 255 : (g < 0 ? 0 : g);
     b = b > 255 ? 255 : (b < 0 ? 0 : b);
}

void BrightLow(int &r, int &g, int &b){
     int bright = -65;
     r += bright;
     g += bright;
     b += bright;

     r = r > 255 ? 255 : (r < 0 ? 0 : r);
     g = g > 255 ? 255 : (g < 0 ? 0 : g);
     b = b > 255 ? 255 : (b < 0 ? 0 : b);
}


void black_white(int &r, int &g, int &b){
    if((r + g + b)/3 > 113){
        r = g = b = 255;
    }else{
        r = g = b = 0;
    }
}

void (*linear_filters[11])(int &r, int &g, int &b) = {redComp, greenComp, blueComp, grayScale, sepia, sepiaTurquoise, invert, noise, BrightHigh, BrightLow, black_white};

void LinearProcessing(QImage &pic){
    for(int d = 0; d < 11; d++){
        QImage img(pic);

        for(int i = 0; i < img.width(); i++){
            for(int j = 0; j < img.height(); j++){
                QColor temp = img.pixel(i,j);
                int r, g, b;
                temp.getRgb(&r, &g, &b);

                linear_filters[d](r, g, b);

                temp.setRgb(r, g, b);
                img.setPixel(i, j, temp.rgb());
            }
        }
        img.save("result/linear" + QString::number(d) + ".png");
    }
}

Widget::~Widget()
{
    delete ui;
}
