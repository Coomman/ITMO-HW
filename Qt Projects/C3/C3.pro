#-------------------------------------------------
#
# Project created by QtCreator 2019-04-13T18:37:19
#
#-------------------------------------------------

QT       += core gui

greaterThan(QT_MAJOR_VERSION, 4): QT += widgets

TARGET = C3
TEMPLATE = app


SOURCES += main.cpp\
        widget.cpp \
    Set.cpp

HEADERS  += widget.h \
    Set.h

FORMS    += widget.ui


QMAKE_CXXFLAGS += -std=c++11
