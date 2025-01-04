# Notification-Service
<h2>Запуск</h2>
<ul> 
  В коренной папке с файлом <b>docker-compose.yml</b> :
  <li>docker compose up -d</li>
  Демонстрация выполнения отправки уведомлений на различные источники реализована через <b>Swagger</b> :
  <li>http://localhost:7001/swagger/index.html</li>
  <b>RabbitMq</b> :
  <li>http://localhost:15672/#/queues</li>
</ul>
<h2>Технические детали сервиса:</h2>
<ol>
  <li>Сервис развёрнут в контейнерах (Docker)</li>
  <li>Микросервисы масштабируемы и устойчивы к сбоям</li>
  <li>Уведомления отправляются через все каналы с возможностью добавления новых (в SMS-Service реализована логика, но не заплачен тариф)</li>
  <li>Реализована поддержка повторов и логирование операций</li>
</ol>
<h2>Дополнительная информация:</h2>
<ol>
  <li>channelTypes: Email, SMS, All ...</li>
  <li>В проекте стоят дефолтные подключения к бд : Server=postgres;User Id=postgres;Password=postgres;Database=NotificationServiceDB</li>
</ol>
