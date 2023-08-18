using DreamApp.Data;
using DreamApp.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using WbToken.AppDbContext;

namespace DreamApp.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CategoryController : ControllerBase
    {
        private readonly AppDb _context;
        public CategoryController(AppDb db)
        {
            _context = db;
        }
        [HttpGet]
        public IActionResult GetAllCategories()
        {
            var result = _context.Category.ToList();

            return Ok(result);
        }
        [HttpGet]
        [Route("getbyid/{id:Guid}")]
        public IActionResult GetById([FromRoute] Guid id)
        {
            var result = _context.Category.Find(id);

            return Ok(result);
        }
        [HttpPost]
        public IActionResult Addcategory([FromForm] CategoryDto category)
        {
            using var datastream = new MemoryStream();
            category.CategoryImage.CopyTo(datastream);
            var newcategory = new Category()
            {
                CategoryId = Guid.NewGuid(),
                CategoryName = category.CategoryName,
                CategoryImage = datastream.ToArray(),

            };
            _context.Category.Add(newcategory);
            _context.SaveChanges();
            return Ok(newcategory);

        }
        [HttpPut]
        [Route("Update/{id:Guid}")]
        public IActionResult EditCategory(CategoryDto category, [FromRoute] Guid id)
        {
            using var datastream = new MemoryStream();
            category.CategoryImage.CopyTo(datastream);
            var result = _context.Category.Find(id);
            if (result == null) { return BadRequest("not founf"); }
            result.CategoryName = category.CategoryName;
            result.CategoryImage = datastream.ToArray();
            _context.SaveChanges();
            return Ok("Updated successfully");


        }
        [HttpDelete]
        public IActionResult DeleteCategory(Guid id)
        {
            var result = _context.Category.Find(id);
            if (result == null) { return BadRequest("Category Not Found"); }
            _context.Remove(result);
            _context.SaveChanges();
            return Ok("Deleted Successfully");
        }

    }
}
