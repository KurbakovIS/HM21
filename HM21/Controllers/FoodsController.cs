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
using System.IO;
using Microsoft.AspNetCore.Hosting;
using System.Globalization;
using HM21.Entity.ModelDTO.Food;
using Newtonsoft.Json;

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
            return await _context.Foods.ToListAsync();
        }

        // GET: api/Foods/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Food>> GetFood(int id)
        {
            var food = await _context.Foods.FindAsync(id);

            if (food == null)
                return NotFound();

            return food;
        }

        // PUT: api/Foods/5
       
        [HttpPut("{id}")]
        public async Task<IActionResult> PutFood(int id, UpdateFoodDTO food)
        {
            if (id != food.Id)
                return BadRequest();


            var style = NumberStyles.Number | NumberStyles.AllowCurrencySymbol;
            var provider = new CultureInfo("ru-RU");

            var number = Decimal.Parse(food.Price.Replace('.', ','), style, provider);

            var foodentity = _mapper.Map<Food>(food);

            //_context.Entry(food).State = EntityState.Modified;

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
                if (!FoodExists(id))
                    return NotFound();
                else
                    throw;
            }

            return NoContent();
        }

        // POST: api/Foods
        [HttpPost]
        public async Task<ActionResult<Food>> CreateFood([FromForm ]CreateFoodDto food)
        {

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
                //using var db = new DataContext();
                //db.Foods.Add(
                //    new Food
                //    {
                //        Name = food.Name,
                //        Description = food.Description,
                //        Price = number,
                //        Img = $"img/uploads/{uniqueNameFile}"
                //    }
                //    );
                //db.SaveChanges();
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                return BadRequest(e.Message);
            }
            return Ok();


            //var foodEntity = _mapper.Map<Food>(food);
            //_context.Foods.Add(foodEntity);

            //await _context.SaveChangesAsync();

            //return Ok();
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
            var filePath = Path.Combine(hostingEnvironment.WebRootPath, Img);
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
            var uploads = Path.Combine(hostingEnvironment.WebRootPath, "img\\uploads");

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
