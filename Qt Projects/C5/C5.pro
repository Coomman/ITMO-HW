#-------------------------------------------------
#
# Project created by QtCreator 2019-03-12T23:24:36
#
#-------------------------------------------------

QT       += core gui

greaterThan(QT_MAJOR_VERSION, 4): QT += widgets

TARGET = C5
TEMPLATE = app


SOURCES += main.cpp\
        widget.cpp \
    Circle.cpp \
    Rect.cpp

HEADERS  += widget.h \
    Interfaces.h \
    Circle.h \
    Rect.h

FORMS    += widget.ui
QMAKE_CXXFLAGS += -std=c++11
