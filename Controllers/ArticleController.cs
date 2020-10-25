using System.IO;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

using Orga.Controllers.Builders;
using Orga.Models;
using Orga.Repository;
using Orga.ViewModels;

namespace Orga.Controllers
{
    public class ArticleController : Controller
    {        
        private readonly MakeupDbContext _context;

        private readonly ArticleViewModelBuilder _articleViewModelBuilder;

        public ArticleController(MakeupDbContext context)
        {
            _context = context;
            _articleViewModelBuilder = new ArticleViewModelBuilder(context);

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
            return View(_articleViewModelBuilder.BuildArticleViewModel());
        }

        // POST: Article/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind(
            nameof(ArticleViewModel.Name),
            nameof(ArticleViewModel.PurchaseDate),
            nameof(ArticleViewModel.ImageFile),
            nameof(ArticleViewModel.BrandId))]
            ArticleViewModel articleViewModel)
        {
            if (ModelState.IsValid)
            {
                if (await BrandExists(articleViewModel.BrandId))
                {
                    ImageData image = null;

                    if (articleViewModel.ImageFile != null)
                    {
                        image = new ImageData
                        {
                            Data = await ExtractData(articleViewModel.ImageFile)
                        };
                    }
                    
                    await _context.AddAsync(new Article
                    {
                        Name = articleViewModel.Name,
                        PurchaseDate = articleViewModel.PurchaseDate,
                        BrandId = articleViewModel.BrandId,
                        Image = image
                    });

                    await _context.SaveChangesAsync();
                    return RedirectToAction(nameof(Index));
                }

                return NotFound();
            }
            
            return View(_articleViewModelBuilder.BuildArticleEditViewModel(articleViewModel));
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

            return View(_articleViewModelBuilder.BuildArticleEditViewModel(article));
        }

        // POST: Article/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind(
            nameof(ArticleEditViewModel.Id),
            nameof(ArticleEditViewModel.Name),
            nameof(ArticleEditViewModel.PurchaseDate),
            nameof(ArticleEditViewModel.ImageFile),
            nameof(ArticleEditViewModel.BrandId))]
            ArticleEditViewModel articleEditViewModel)
        {
            if (id != articleEditViewModel.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                // Connexion à l'article à modifier
                var article = await _context.Articles.Include(a => a.Image).FirstOrDefaultAsync(a => a.Id == id);

                if ((article != null) && (await BrandExists(articleEditViewModel.BrandId)))
                {
                    article.Name = articleEditViewModel.Name;
                    article.PurchaseDate = articleEditViewModel.PurchaseDate;
                    article.BrandId = articleEditViewModel.BrandId;

                    // Si l'image est ajoutée ou modifiée
                    if (articleEditViewModel.ImageFile != null)
                    {
                        article.Image.Data = await ExtractData(articleEditViewModel.ImageFile);
                    }

                    try
                    {
                        await _context.SaveChangesAsync();
                    }
                    catch (DbUpdateConcurrencyException)
                    {
                        if ((!await ArticleExists(articleEditViewModel.Id)) || (!await BrandExists(articleEditViewModel.BrandId)))
                        {
                            return NotFound();
                        }
                        else
                        {
                            throw;
                        }
                    }
                }
                else
                {
                    return NotFound();
                }
                
                return RedirectToAction(nameof(Details), new { Id = articleEditViewModel.Id });
            }

            return View(articleEditViewModel);
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
        /// Récupère l'article dont l'ID est passé en paramètre avec la marque associée
        /// </summary>
        /// <param name="id">l'ID de l'arcticle</param>
        /// <returns>l'article dont l'ID est passé en paramètre avec la marque associée</returns>
        private async Task<Article> ArticleWithBrand(int id) => await _context.Articles.Include(a => a.Brand).FirstOrDefaultAsync(a => a.Id == id);

        /// <summary>
        /// Extrait d'un fichier téléversé ses données
        /// </summary>
        /// <param name="file">Le fichier d'où extraire les données</param>
        /// <returns>Un tableau de byte contenant le binaire du fichier</returns>
        private static async Task<byte[]> ExtractData(IFormFile file)
        {
            using(Stream fileStream = file.OpenReadStream())
            {
                byte[] data = new byte[fileStream.Length];
                await fileStream.ReadAsync(data, 0, data.Length);
                return data;
            }
        }
    }
}
