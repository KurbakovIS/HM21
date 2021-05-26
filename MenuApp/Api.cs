using MenuApp.Entity.Models;
using MenuApp.Entity.ModelsDto;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace MenuApp
{
    class Api
    {
        private HttpClient HttpClient { get; set; }

        public Api()
        {
            HttpClient = new HttpClient();
        }
        /// <summary>
        /// Создать позицию
        /// </summary>
        /// <param name="food"></param>
        public void AddFood(FoodForCreationDto food)
        {
            string url = @"http://localhost:5000/api/Foods/wpf";
            _ = HttpClient.PostAsync(
                requestUri: url,
                content: new StringContent(JsonConvert.SerializeObject(food), Encoding.UTF8,
                mediaType: "application/json")
                ).Result;

        }

        public void UpdateFood(FoodForUpdateDto food)
        {
            string url = @"http://localhost:5000/api/Foods/wpf";
            _ = HttpClient.PutAsync(
                requestUri: url,
                content: new StringContent(JsonConvert.SerializeObject(food), Encoding.UTF8,
                mediaType: "application/json")
                ).Result;

        }

        public void DeleteFood(int id)
        {
            string url = $"http://localhost:5000/api/Foods/wpf/{id}";
            _ = HttpClient.DeleteAsync(
                requestUri: url).Result;

        }

        /// <summary>
        /// Получить позиции меню
        /// </summary>
        /// <returns></returns>
        public IEnumerable<Food> GetFoods()
        {
            string url = @"http://localhost:5000/api/Foods";

            string json = HttpClient.GetStringAsync(url).Result;
            Console.WriteLine(json);
            return JsonConvert.DeserializeObject<IEnumerable<Food>>(json);

        }
    }
}
