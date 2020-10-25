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
        /// L'ID de l'image actuelle de l'article
        /// </summary>
        /// <value></value>
        public int? ImageId { get; set; }
    }
}