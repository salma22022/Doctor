using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging; 
using Project.Models;
using Project.ViewModels;
using System;
using System.Linq;

namespace Project.Controllers
{
    public class AppointmentController : Controller
    {
        private readonly VeseetaDBContext _context;
        private readonly ILogger<AppointmentController> _logger;


        public AppointmentController(VeseetaDBContext context, ILogger<AppointmentController> logger)
        {
            _context = context;
            _logger = logger;
        }

        // GET: AppointmentController
        public IActionResult BookAppointment(string doctorName)
        {
            ViewBag.DoctorName = doctorName;

            // Pass an instance of the ViewModel to the view
            return View(new AppointmentViewModel
            {
                DoctorName = doctorName
            });
        }


        // POST: AppointmentController/SubmitBooking
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult SubmitBooking(AppointmentViewModel model)
        {
            if (ModelState.IsValid)
            {
                _logger.LogInformation($"DoctorName from model: '{model.DoctorName}'");
                _logger.LogInformation($"Attempting to find doctor with name: {model.DoctorName}");
                _logger.LogInformation($"User.Identity.Name: '{User.Identity.Name}'");


                var doctorName = model.DoctorName?.Trim().ToLower();
                var doctor = _context.Doctors
                    .FirstOrDefault(d => d.User.UserName.ToLower() == doctorName);

                if (doctor == null)
                {
                    _logger.LogError($"Doctor '{model.DoctorName}' not found.");
                }

                var userName = User.Identity.Name?.Trim().ToLower();
                var user = _context.Users
                    .FirstOrDefault(u => u.UserName.ToLower() == userName);

                if (doctor != null && user != null)
                {
                    var booking = new Booking
                    {
                        Date = model.AppointmentDate,
                        UserId = user.Id,
                        DayId = doctor.Days.FirstOrDefault()?.DayId,
                        InsuranceId = null,
                        Status = "pending"
                    };

                    _context.Bookings.Add(booking);
                    _context.SaveChanges();

                    return RedirectToAction("Index", "Home");
                }

                if (doctor == null)
                {
                    _logger.LogWarning($"Doctor '{model.DoctorName}' not found.");
                    ModelState.AddModelError("", $"Doctor '{model.DoctorName}' not found. Please check the doctor name.");
                }

                if (user == null)
                {
                    _logger.LogWarning($"User '{User.Identity.Name}' not found.");
                    ModelState.AddModelError("", "User not found. Please ensure you are logged in.");
                }
            }

            return View("BookAppointment", model);
        }

        // GET: AppointmentController/Index
        public ActionResult Index()
        {
            return View();
        }




    }
}
