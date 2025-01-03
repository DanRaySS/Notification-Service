using System.Collections.Concurrent;
using System.Diagnostics;
using System.Text;

namespace SMS_Service.Tests
{
    public class LoadTest
    {
        static async Task Main(string[] args)
        {
            const int numberOfRequests = 100; // Общее количество запросов
            const int degreeOfParallelism = 10; // Одновременные потоки
            var url = "https://sms-api.example.com/send"; // URL вашего SMS API
            var payloadTemplate = new
            {
                to = "Номер_получателя", // Тестовый номер
                message = "Тестовое сообщение" // Тестовое сообщение
            };

            var client = new HttpClient();
            var stopwatch = new Stopwatch();
            var results = new ConcurrentBag<(bool success, string response)>();

            Console.WriteLine($"Начало нагрузочного тестирования: {DateTime.Now}");

            stopwatch.Start();

            // SemaphoreSlim ограничивает параллельность
            using var semaphore = new SemaphoreSlim(degreeOfParallelism);

            var tasks = new Task[numberOfRequests];
            for (int i = 0; i < numberOfRequests; i++)
            {
                await semaphore.WaitAsync(); // Ждем, пока не освободится поток

                tasks[i] = Task.Run(async () =>
                {
                    try
                    {
                        var payload = new StringContent(
                            Newtonsoft.Json.JsonConvert.SerializeObject(payloadTemplate),
                            Encoding.UTF8,
                            "application/json"
                        );

                        var response = await client.PostAsync(url, payload);
                        var responseString = await response.Content.ReadAsStringAsync();

                        results.Add((response.IsSuccessStatusCode, responseString));
                    }
                    catch (Exception ex)
                    {
                        results.Add((false, ex.Message));
                    }
                    finally
                    {
                        semaphore.Release(); // Освобождаем поток
                    }
                });
            }

            await Task.WhenAll(tasks);

            stopwatch.Stop();

            Console.WriteLine($"Тест завершен: {DateTime.Now}");
            Console.WriteLine($"Общее время: {stopwatch.Elapsed.TotalSeconds} секунд");
            Console.WriteLine($"Успешных запросов: {results.Count(r => r.success)}");
            Console.WriteLine($"Неудачных запросов: {results.Count(r => !r.success)}");

            // Вывод первых 10 результатов
            Console.WriteLine("\nПримеры ответов:");
            foreach (var result in results.Take(10))
            {
                Console.WriteLine($"Успех: {result.success}, Ответ: {result.response}");
            }
        }
    }
}