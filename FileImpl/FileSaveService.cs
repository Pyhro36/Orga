using System;
using System.IO;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Mono.Unix;

using Orga.FileAbstraction;

namespace Orga.FileImpl
{
    /// <summary>
    /// Implémentation basique d'un service de sauvegarde de fichiers en stockage physique local
    /// </summary>
    public class FileSaveService : IFileSaveService
    {
        /// <summary>
        /// La configuration de l'application
        /// </summary>
        private readonly IConfiguration _configuration;

        /// <summary>
        /// Constructeur du service de sauvegarde des fichiers pour lui passer les dépendances nécessaires
        /// </summary>
        /// <param name="configuration">Le module de configuration de l'application</param>
        public FileSaveService(IConfiguration configuration)
        {
            this._configuration = configuration;
        }

        /// <summary>
        /// Démarre et renvoie une tâche de sauvegarde du fichier passé en paramètres
        /// </summary>
        /// <param name="file">Le fichier à sauvegarder</param>
        /// <param name="name">Le nom à donner au fichier</param>
        /// <returns>La tâche de sauvegarde</returns>
        public async Task SaveAsync(IFormFile file, string name)
        {
            string fullPath = Path.Combine(_configuration.GetValue<string>(Constants.VAR_ROOT_DIR_KEY), name);

            using (var stream = new Mono.Unix.UnixFileInfo(fullPath).Create(
                    FileAccessPermissions.UserRead |
                    FileAccessPermissions.UserWrite |
                    FileAccessPermissions.GroupRead))
            {
                await file.CopyToAsync(stream);
            }
        }

        /// <summary>
        /// Démarre et renvoie une tâche de suppression du fichier dont le nom est passé en paramètres
        /// </summary>
        /// <param name="name">Le nom du fichier à supprimer</param>
        /// <returns>La tâche de suppression</returns>
        public async Task DeleteAsync(string name) => await Task.Run(() => System.IO.File.Delete(name));
    }
}