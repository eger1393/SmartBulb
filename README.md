# SmartBulb
Решение для управления умными лампами от TPLink.

Было созданно для проверки гипотезы "умного" светогого будильника.

информация о апи TpLink была взята из https://github.com/DaveGut/TP-Link-SmartThings

Для запуска проекта исспользовать

```docker-compose build```

```docker-compose up```

Для работы с админкой зайти на http://localhost:8095/


Структура проекта:
```
Old - Первый прототип
├── SmartBulb.Data - Слой доступа в данным (модели и репозитории)
├── SmartBulb.TpLinkApi - Слой бизнес логики(так-же содержит в себе сервис по общению с серверами TpLink)
└── SmartBulb - Уровень представления 
```
```
Microservices - реализация на микросервисах
├── TpLinkApi - Сервис служащий для общения с серверами TpLink
│   ├── Service.TpLinkApi - Веб сервис, содержит в себе ручки для вызова
│   └── TpLinkApi.Implementation - Реализация логики по общению с TpLink
│
├── ScriptService - Сервис отвечающий за работу со скриптами(скрипт это последовательность действий по смене состояния лампы, может содержать в себе паузы)
│   ├── Service.Script - Веб сервис, содержит в себе ручки для вызова
│   └── Script.Data - Уровень доступа к данным (модели и репозитории)
│
├── HostedServices - Отвечает за запуск скриптов хранящихся в БД в нужное время
│
├── Gate - Общий шлюз, маршрутизирующий запросы на нужный сервис
│
└── Front - Здесь будет какой-то фронт

```
