#include "widget.h"
#include <QApplication>

#include <ctime>
#include <set>
#include <fstream>
#include <iostream>
#include <vector>
#include <string>
#include <cmath>
#include <QWidget>
#include <QImage>
#include <QPixmap>

using namespace std;

ifstream in("out.txt");
ofstream out("out.out");

void encode (string* arr){
    char ch;
    while(in >> ch){
        switch(ch){
        case 'a':
            out << arr[0];
            continue;
        case 'b':
            out << arr[1];
            continue;
        case 'c':
            out << arr[2];
            continue;
        case 'd':
            out << arr[3];
            continue;
        case 'e':
            out << arr[4];
            continue;
        case 'f':
            out << arr[5];
            continue;
        case 'g':
            out << arr[6];
            continue;
        case 'h':
            out << arr[7];
            continue;
        case 'i':
            out << arr[8];
            continue;
        case 'j':
            out << arr[9];
            continue;
        case 'k':
            out << arr[10];
            continue;
        case 'l':
            out << arr[11];
            continue;
        case 'm':
            out << arr[12];
            continue;
        }
    }
    out << endl;
    cout << "DONE";
}

int main(int argc, char *argv[])
{
    QApplication a(argc, argv);

//    srand(time(NULL));
//    QImage img(128, 128, QImage::Format_RGB32);
//    for(int i = 0; i < img.width(); i++){
//        for(int j = 0; j < img.height(); j++){
//            int r = rand()%(256);
//            QColor temp;
//            temp.setRgb(r,r,r);
//            img.setPixel(i, j, temp.rgb());
//        }
//    }
//    img.save("image.png");

    //QImage img("image.png");
    //ofstream out("out.txt");


//    set<int> pixels;
//    for(int i = 0;i<img.width();i++){
//        QColor temp = img.pixel(64, i);
//        int r = temp.red();
//        r = r/20*20;
//        temp.setRgb(r,r,r);
//        img.setPixel(64, i, temp.rgb());

//        pixels.insert(r);
//    }

//    copy(pixels.begin(), pixels.end(), ostream_iterator<int>(out, " "));
//    out << endl;

//    vector<int> freq(13);
//    for(int i = 0; i < img.width(); i++){
//        int j = 0;
//        QColor temp = img.pixel(64, i);
//        int r = temp.red();
//       for(set<int>::iterator it = pixels.begin(); it !=  pixels.end(); ++it){
//            if(r == *it){
//                freq[j]++;
//            }
//            j++;
//       }
//    }

//    for(int i=0;i<13;i++){
//        out << freq[i] << ' ';
//    }
//    out << endl << endl;

//    for(int i = 0; i < img.width(); i++){
//        QColor temp = img.pixel(64, i);
//        out << temp.red() << ' ';
//    }



//int n;
//while(in >> n){
//    switch(n){
//    case 0:
//        out << "a";
//        continue;
//    case 20:
//        out << "b";
//        continue;
//    case 40:
//        out << "c";
//        continue;
//    case 60:
//        out << "d";
//        continue;
//    case 80:
//        out << "e";
//        continue;
//    case 100:
//        out << "f";
//        continue;
//    case 120:
//        out << "g";
//        continue;
//    case 140:
//        out << "h";
//        continue;
//    case 160:
//        out << "i";
//        continue;
//    case 180:
//        out << "j";
//        continue;
//    case 200:
//        out << "k";
//        continue;
//    case 220:
//        out << "l";
//        continue;
//    case 240:
//        out << "m";
//        continue;
//    }
//}

//float ent = 0.0;
//int temp;
//while(in >> temp){
//    ent += (temp/128.0)*log2(temp/128.0);
//}
//out << -ent;

string bin [13] = {"0000", "0001", "0010", "0011", "0100", "0101", "0110", "0111", "1000", "1001", "1010", "1011", "1100"};
string sf [13] = {"1000", "0101", "0000", "001", "1010", "1011", "1100", "011", "0100", "1101", "111", "001", "1001"};
string haf [13] = {"0100", "1010", "0101", "1100", "1011", "0000", "1001", "011", "0001", "1000", "001", "111", "1101"};

//encode(bin);
encode(sf);
//encode(haf);

//float eff = 0.0;
//int temp;
//while(in >> temp){
//    if(temp == 16 || temp == 14 || temp == 12){
//        eff += temp/128.0*3;
//    }else{
//        eff += temp/128.0*4;
//    }
//}
//out << eff;

//otnos izbit
//out << 1-4/0.91/3.67188;
    Widget w;
    w.show();
    return a.exec();
}
