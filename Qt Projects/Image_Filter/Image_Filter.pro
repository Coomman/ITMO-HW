#-------------------------------------------------
#
# Project created by QtCreator 2018-11-14T22:39:55
#
#-------------------------------------------------

QT       += core gui

greaterThan(QT_MAJOR_VERSION, 4): QT += widgets

TARGET = Image_Filter
TEMPLATE = app


SOURCES += main.cpp\
        widget.cpp \
    matrix.cpp

HEADERS  += widget.h \
    matrix.h

FORMS    += widget.ui \
    matrix.ui

CONFIG += c++11\
    widget.cpp\
