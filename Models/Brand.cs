using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Orga.Models
{

    /// <summary>
    /// Décrit une marque créatrice de maquillage
    /// </summary>
    public class Brand
    {
        /// <summary>
        /// ID
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// Nom de la marque
        /// </summary>
        /// <value></value>
        [Display(Name="Nom")]
        public string Name { get; set; }

        /// <summary>
        /// La liste des articles de la marque
        /// </summary>
        /// <value></value>
        public ICollection<Article> Articles { get; set; } 
    }
}