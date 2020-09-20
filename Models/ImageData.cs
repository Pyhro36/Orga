using System.IO;

namespace Orga.Models
{
    public class ImageData
    {
        /// <summary>
        /// ID
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// Données de l'image
        /// </summary>
        public byte[] Data { get; set; }
    }
}