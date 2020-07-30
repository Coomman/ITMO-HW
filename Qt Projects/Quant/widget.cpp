#include "widget.h"

Widget::Widget(QWidget *parent)
    : QWidget(parent)
{
    //srand(time(NULL));
    //QImage img(128, 128, QImage::Format_RGB32);
    //QImage img("image.png");
    //ofstream out("out.txt");

//    for(int i = 0;i<img.width();i++){
//        for(int j = 0; j<img.height();j++){
//            int r = rand()%(256);
//            QColor temp;
//            temp.setRgb(r,r,r);
//            img.setPixel(i, j, temp.rgb());
//        }
//    }
//    img.save("image.png");



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

//int str;
//while(in >> str){
//    switch(str){
//    case 0:
//        out << "a";
//        out << ' ';
//        continue;
//    case 20:
//        out << "b";
//        out << ' ';
//        continue;
//    case 40:
//        out << "c";
//        out << ' ';
//        continue;
//    case 60:
//        out << "d";
//        out << ' ';
//        continue;
//    case 80:
//        out << "e";
//        out << ' ';
//        continue;
//    case 100:
//        out << "f";
//        out << ' ';
//        continue;
//    case 120:
//        out << "g";
//        out << ' ';
//        continue;
//    case 140:
//        out << "h";
//        out << ' ';
//        continue;
//    case 160:
//        out << "i";
//        out << ' ';
//        continue;
//    case 180:
//        out << "j";
//        out << ' ';
//        continue;
//    case 200:
//        out << "k";
//        out << ' ';
//        continue;
//    case 220:
//        out << "l";
//        out << ' ';
//        continue;
//    case 240:
//        out << "m";
//        out << ' ';
//        continue;
//    }
//}

//float ent = 0.0;
//int temp;
//while(in >> temp){
//    ent += (temp/128.0)*log2(temp/128.0);
//}
//out << -ent;

//char str;
//while(in >> str){
//    switch(str){
//    case 'a':
//        out << "0000";
//        continue;
//    case 'b':
//        out << "0001";
//        continue;
//    case 'c':
//        out << "0010";
//        continue;
//    case 'd':
//        out << "0011";
//        continue;
//    case 'e':
//        out << "0100";
//        continue;
//    case 'f':
//        out << "0101";
//        continue;
//    case 'g':
//        out << "0110";
//        continue;
//    case 'h':
//        out << "0111";
//        continue;
//    case 'i':
//        out << "1000";
//        continue;
//    case 'j':
//        out << "1001";
//        continue;
//    case 'k':
//        out << "1010";
//        continue;
//    case 'l':
//        out << "1011";
//        continue;
//    case 'm':
//        out << "1100";
//        continue;
//    }
//}

//while(in >> str){
//    switch(str){
//    case 'a':
//        out << "1000";
//        continue;
//    case 'b':
//        out << "0101";
//        continue;
//    case 'c':
//        out << "0000";
//        continue;
//    case 'd':
//        out << "0001";
//        continue;
//    case 'e':
//        out << "1010";
//        continue;
//    case 'f':
//        out << "1011";
//        continue;
//    case 'g':
//        out << "1100";
//        continue;
//    case 'h':
//        out << "011";
//        continue;
//    case 'i':
//        out << "0100";
//        continue;
//    case 'j':
//        out << "1101";
//        continue;
//    case 'k':
//        out << "111";
//        continue;
//    case 'l':
//        out << "001";
//        continue;
//    case 'm':
//        out << "1001";
//        continue;
//    }
//}

//char str;
//while(in >> str){
//    switch(str){
//    case 'a':
//        out << "0100";
//        continue;
//    case 'b':
//        out << "1010";
//        continue;
//    case 'c':
//        out << "0101";
//        continue;
//    case 'd':
//        out << "1100";
//        continue;
//    case 'e':
//        out << "1011";
//        continue;
//    case 'f':
//        out << "0000";
//        continue;
//    case 'g':
//        out << "1001";
//        continue;
//    case 'h':
//        out << "011";
//        continue;
//    case 'i':
//        out << "0001";
//        continue;
//    case 'j':
//        out << "1000";
//        continue;
//    case 'k':
//        out << "001";
//        continue;
//    case 'l':
//        out << "111";
//        continue;
//    case 'm':
//        out << "1101";
//        continue;
//    }
//}

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

//out << 1-4/0.91/3.67188;
}


Widget::~Widget(){

}
