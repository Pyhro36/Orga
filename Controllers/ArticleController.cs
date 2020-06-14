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

            Article article = await ArticleWithBrand(id.Value);
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
                if (await BrandExists(article.BrandId))
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
            nameof(Article.Id),
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
                    if ((!await ArticleExists(article.Id)) || (!await BrandExists(article.BrandId)))
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

            var article = await ArticleWithBrand(id.Value);
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

        /// <summary>
        /// Cherche dans le contexte de DB si l'article dont l'ID est donné en paramètre existe
        /// </summary>
        /// <param name="id">l'ID de l'article</param>
        /// <returns>vrai si l'ID existe dans le contexte, faux sinon</returns>
        private async Task<bool> ArticleExists(int id) => await _context.Articles.AnyAsync(a => a.Id == id);

        /// <summary>
        /// Cherche dans le contexte de DB si la marque dont l'ID est donné en paramètre existe, ou retourne
        /// vrai si l'ID est null
        /// </summary>
        /// <param name="id">l'ID de la marque ou null</param>
        /// <returns>vrai si l'ID est null ou si la marque existe dans le contexte, faux sinon</returns>
        private async Task<bool> BrandExists(int? id)
        {
            if (id != null)
            {
                return await _context.Brands.AnyAsync(b => b.Id == id);
            }
            else
            {
                return true;
            }
        }

        /// <summary>
        /// Crée le Modèle de vue pour l'édition (ou la création) d'article en y incluant la liste des marques
        /// </summary>
        /// <param name="article">L'article à éditer, indiquer à null pour un nouvel article</param>
        /// <returns>Le modèle de vue pour l'édition (ou la création) d'article</returns>
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

        /// <summary>
        /// Récupère l'article dont l'ID est passé en paramètre avec la marque associée
        /// </summary>
        /// <param name="id">l'ID de l'arcticle</param>
        /// <returns>l'article dont l'ID est passé en paramètre avec la marque associée</returns>
        private async Task<Article> ArticleWithBrand(int id) => await _context.Articles.Include(a => a.Brand).FirstOrDefaultAsync(a => a.Id == id);
    }
}
