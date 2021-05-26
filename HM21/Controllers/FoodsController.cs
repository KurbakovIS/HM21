using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using HM21.Entity;
using HM21.Entity.Model;
using HM21.Entity.ModelDTO;
using AutoMapper;
using Microsoft.AspNetCore.Hosting;
using System.Globalization;
using HM21.Entity.ModelDTO.Food;
using Newtonsoft.Json;
using System.IO;

namespace HM21.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class FoodsController : ControllerBase
    {
        private readonly DbContextFood _context;
        private readonly IMapper _mapper;
        private readonly IWebHostEnvironment hostingEnvironment;
        public FoodsController(IWebHostEnvironment environment,DbContextFood context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
            hostingEnvironment = environment;
        }

        // GET: api/Foods
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Food>>> GetFoods()
        {
            var foods = await _context.Foods.ToListAsync();
            foreach(var food in foods)
                food.Img = GetFile(food.Img);
            return foods;
        }

        // GET: api/Foods/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Food>> GetFood(int id)
        {
            var food = await _context.Foods.FindAsync(id);
            food.Img = GetFile(food.Img);
            if (food == null)
                return NotFound();

            return food;
        }

        // PUT: api/Foods/5
       
        [HttpPut]
        public async Task<IActionResult> PutFood( [FromForm]UpdateFoodDTO food)
        {
            if (!FoodExists(food.Id))
                return NotFound();

            var foodentity =await _context.Foods.FindAsync(food.Id);
            _mapper.Map(food, foodentity);

            if (food.Img != null)
            {
                var uniqueNameFile = await RecordImg(food.Img);
                foodentity.Img = $"img/uploads/{uniqueNameFile}";
            }

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                return BadRequest();
            }

            return NoContent();
        }

        // POST: api/Foods
        [HttpPost]
        public async Task<ActionResult<Food>> CreateFood([FromForm]CreateFoodDto food)
        {
            if(food.Name == null)
                return NotFound();

            var uniqueNameFile = "";
            var style = NumberStyles.Number | NumberStyles.AllowCurrencySymbol;
            var provider = new CultureInfo("ru-RU");

            if (food.Img != null)
                uniqueNameFile = await RecordImg(food.Img);

            try
            {
                var number = Decimal.Parse(food.Price.Replace('.', ','), style, provider);

                var foodEntity = _mapper.Map<Food>(food);
                foodEntity.Img = $"img/uploads/{uniqueNameFile}";
                _context.Foods.Add(foodEntity);

                await _context.SaveChangesAsync();
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                return BadRequest(e.Message);
            }
            return Ok();
        }

        // DELETE: api/Foods/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteFood(int id)
        {
            var food = await _context.Foods.FindAsync(id);
            if (food == null)
                return NotFound();

            DeleteImg(food.Img);
            _context.Foods.Remove(food);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool FoodExists(int id)
        {
            return _context.Foods.Any(e => e.Id == id);
        }

        /// <summary>
        /// Удаляет картинку
        /// </summary>
        /// <param name="Img">название картинки</param>
        async void DeleteImg(string Img)
        {
            var filePath = Path.Combine(hostingEnvironment.ContentRootPath, Img);
            FileInfo img = new FileInfo(filePath);
            await img.DeleteAsync();
        }
        /// <summary>
        /// Копирует в папку изображение пришедшее с клиента
        /// </summary>
        /// <param name="Img">изображение с клиента</param>
        /// <returns>уникальное название изображения</returns>
        async Task<string> RecordImg(IFormFile Img)
        {
            var uploads = Path.Combine(hostingEnvironment.ContentRootPath, "img\\uploads");

            if (!Directory.Exists(uploads))
                Directory.CreateDirectory(uploads);

            string uniqueNameFile = GetUniqueFileName(Img.FileName);
            var filePath = Path.Combine(uploads, uniqueNameFile);
            FileStream neImg = new FileStream(filePath, FileMode.Create);
            await Img.CopyToAsync(neImg);
            neImg.Close();

            return uniqueNameFile;
        }

        /// <summary>
        /// Присваивает уникальное имя файлу
        /// </summary>
        /// <param name="FileName">Название файла</param>
        /// <returns>Уникальное названеифайла</returns>
        string GetUniqueFileName(string FileName)
        {
            FileName = Path.GetFileName(FileName);

            return Path.GetFileNameWithoutExtension(FileName)
                      + "_" + Guid.NewGuid().ToString().Substring(0, 4)
                      + Path.GetExtension(FileName);
        }

        /// <summary>
        /// Находит файл в системе и конвертирует его в base64
        /// </summary>
        /// <param name="fileName"></param>
        /// <returns></returns>
         string GetFile(string fileName)
        {
            var base64String = "";
            var path = Path.Combine(hostingEnvironment.ContentRootPath, fileName);
            if (System.IO.File.Exists(path))
            {
                var mas = System.IO.File.ReadAllBytes(path);
                base64String = Convert.ToBase64String(mas, 0, mas.Length);
            }
           
            return "data:image/png;base64," + base64String;
        }
    }
    /// <summary>
    /// Расширение FileExtensions для ассинхронного удаления
    /// </summary>
    public static class FileExtensions
    {
        /// <summary>
        /// ассинхронное удаление файла
        /// </summary>
        /// <param name="fi">удаляемый файл</param>
        /// <returns></returns>
        public static Task DeleteAsync(this FileInfo fi)
        {
            return Task.Factory.StartNew(() => fi.Delete());
        }
    }



}
