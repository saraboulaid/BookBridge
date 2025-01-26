using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using UserApp.Data;
using UserApp.Models;

namespace UserApp.Controllers
{
    public class BooksController : Controller
    {
        private readonly AppDbContext _context;
        private readonly IWebHostEnvironment _webHostEnvironment;

        public BooksController(AppDbContext context, IWebHostEnvironment webHostEnvironment)
        {
            _context = context;
            _webHostEnvironment = webHostEnvironment;
        }

        // GET: Books
        public async Task<IActionResult> Index(string searchField, string searchTerm, string genreFilter, string availabilityFilter)
        {
            var books = from b in _context.Book
                        select b;

            // Recherche
            if (!string.IsNullOrEmpty(searchTerm) && !string.IsNullOrEmpty(searchField))
            {
                switch (searchField)
                {
                    case "Title":
                        books = books.Where(b => b.Title.Contains(searchTerm));
                        break;
                    case "Author":
                        books = books.Where(b => b.Author.Contains(searchTerm));
                        break;
                    case "PublicationDate":
                        if (DateOnly.TryParse(searchTerm, out var searchDate))
                        {
                            books = books.Where(b => b.PublicationDate == searchDate);
                        }
                        break;
                }
            }

            // Filtrage par genre
            if (!string.IsNullOrEmpty(genreFilter))
            {
                books = books.Where(b => b.Genre == Enum.Parse<Genre>(genreFilter));
            }

            // Filtrage par disponibilité
            if (availabilityFilter == "Disponible")
            {
                books = books.Where(b => b.NumberOfLoans < b.NumberOfCopies);
            }
            else if (availabilityFilter == "Indisponible")
            {
                books = books.Where(b => b.NumberOfLoans >= b.NumberOfCopies);
            }

            return View(await books.ToListAsync());
        }

        // GET: Books/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var book = await _context.Book
                .FirstOrDefaultAsync(m => m.Id == id);
            if (book == null)
            {
                return NotFound();
            }

            return View(book);
        }

        // GET: Books/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Books/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Title,Author,Genre,PublicationDate,NumberOfCopies,NumberOfLoans")] Book book, IFormFile imageFile)
        {
            if (imageFile != null)
            {
                // Sauvegarde de l'image localement
                string uploadsFolder = Path.Combine(_webHostEnvironment.WebRootPath, "images");
                string uniqueFileName = Guid.NewGuid().ToString() + "_" + imageFile.FileName;
                string filePath = Path.Combine(uploadsFolder, uniqueFileName);
                using (var fileStream = new FileStream(filePath, FileMode.Create))
                {
                    await imageFile.CopyToAsync(fileStream);
                }
                book.ImagePath = "/images/" + uniqueFileName;
            }
            _context.Add(book);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        // GET: Books/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var book = await _context.Book.FindAsync(id);
            if (book == null)
            {
                return NotFound();
            }
            return View(book);
        }

        // POST: Books/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Title,Author,Genre,PublicationDate,NumberOfCopies,NumberOfLoans")] Book book, IFormFile imageFile)
        {
            if (id != book.Id)
            {
                return NotFound();
            }

            try
                {
                // Récupérer l'ancien livre
                var oldBook = await _context.Book.AsNoTracking().FirstOrDefaultAsync(l => l.Id == id);

                if (imageFile != null)
                {
                    // Supprimer l'ancienne image si elle existe
                    if (!string.IsNullOrEmpty(oldBook.ImagePath))
                    {
                        string oldImagePath = Path.Combine(_webHostEnvironment.WebRootPath, oldBook.ImagePath.TrimStart('/'));
                        if (System.IO.File.Exists(oldImagePath))
                        {
                            System.IO.File.Delete(oldImagePath);
                        }
                    }

                    // Sauvegarde de la nouvelle image
                    string uploadsFolder = Path.Combine(_webHostEnvironment.WebRootPath, "images");
                    string uniqueFileName = Guid.NewGuid().ToString() + "_" + imageFile.FileName;
                    string filePath = Path.Combine(uploadsFolder, uniqueFileName);
                    using (var fileStream = new FileStream(filePath, FileMode.Create))
                    {
                        await imageFile.CopyToAsync(fileStream);
                    }
                    book.ImagePath = "/images/" + uniqueFileName;
                }
                else
                {
                    book.ImagePath = oldBook.ImagePath; // Conserver l'ancienne image si aucune nouvelle n'est fournie
                }
                _context.Update(book);
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!BookExists(book.Id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return RedirectToAction(nameof(Index));
            
        }

        // GET: Books/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var book = await _context.Book
                .FirstOrDefaultAsync(m => m.Id == id);
            if (book == null)
            {
                return NotFound();
            }

            return View(book);
        }

        // POST: Books/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var book = await _context.Book.FindAsync(id);
            if (book != null)
            {
                // Supprimer l'image associée si elle existe
                if (!string.IsNullOrEmpty(book.ImagePath))
                {
                    string imagePath = Path.Combine(_webHostEnvironment.WebRootPath, book.ImagePath.TrimStart('/'));
                    if (System.IO.File.Exists(imagePath))
                    {
                        System.IO.File.Delete(imagePath);
                    }
                }
                _context.Book.Remove(book);
                await _context.SaveChangesAsync();
            }

            return RedirectToAction(nameof(Index));
        }

        private bool BookExists(int id)
        {
            return _context.Book.Any(e => e.Id == id);
        }
    }
}
