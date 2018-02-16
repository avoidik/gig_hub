using Microsoft.AspNet.Identity;
using System.Data.Entity;
using System.Linq;
using System.Web.Http;
using WebApplication1.Models;

namespace WebApplication1.Controllers.Api
{
    public class GigStateDto
    {
        public int GigId { get; set; }
    }

    [Authorize]
    public class GigsController : ApiController
    {
        private ApplicationDbContext _context;

        public GigsController()
        {
            _context = new ApplicationDbContext();
        }

        [HttpDelete]
        public IHttpActionResult Cancel(int id)
        {
            var userId = User.Identity.GetUserId();
            var gig = _context.Gig
                .Include(g => g.Attendances.Select(a => a.Attendee))
                .Single(g => g.Id == id && g.ArtistId == userId);

            if (gig.IsCancelled)
            {
                return NotFound();
            }

            gig.Cancel();

            _context.SaveChanges();

            return Ok();
        }

        [HttpPost]
        public IHttpActionResult Uncancel(GigStateDto dto)
        {
            var userId = User.Identity.GetUserId();
            var gig = _context.Gig
                .Include(g => g.Attendances.Select(a => a.Attendee))
                .Single(g => g.Id == dto.GigId && g.ArtistId == userId);

            if (!gig.IsCancelled)
            {
                return NotFound();
            }

            gig.Uncancel();

            _context.SaveChanges();

            return Ok();
        }
    }
}
