using System;
using System.ComponentModel.DataAnnotations;

namespace Orga.Models
{
    /// <summary>
    /// Décrit un article de maquillage
    /// </summary>
    public class Article
    {
        /// <summary>
        /// ID
        /// </summary>
        /// <value></value>
        public int Id { get; set; }

        /// <summary>
        /// Le nom de l'article
        /// </summary>
        /// <value></value>
        [Display(Name="Nom")]
        public string Name { get; set; }

        /// <summary>
        /// La date d'achat de l'article
        /// </summary>
        /// <value></value>
        [Display(Name="Date d'achat")]
        [DataType(DataType.Date)]
        public DateTime PurchaseDate { get; set; }

        /// <summary>
        /// L'ID de l'image de l'article
        /// </summary>
        /// <value></value>
        public int? ImageId { get; set; }

        /// <summary>
        /// L'image de l'article
        /// </summary>
        [DataType(DataType.ImageUrl)]
        public ImageData Image { get; set; }

        /// <summary>
        /// L'ID de la marque productrice de l'article
        /// </summary>
        /// <value></value>
        public int? BrandId { get; set; }

        /// <summary>
        /// La marque productrice de l'article
        /// </summary>
        /// <value></value>
        [Display(Name="Marque")]
        public Brand Brand { get; set; }

        /// <summary>
        /// L'ID de la catégorie de l'article
        /// </summary>
        /// <value></value>
        public int? CategoryId { get; set; }

        /// <summary>
        /// La catégorie de l'article
        /// </summary>
        /// <value></value>
        [Display(Name="Catégorie")]
        public Category Category { get; set; }
    }
}