using System;
using System.Linq;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;

using Orga.Models;
using Orga.Repository;
using Orga.ViewModels;

namespace Orga.Controllers.Builders
{
    internal class ArticleViewModelBuilder
    {
        private readonly MakeupDbContext _context;
        
        internal ArticleViewModelBuilder(MakeupDbContext context)
        {
            this._context = context;
        }

        /// <summary>
        /// Récupère la liste des marques du contexte
        /// </summary>
        /// <returns>Le modèle de vue d'article</returns>
        private IQueryable<Brand> GetBrandList()
        {
            var brandQuery = from b in _context.Brands
                             orderby b.Name
                             select b;

            return brandQuery.AsNoTracking();
        }

        /// <summary>
        /// Crée le Modèle de vue d'article en y incluant la liste des marques
        /// </summary>
        /// <returns>Le modèle de vue d'article</returns>
        internal ArticleViewModel BuildArticleViewModel()
        {
            return new ArticleViewModel
            {
                PurchaseDate = DateTime.Now,
                
                Brands = new SelectList(
                    items: GetBrandList(),
                    dataValueField: nameof(Brand.Id),
                    dataTextField: nameof(Brand.Name),
                    selectedValue: null
                )
            };
        }

        /// <summary>
        /// Crée le Modèle de vue d'édition d'un article en y incluant la liste des marques
        /// </summary>
        /// <param name="article">L'article à éditer</param>
        /// <returns>Le modèle de vue de l'article à éditer</returns>
        internal ArticleEditViewModel BuildArticleEditViewModel(Article article)
        {
            return new ArticleEditViewModel
            {
                Name = article.Name,
                PurchaseDate = article.PurchaseDate,
                ImageId = article.ImageId,

                Brands = new SelectList(
                    items: GetBrandList(),
                    dataValueField: nameof(Brand.Id),
                    dataTextField: nameof(Brand.Name),
                    selectedValue: null
                )
            };
        }

        /// <summary>
        /// Crée le Modèle de vue d'édition d'un article en y incluant la liste des marques
        /// </summary>
        /// <param name="articleViewModel">Lun modèle de vue portant l'article à éditer</param>
        /// <returns>Le modèle de vue de l'article à éditer</returns>
        internal ArticleEditViewModel BuildArticleEditViewModel(ArticleViewModel articleViewModel)
        {
            return new ArticleEditViewModel
            {
                Name = articleViewModel.Name,

                PurchaseDate = articleViewModel.PurchaseDate,

                Brands = new SelectList(
                    items: GetBrandList(),
                    dataValueField: nameof(Brand.Id),
                    dataTextField: nameof(Brand.Name),
                    selectedValue: null
                )
            };
        }
    }
}