﻿using Microsoft.AspNet.Identity;
using System.Linq;
using System.Web.Http;
using WebApplication1.Models;

namespace WebApplication1.Controllers.Api
{
    public class AttendanceDto
    {
        public int GigId { get; set; }
    }

    [Authorize]
    public class AttendancesController : ApiController
    {
        private ApplicationDbContext _context;

        public AttendancesController()
        {
            _context = new ApplicationDbContext();
        }

        [HttpPost]
        public IHttpActionResult Attend(AttendanceDto dto)
        {
            var userId = User.Identity.GetUserId();

            if (_context.Attendence.Any(a => a.AttendeeId == userId && a.GigId == dto.GigId))
            {
                return BadRequest("Attendance already exists");
            }

            var attendance = new Attendance
            {
                GigId = dto.GigId,
                AttendeeId = userId
            };

            _context.Attendence.Add(attendance);
            _context.SaveChanges();

            return Ok();
        }
    }
}
