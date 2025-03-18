using GoldSavings.App.Model;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text.Json;
using System.Threading.Tasks;
using System.Globalization;

namespace GoldSavings.App.Services
{
    public class GoldDataService
    {
        private readonly HttpClient _httpClient;
        private const string BASE_URL = "https://api.nbp.pl/api/cenyzlota";
        private const int MAX_DAYS_PER_REQUEST = 367; // NBP API często ogranicza do 1 roku (367 dni) na zapytanie

        public GoldDataService()
        {
            _httpClient = new HttpClient();
            _httpClient.DefaultRequestHeaders.Add("Accept", "application/json");
        }

        /// <summary>
        /// Pobiera ceny złota z API NBP dla podanego zakresu dat
        /// </summary>
        public async Task<List<GoldPrice>> GetGoldPrices(DateTime startDate, DateTime endDate)
        {

            try
            {
                Console.WriteLine("Using sample data for development/testing...");
                //return GetSampleGoldPrices();

                // Zakomentowano kod API, który można odkomentować po rozwiązaniu problemów

                var result = new List<GoldPrice>();

                // API NBP ma limit na ilość dni w jednym zapytaniu (zwykle 367 dni)
                // Dlatego dzielimy duże zakresy na mniejsze podzakresy
                var currentStartDate = startDate;

                while (currentStartDate <= endDate)
                {
                    var currentEndDate = currentStartDate.AddDays(MAX_DAYS_PER_REQUEST - 1);
                    if (currentEndDate > endDate)
                    {
                        currentEndDate = endDate;
                    }

                    var batchPrices = await GetGoldPricesBatch(currentStartDate, currentEndDate);
                    result.AddRange(batchPrices);

                    // Ustawiamy następny początek zakresu
                    currentStartDate = currentEndDate.AddDays(1);

                    // Dodajemy małe opóźnienie, aby nie przeciążać API
                    await Task.Delay(100);
                }

                return result;

            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error fetching gold prices: {ex.Message}");
                Console.WriteLine("Using sample data instead...");
                return GetSampleGoldPrices();
            }
        }

        private async Task<List<GoldPrice>> GetGoldPricesBatch(DateTime startDate, DateTime endDate)
        {
            try
            {
                string formattedStartDate = startDate.ToString("yyyy-MM-dd");
                string formattedEndDate = endDate.ToString("yyyy-MM-dd");

                string url = $"{BASE_URL}/{formattedStartDate}/{formattedEndDate}";
                Console.WriteLine($"Requesting data from: {url}");

                HttpResponseMessage response = await _httpClient.GetAsync(url);

                if (response.IsSuccessStatusCode)
                {
                    string jsonContent = await response.Content.ReadAsStringAsync();
                    var goldPriceData = JsonSerializer.Deserialize<List<NbpGoldPriceDto>>(
                        jsonContent,
                        new JsonSerializerOptions
                        {
                            PropertyNameCaseInsensitive = true
                        });

                    var result = new List<GoldPrice>();
                    foreach (var dto in goldPriceData)
                    {
                        // Używamy CultureInfo.InvariantCulture do parsowania daty
                        if (DateTime.TryParse(dto.Data, CultureInfo.InvariantCulture, DateTimeStyles.None, out var date))
                        {
                            result.Add(new GoldPrice
                            {
                                Date = date,
                                Price = dto.Cena
                            });
                        }
                        else
                        {
                            Console.WriteLine($"Failed to parse date: {dto.Data}");
                        }
                    }

                    return result;
                }
                else
                {
                    Console.WriteLine($"API returned status code: {response.StatusCode}");
                    Console.WriteLine($"Response content: {await response.Content.ReadAsStringAsync()}");
                    return new List<GoldPrice>();
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error in GetGoldPricesBatch: {ex.Message}");
                return new List<GoldPrice>();
            }
        }

        /// <summary>
        /// Zwraca przykładowe dane o cenach złota do testów
        /// </summary>
        private List<GoldPrice> GetSampleGoldPrices()
        {
            // Przykładowe dane dla lat 2019-2024 do testowania funkcjonalności
            return new List<GoldPrice>
            {
                // 2019
                new GoldPrice { Date = new DateTime(2019, 1, 2), Price = 157.88m },
                new GoldPrice { Date = new DateTime(2019, 3, 15), Price = 159.76m },
                new GoldPrice { Date = new DateTime(2019, 6, 20), Price = 166.55m },
                new GoldPrice { Date = new DateTime(2019, 9, 10), Price = 179.32m },
                new GoldPrice { Date = new DateTime(2019, 12, 5), Price = 180.45m },
                
                // 2020
                new GoldPrice { Date = new DateTime(2020, 1, 2), Price = 182.55m },
                new GoldPrice { Date = new DateTime(2020, 1, 15), Price = 186.34m },
                new GoldPrice { Date = new DateTime(2020, 1, 30), Price = 190.21m },
                new GoldPrice { Date = new DateTime(2020, 3, 16), Price = 206.19m },  // COVID spike
                new GoldPrice { Date = new DateTime(2020, 8, 6), Price = 243.28m },   // 2020 high
                new GoldPrice { Date = new DateTime(2020, 12, 10), Price = 223.15m },
                
                // 2021
                new GoldPrice { Date = new DateTime(2021, 1, 5), Price = 227.45m },
                new GoldPrice { Date = new DateTime(2021, 3, 30), Price = 212.87m },
                new GoldPrice { Date = new DateTime(2021, 7, 15), Price = 225.36m },
                new GoldPrice { Date = new DateTime(2021, 11, 5), Price = 229.91m },
                
                // 2022
                new GoldPrice { Date = new DateTime(2022, 1, 10), Price = 232.56m },
                new GoldPrice { Date = new DateTime(2022, 3, 8), Price = 280.43m },   // War in Ukraine spike
                new GoldPrice { Date = new DateTime(2022, 6, 20), Price = 256.18m },
                new GoldPrice { Date = new DateTime(2022, 10, 1), Price = 261.34m },
                new GoldPrice { Date = new DateTime(2022, 12, 15), Price = 268.72m },
                
                // 2023
                new GoldPrice { Date = new DateTime(2023, 2, 1), Price = 273.45m },
                new GoldPrice { Date = new DateTime(2023, 5, 10), Price = 276.83m },
                new GoldPrice { Date = new DateTime(2023, 8, 20), Price = 265.91m },
                new GoldPrice { Date = new DateTime(2023, 12, 1), Price = 282.45m },
                
                // 2024
                new GoldPrice { Date = new DateTime(2024, 1, 5), Price = 285.67m },
                new GoldPrice { Date = new DateTime(2024, 2, 10), Price = 308.22m },
                new GoldPrice { Date = new DateTime(2024, 3, 1), Price = 307.93m },
                new GoldPrice { Date = new DateTime(2024, 3, 15), Price = 305.44m }
            };
        }
    }


    // DTO dla odpowiedzi z API NBP
    public class NbpGoldPriceDto
    {
        public string Data { get; set; }
        public decimal Cena { get; set; }
    }
}