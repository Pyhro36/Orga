using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

using Orga.Models;
using Orga.Repository;

namespace Orga.Controllers
{
    public class ImageController : Controller
    {
        private const string PNG_CONTENT_TYPE = "image/png";
        private readonly MakeupDbContext _context;

        public ImageController(MakeupDbContext context)
        {
            _context = context;
        }

        // GET: Image/Get/5
        public async Task<IActionResult> Get(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            ImageData image = await _context.ImageDatas.FirstOrDefaultAsync(imageData => imageData.Id == id);
            if (image == null)
            {
                return NotFound();
            }

            return File(image.Data, PNG_CONTENT_TYPE);
        }
    }
}
