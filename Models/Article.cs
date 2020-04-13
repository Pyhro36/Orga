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
        public string Name { get; set; }

        /// <summary>
        /// Une référence à l'image de l'article
        /// </summary>
        /// <value></value>
        public string ImageReference { get; set; }

        /// <summary>
        /// Le nombre d'articles disponibles
        /// </summary>
        /// <value></value>
        public int AvailableQuantity { get; set; }

        /// <summary>
        /// La marque productrice de l'article
        /// </summary>
        /// <value></value>
        public Brand Brand { get; set; }

        /// <summary>
        /// La marque productrice de l'article
        /// </summary>
        /// <value></value>
        public Category Category { get; set; }
    }
}