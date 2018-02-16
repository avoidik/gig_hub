using Microsoft.AspNet.Identity;
using System;
using System.Data.Entity;
using System.Linq;
using System.Web.Mvc;
using WebApplication1.Models;
using WebApplication1.ViewModels;

namespace WebApplication1.Controllers
{
    public class GigsController : Controller
    {
        private ApplicationDbContext _context;

        public GigsController()
        {
            _context = new ApplicationDbContext();

        }

        [HttpPost]
        public ActionResult Search(GigsViewModel model)
        {
            return RedirectToAction("Index", "Home", new { query = model.SearchTerm });
        }

        // GET: Gigs
        [Authorize]
        public ActionResult Create()
        {
            var viewModel = new GigFormViewModel
            {
                Genres = _context.Genre.ToList(),
                Heading = "Add a Gig"
            };

            return View("GigForm", viewModel);
        }

        [Authorize]
        public ActionResult Edit(int id)
        {
            var userId = User.Identity.GetUserId();
            var gig = _context.Gig.Single(g => g.Id == id && g.ArtistId == userId);

            var viewModel = new GigFormViewModel
            {
                Id = gig.Id,
                Genres = _context.Genre.ToList(),
                Date = gig.DateTime.ToString("d MMM yyyy"),
                Time = gig.DateTime.ToString("HH:MM"),
                Genre = gig.GenreId,
                Venue = gig.Venue,
                Heading = "Edit a Gig"
            };

            return View("GigForm", viewModel);
        }

        [Authorize]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(GigFormViewModel model)
        {
            if (!ModelState.IsValid)
            {
                model.Genres = _context.Genre.ToList();
                return View("GigForm", model);
            }

            var gig = Gig.NewGig(model.GetDateTime(), User.Identity.GetUserId(), model.Genre, model.Venue);

            _context.Gig.Add(gig);
            _context.SaveChanges();

            return RedirectToAction("Mine", "Gigs");
        }

        [Authorize]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Update(GigFormViewModel model)
        {
            if (!ModelState.IsValid)
            {
                model.Genres = _context.Genre.ToList();
                return View("GigForm", model);
            }

            var userId = User.Identity.GetUserId();
            var gig = _context.Gig
                .Include(g => g.Attendances.Select(a => a.Attendee))
                .Single(g => g.Id == model.Id && g.ArtistId == userId);

            gig.Update(model.GetDateTime(), model.Venue, model.Genre);

            _context.SaveChanges();

            return RedirectToAction("Mine", "Gigs");
        }

        [Authorize]
        public ActionResult Attending()
        {
            var userId = User.Identity.GetUserId();
            var gigs = _context.Attendence
                .Where(a => a.AttendeeId == userId)
                .Select(a => a.Gig)
                .Include(g => g.Artist)
                .Include(g => g.Genre)
                .ToList();

            var viewModel = new GigsViewModel
            {
                UpcomingGigs = gigs,
                ShowActions = User.Identity.IsAuthenticated,
                Heading = "Gigs I'm attending"
            };

            return View("Gigs", viewModel);
        }

        [Authorize]
        public ActionResult Mine()
        {
            var userId = User.Identity.GetUserId();
            var gigs = _context.Gig
                .Where(g => g.ArtistId == userId && g.DateTime > DateTime.Now)
                .Include(g => g.Genre)
                .ToList();
            return View(gigs);
        }
    }
}