# Тестовое задание ATON
## Описание 📝
Данный проект является заданием для стажера по направлению «C#-разработка» в компанию ATON.  
Основная задача созданного сервиса - работа с информацией о пользователях.
## Быстрый старт (с Docker) ⚡️
1. Склонируйте данный репозиторий с помощью команды ```git clone```. (На самом деле, для тестирования системы достаточно скопировать docker-compose.yaml и папку Config, а не весь проект. При желании можно так и сделать)  
2. После клонирования репозитория (или копирования файла и папки) через терминал перейдите в директорию с файлом docker-compose.yaml. 
> Переход к папке делается с помощью команды ```cd your_path_to_folder```
3. В директории с docker-compose.yaml выполните команду: ```docker-compose --env-file Config/.env up -d```.  
Данная команда запустит проект. 
> [!IMPORTANT]
> Для корректного запуска необходимо иметь на устройстве установленный и запущенный Docker. А также нужно, чтобы папка Config находилась в одной директории с docker-compose. Если папка Config находится не в той же директории, то в команде необходимо указать путь до того места, где она находится.
4. Перейдите на ```https://localhost:8081/swagger/index.html```. Перед вам откроется интерфейс Swagger для доступа к API. 
> [!IMPORTANT]
> Очень важно перейти именно на ```https``` версию, поскольку для авторизации используется JWT токен и Cookies.
5. Для тестирования системы используйте данные аккаунта администратора:  
login: ```aton```  
password: ```admin```
6. При необходимости остановки контейнеров в папке с docker-compose.yaml используйте команду: ```docker-compose --env-file Config/.env down```
## Стек 🛠️
- .NET
- Entity Framework
- PostgreSQL
- Docker
- JWT
- Swagger
## Архитектура и паттерны 👷‍♂️
Организация кода соответсвует Layer Architecture (Слоистой архитектуре). Проект разделен на слои:
- Application - основная логика и сервисы
- Core - общие для всего проекта классы и интерфейсы.
- DataAccess - слой, инкапсулирующий работу с БД.
- Infrastructure - поддержка и инфраструктура для работы приложения.
- WebApi - представление данных и взаимодействие с пользователем через API  

Если упоминать паттерны, то в основном был применен паттерн Repository в слое взаимодействия с БД.