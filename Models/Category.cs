namespace Orga.Models
{
    /// <summary>
    /// Décrit la catégorie de maquillage (Eyeliner, Mascara, Rouge à lèvres ...)
    /// </summary>
    public class Category
    {
        /// <summary>
        /// ID
        /// </summary>
        /// <value></value>
        public int Id { get; set; }

        /// <summary>
        /// Le nom de la catégorie
        /// </summary>
        /// <value></value>
        public string Name { get; set; }
    }
}