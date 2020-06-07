using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Orga.Models;
using Orga.Repository;
using Orga.ViewModels;

namespace Orga.Controllers
{
    public class ArticleController : Controller
    {
        private readonly MakeupDbContext _context;

        public ArticleController(MakeupDbContext context)
        {
            _context = context;
        }

        // GET: Article
        public async Task<IActionResult> Index()
        {
            return View(await _context.Articles.Include(a => a.Brand).ToListAsync());
        }

        // GET: Article/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var article = await _context.Articles.Include(a => a.Brand).FirstOrDefaultAsync(a => a.Id == id);
            if (article == null)
            {
                return NotFound();
            }

            return View(article);
        }

        // GET: Article/Create
        public IActionResult Create()
        {
            return View(BuildArticleEditViewModel());
        }

        // POST: Article/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind(
            nameof(Article.Name),
            nameof(Article.PurchaseDate),
            nameof(Article.BrandId),
            Prefix = nameof(Article))] Article article)
        {
            if (ModelState.IsValid)
            {
                if (await _context.Brands.AnyAsync(b => b.Id == article.BrandId))
                {    
                        await _context.AddAsync(article);
                        await _context.SaveChangesAsync();
                        return RedirectToAction(nameof(Index));
                }
                
                return NotFound();
            }
            
            return View(BuildArticleEditViewModel(article));
        }

        // GET: Article/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var article = await _context.Articles.FindAsync(id);
            if (article == null)
            {
                return NotFound();
            }

            return View(BuildArticleEditViewModel(article));
        }

        // POST: Article/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind(
            nameof(Article.Name),
            nameof(Article.PurchaseDate),
            nameof(Article.BrandId),
            Prefix = nameof(Article))] Article article)
        {
            if (id != article.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(article);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!await ArticleExists(article.Id))
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

            return View(BuildArticleEditViewModel(article));
        }

        // GET: Article/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var article = await _context.Articles.Include(a => a.Brand).FirstOrDefaultAsync(m => m.Id == id);
            if (article == null)
            {
                return NotFound();
            }

            return View(article);
        }

        // POST: Article/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var article = await _context.Articles.FindAsync(id);
            _context.Articles.Remove(article);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private async Task<bool> ArticleExists(int id)
        {
            return await _context.Articles.AnyAsync(e => e.Id == id);
        }

        private ArticleEditViewModel BuildArticleEditViewModel(Article article = null)
        {
            var brandQuery = from b in _context.Brands
                             orderby b.Name
                             select b;

            return new ArticleEditViewModel
            {
                Article = article ?? new Article {
                    PurchaseDate = DateTime.Now
                },

                Brands = new SelectList(
                    items: brandQuery.AsNoTracking(),
                    dataValueField: nameof(Brand.Id),
                    dataTextField: nameof(Brand.Name),
                    selectedValue: article?.Id
                )
            };
        }
    }
}
