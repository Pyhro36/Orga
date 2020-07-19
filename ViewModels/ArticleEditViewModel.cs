using System;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.Rendering;

using Orga.Models;

namespace Orga.ViewModels
{
    public class ArticleEditViewModel : ArticleViewModel
    {   
        /// <summary>
        /// l'ID de l'article
        /// </summary>
        /// <value></value>
        public int Id { get; set; }

        /// <summary>
        /// Le chemin vers le fichier de l'image
        /// </summary>
        /// <value></value>
        public string ImageUrl { get; set; }
    }
}