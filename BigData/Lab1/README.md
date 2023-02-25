# HW01: Hadoop

## Блок 1. Развертывание локального кластера Hadoop

За основу был взят fork репозитория с docker скриптами https://github.com/wxw-matt/docker-hadoop  
*docker-compose* в нём описан лучше, чем в оригинальной репе и имеется маппинг локальной папки в контейнер *namenode*

Получившийся [docker-compose file](docker-compose.yml) с конфигурацией ***1 NN, 2 DN + 2 NM, 1 RM, 1 HS***  
Также на этом этапе в *Dockerfile namenode* были добавлены шаги по установке *python* и *numpy*


## Блок 2. Написание map reduce на Python

Для подсчета средней цены и дисперсии была использована библиотека *pandas*  
Расчеты можно найти в [тетрадке](pandas.ipynb)

[Мапперы](mappers) и [редьюсеры](reducers) для соответствующих операций были перенесены на *namenode*  
Данные по ценам были размещены в *hdfs* с помощью команды ***hdfs dfs -put prices.txt /input***

***Map-reduce*** операция для подсчета среднего значения была запущена с помощью команды:  
*mapred streaming \  
-files mean_mapper.py,mean_reducer.py \  
-input /input/prices.txt \  
-output /output/mean \  
-mapper mean_mapper.py \  
-reducer mean_reducer.py*  

Аналогично для вычисления дисперсии


## Результаты

| Value    | pandas             | map-reduce         |
|----------|--------------------|--------------------|
| Mean     | 152.75505277800508 | 152.75505277800508 |
| Variance | 57681.75384084372  | 57680.573868790685 |

Потеря точности дисперсии при *map-reduce* объясняется итеративными вычислениями.  
В то же время среднее было подсчитано с помощью метода ***numpy.dot*** и поэтому не отличается.