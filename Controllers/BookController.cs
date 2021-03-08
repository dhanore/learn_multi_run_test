using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using yoyo_web_app.Filters;
using yoyo_web_app.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace yoyo_web_app.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    //[Authorize]
    public class BookController : ControllerBase
    {
        private readonly DBContext _context;

        public BookController(DBContext context)
        {
            _context = context;
        }

        private bool BookExists(long id) =>
                _context.Books.Any(e => e.Id == id);

        private static BookDto GetBookDto(Book Book) =>
            new BookDto
            {
                Id = Book.Id,
                Name = Book.Name,
                InStock = Book.InStock
            };

        // GET: api/<BookController>
        [HttpGet]
        public async Task<List<BookDto>> GetAll()
        {
            var items = await _context.Books
                                .Select(x => GetBookDto(x))
                                .ToListAsync();
            return items;
        }

        // GET api/<BookController>/5
        [HttpGet("{id}")]
        public async Task<ActionResult<BookDto>> Get(long id)
        {
            var Book = await _context.Books.FindAsync(id);

            if (Book == null)
            {
                return NotFound();
            }

            return GetBookDto(Book);
        }

        // POST api/<BookController>
        [HttpPost]
        public async Task<ActionResult<BookDto>> Create(BookDto BookDto)
        {
            var Book = new Book
            {
                InStock = BookDto.InStock,
                Name = BookDto.Name
            };

            _context.Books.Add(Book);
            await _context.SaveChangesAsync();

            return CreatedAtAction(
                nameof(Get),
                new { id = Book.Id },
                GetBookDto(Book));
        }

        // PUT api/<BookController>/5
        [HttpPut("{id}")]
        public async Task<IActionResult> Update(long id, BookDto BookDto)
        {
            if (id != BookDto.Id)
            {
                return BadRequest();
            }

            var Book = await _context.Books.FirstOrDefaultAsync(o => o.Id == id);
            if (Book == null)
            {
                return NotFound();
            }

            Book.Name = BookDto.Name;
            Book.InStock = BookDto.InStock;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException) when (!BookExists(id))
            {
                return NotFound();
            }

            return NoContent();
        }

        // DELETE api/<BookController>/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(long id)
        {
            var Book = await _context.Books.FirstOrDefaultAsync(o => o.Id == id);

            if (Book == null)
            {
                return NotFound();
            }

            _context.Books.Remove(Book);
            await _context.SaveChangesAsync();

            return NoContent();
        }

    }
}
