import { randomString, uuidv4 } from 'https://jslib.k6.io/k6-utils/1.2.0/index.js';
import { check, sleep } from 'k6';
import http from 'k6/http';

export let options = {
    stages: [
        { duration: '30s', target: 10 },  // медленный рост до 10 пользователей
        { duration: '1m', target: 50 },   // стабильная нагрузка с 50 пользователями
        { duration: '30s', target: 0 },   // снижение нагрузки
    ],
};

const BASE_URL = 'http://localhost:5000/api/notifications';

export default function () {
    // Генерация случайного уведомления для отправки
    let payload = JSON.stringify({
        title: randomString(10),
        message: randomString(20),
        recipient: randomString(10) + '@example.com',
    });

    let params = {
        headers: { 'Content-Type': 'application/json' },
    };

    // Тестирование POST-запроса на отправку уведомления
    let sendRes = http.post(`${BASE_URL}/send?channelType=email`, payload, params);
    check(sendRes, {
        'POST /send status is 200': (r) => r.status === 200,
    });

    // Тестирование PATCH-запроса для обновления уведомления
    let updatePayload = JSON.stringify({
        title: 'Updated ' + randomString(5),
        message: randomString(15),
    });
    let updateRes = http.patch(`${BASE_URL}/updateById?id=${uuidv4()}`, updatePayload, params);
    check(updateRes, {
        'PATCH /updateById status is 200 or 404': (r) => r.status === 200 || r.status === 404,
    });

    // Тестирование GET-запроса для получения уведомления по ID
    let getRes = http.get(`${BASE_URL}/getById?id=${uuidv4()}`);
    check(getRes, {
        'GET /getById status is 200 or 404': (r) => r.status === 200 || r.status === 404,
    });

    // Тестирование GET-запроса для получения всех уведомлений
    let getAllRes = http.get(`${BASE_URL}/all`);
    check(getAllRes, {
        'GET /all status is 200': (r) => r.status === 200,
    });

    sleep(1);
}
