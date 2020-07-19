using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;

namespace Orga.FileAbstraction
{
    /// <summary>
    /// Décrit l'interface d'un service de sauvegarde de fichiers
    /// </summary>
    public interface IFileSaveService
    {
        /// <summary>
        /// Démarre et renvoie une tâche de sauvegarde du fichier passé en paramètres
        /// </summary>
        /// <param name="file">Le fichier à sauvegarder</param>
        /// <param name="name">Le nom à donner au fichier</param>
        /// <returns>La tâche de sauvegarde</returns>
        Task SaveAsync(IFormFile file, string name);

        /// <summary>
        /// Démarre et renvoie une tâche de suppression du fichier dont le nom est passé en paramètres
        /// </summary>
        /// <param name="name">Le nom du fichier à supprimer</param>
        /// <returns>La tâche de suppression</returns>
        Task DeleteAsync(string name);
    }
}