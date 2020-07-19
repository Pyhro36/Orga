using System;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.Rendering;

using Orga.Models;

namespace Orga.ViewModels
{
    public class ArticleViewModel
    {   
        /// <summary>
        /// le nom de l'article
        /// </summary>
        /// <value></value>
        public string Name { get; set; }

        /// <summary>
        /// La date d'achat de l'article
        /// </summary>
        /// <value></value>
        public DateTime PurchaseDate { get; set; }

        /// <summary>
        /// L'ID de la marque productrice de l'article
        /// </summary>
        /// <value></value>
        public int? BrandId { get; set; }

        /// <summary>
        /// Le fichier de l'image de l'article
        /// </summary>
        /// <value></value>
        public IFormFile ImageFile { get; set; }

        /// <summary>
        /// La liste marques à choisir
        /// </summary>
        /// <value></value>
        public SelectList Brands { get; set; }
    }
}