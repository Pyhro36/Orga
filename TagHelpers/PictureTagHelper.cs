using System.IO;
using System.Text.Encodings.Web;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc.Routing;
using Microsoft.AspNetCore.Mvc.TagHelpers;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using Microsoft.AspNetCore.Razor.TagHelpers;

namespace Orga.TagHelpers
{
    /// <summary>
    /// Tag-helper servant à afficher l'image d'un objet sauvegardée en fichier
    /// </summary>
    public class PictureTagHelper : ImageTagHelper
    {
        /// <summary>
        /// Le nom de l'attribut HTML de source
        /// </summary>
        private const string SRC_ATTRIBUTE = "src";

        /// <summary>
        /// Définit l'attribut "org-name" devant contenir le nom de l'image à afficher
        /// </summary>
        /// <value>Le nom de l'image à afficher</value>
        public string OrgName { get; set; }

        /// <summary>
        /// Crée une nouvelle instance tag-helper &lt;picture&gt; 
        /// </summary>
        /// <param name="fileVersionProvider"></param>
        /// <param name="htmlEncoder"></param>
        /// <param name="urlHelperFactory"></param>
        /// <returns></returns>
        public PictureTagHelper(IFileVersionProvider fileVersionProvider,
                                HtmlEncoder htmlEncoder,
                                IUrlHelperFactory urlHelperFactory) :
                                base(fileVersionProvider, htmlEncoder, urlHelperFactory)
        {
        }

        /// <summary>
        /// Méthode de processus synchrone du tag-helper
        /// </summary>
        /// <param name="context"></param>
        /// <param name="output"></param>
        public override void Process(TagHelperContext context, TagHelperOutput output)
        {
            InnerProcess(output);
            base.Process(context, output);
        }

        /// <summary>
        /// Méthode de processus asynchrone du tag-helper
        /// </summary>
        /// <param name="context"></param>
        /// <param name="output"></param>
        public async override Task ProcessAsync(TagHelperContext context, TagHelperOutput output)
        {
            await Task.Run(() => InnerProcess(output));
            await base.ProcessAsync(context, output);
        }

        /// <summary>
        /// Méthode de processus spécifique au tag-helper : génère la source de l'image à partir du nom donné de l'image en attribut "org-name"
        /// </summary>
        /// <param name="output"></param>
        private void InnerProcess(TagHelperOutput output)
                => output.Attributes.SetAttribute(SRC_ATTRIBUTE, Path.Combine(Constants.VARIABLE_CONTENT_URL_PATH, OrgName));
    }
}