using DreamApp.Data;
using DreamApp.Models;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using WbToken.AppDbContext;
using IHostingEnvironment = Microsoft.AspNetCore.Hosting.IHostingEnvironment;

namespace DreamApp.Controllers
{
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    [Route("api/[controller]")]
    [ApiController]
    public class ItemController : ControllerBase
    {
        public IHostingEnvironment hostingEnvironment;

        private readonly AppDb _context;
        public ItemController(AppDb db, IHostingEnvironment hosting)
        {
            _context = db;
            hostingEnvironment = hosting;
        }
        [HttpGet]
        public IActionResult GetAllItems()
        {
            var resul = _context.Item.ToList();
            return Ok(resul);

        }
        [HttpPost]
        public IActionResult AddItem([FromForm] Item items)
        {
            try
            {
                var files = HttpContext.Request.Form.Files;
                if (files != null && files.Count > 0)
                {
                    foreach (var file in files)
                    {
                        FileInfo fi = new FileInfo(file.FileName);
                        var newfilename = "Image_" + fi.Extension;
                        var path = Path.Combine("", hostingEnvironment.ContentRootPath + "\\ItemImages\\" + newfilename);
                        using (var stream = new FileStream(path, FileMode.Create))
                        {
                            file.CopyTo(stream);
                        }
                        var newitem = new Item()
                        {
                            ItemId = Guid.NewGuid(),
                            ItemName = items.ItemName,
                            ItemImage = path,
                            ItemPackage = items.ItemPackage,
                            CategoryId = items.CategoryId,
                            ItemPrice = items.ItemPrice,
                            ItemSize = items.ItemSize,
                            Lable = items.Lable
                        };
                        _context.Item.Add(newitem);
                        _context.SaveChanges();
                        return Ok("Added Successfully");
                    }
                }
                return Ok();

            }
            catch (Exception)
            {

                throw;
            }


        }
        [HttpGet]
        [Route("getbyid/{id:Guid}")]

        public IActionResult GetById(Guid id)
        {
            var result = _context.Item.Find(id);
            return Ok(result);

        }
        [HttpGet]
        [Route("getbyname")]
        public IActionResult GetByName(string ItemName)
        {
            var result = _context.Item.FirstOrDefault(x => x.ItemName == ItemName);
            return Ok(result);

        }
        [HttpGet]
        [Route("getbycategory")]
        public List<Item> GetByCategory(Guid id)
        {
            var result = _context.Item.Where(x => x.CategoryId == id).ToList();
            return (result);

        }
        [HttpPut]
        [Route("updateitem")]
        public IActionResult EditItem(Guid id, Item items)
        {
            var result = _context.Item.Find(id);
            if (result == null) { BadRequest("item not found"); }
            result.ItemName = items.ItemName;
            result.ItemPackage = items.ItemPackage;
            result.CategoryId = items.CategoryId;
            result.ItemPrice = items.ItemPrice;
            result.ItemSize = items.ItemSize;
            result.Lable = items.Lable;
            _context.SaveChanges();
            return Ok("updated successfully");

        }
        [HttpPost]
        [Route("addlable")]
        public IActionResult AddLable(Guid id, string lbl)
        {
            var result = _context.Item.Find(id);
            if (result == null) { BadRequest(); }
            result.Lable = lbl;
            _context.SaveChanges();
            return Ok("lable added");

        }
        [HttpDelete]
        [Route("deleteitem")]
        public IActionResult DeleteCategory(Guid id)
        {
            var result = _context.Item.Find(id);
            if (result == null) { return BadRequest("Category Not Found"); }
            _context.Remove(result);
            _context.SaveChanges();
            return Ok("Deleted Successfully");
        }

    }
}
