using Microsoft.AspNetCore.Mvc.Rendering;

using Orga.Models;

namespace Orga.ViewModels
{
    public class ArticleEditViewModel
    {   
        /// <summary>
        /// L'article à créer/éditer
        /// </summary>
        /// <value></value>
        public Article Article { get; set; }

        /// <summary>
        /// La liste marques à choisir
        /// </summary>
        /// <value></value>
        public SelectList Brands { get; set; }
    }
}