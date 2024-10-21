using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using Project.Models;
using Project.ViewModels;

public class DoctorAccountController : Controller
{
    private readonly SignInManager<User> signInManager;
    private readonly UserManager<User> userManager;
    
    private readonly VeseetaDBContext context;

    public DoctorAccountController(SignInManager<User> signInManager, UserManager<User> userManager, VeseetaDBContext context)
    {
        this.signInManager = signInManager;
        this.userManager = userManager;
        this.context=context;
    }
    [HttpGet]
    public async Task<IActionResult> SearchDoctors(string speciality, string city, string insurance)
    {
        // Ensure at least one filter is provided
        if (string.IsNullOrEmpty(speciality) && string.IsNullOrEmpty(city) && string.IsNullOrEmpty(insurance))
        {
            return BadRequest("Please provide at least one search criteria.");
        }

        // Build the query from the Doctor table
        var query = context.Doctors
            .Include(d => d.Specialization)
            .Include(d => d.Clinic)
            .ThenInclude(c => c.Location) // Assuming Clinic has a Location property
            .Include(d => d.Clinic.ClinicInsurances) // Include ClinicInsurances
            .AsQueryable();

        // Apply speciality filter if provided
        if (!string.IsNullOrEmpty(speciality))
        {
            query = query.Where(d => d.Specialization.Name == speciality);
        }

        // Apply city filter if provided
        if (!string.IsNullOrEmpty(city))
        {
            query = query.Where(d => d.Clinic.Location.City == city);
        }

        // Apply insurance filter if provided
        if (!string.IsNullOrEmpty(insurance))
        {
            query = query.Where(d => d.Clinic.ClinicInsurances.Any(ci => ci.Insurance.Name == insurance));
        }

        // Execute the query and get the results
        var doctors = await query.Select(d => new DoctorViewModel
        {
            Bio = d.Bio,
            DoctorName = d.User.Name,
            ClinicName = d.Clinic.Name,
            ClinicPrice = (decimal)d.Clinic.Price,
            ClinicCity = d.Clinic.Location.City,
            SpecializationName = d.Specialization.Name,
            InsuranceAccepted = d.Clinic.ClinicInsurances.Select(ci => ci.Insurance.Name).ToList() // Collect insurance names
        }).ToListAsync();

        // Return the view with the results
        return View("SearchResults", doctors);
    }


    [HttpGet]
    public IActionResult Login()
    {
        return View();
    }

    [HttpPost]
    public async Task<IActionResult> Login(DoctorLoginViewModel model)
    {
        if (ModelState.IsValid)
        {
            var result = await signInManager.PasswordSignInAsync(model.Email, model.Password, model.RememberMe, false);

            if (result.Succeeded)
            {
                return RedirectToAction("Index", "Home");
            }
            else
            {
                ModelState.AddModelError("", "Email or password is incorrect");
                return View(model);
            }
        }
        return View(model);
    }

    [HttpGet]
    public IActionResult Register()
    {
        return View();
    }

    [HttpPost]
    public async Task<IActionResult> Register(DoctorRegisterViewModel model)
    {
        if (ModelState.IsValid)
        {
            User user = new User
            {
                Name = model.Name,
                Email = model.Email,
                UserName = model.Email,
                PhoneNumber = model.PhoneNumber,
                Gender = model.Gender,
            };

            var result = await userManager.CreateAsync(user, model.Password);

            if (result.Succeeded)
            {
                await userManager.AddToRoleAsync(user, "Doctor");

                Location clinicLocation = new Location
                {
                    City = model.City,
                    Area = model.Area,
                    Address = model.Address,
                    GpsLoc = model.GpsLoc
                };

                Clinic clinic = new Clinic
                {
                    Name = model.ClinicName,
                    Price = model.ClinicPrice,
                    Location = clinicLocation
                };

               
                if (!Enum.TryParse(model.SpecializationName, true, out SpecializationName specializationName))
                {
                    ModelState.AddModelError("SpecializationName", "Invalid specialization selected.");
                    return View(model);
                }

           
                var specialization = new Specialization
                {
                    Name = specializationName.ToString() // 
                };

              
                context.Specializations.Add(specialization);
                await context.SaveChangesAsync(); 

                
                Doctor doctor = new Doctor
                {
                    DoctorId = user.Id,
                    Bio = model.Bio,
                    LicenseNumber = model.LicenseNumber,
                    Clinic = clinic,
                    SpecializationId = specialization.SpecializationId,
                    UserId = user.Id,
                    User = user
                };

                context.Add(doctor);
                await context.SaveChangesAsync();

                return RedirectToAction("Login", "DoctorAccount");
            }
            else
            {
                foreach (var error in result.Errors)
                {
                    ModelState.AddModelError("", error.Description);
                }
                return View(model);
            }
        }

        return View(model);
    }






    public async Task<IActionResult> Logout()
    {
        await signInManager.SignOutAsync();
        return RedirectToAction("Index", "Home");
    }

    public IActionResult VerifyEmail()
    {
        return View();
    }
    public IActionResult ChangePassword()
    {
        return View();
    }




    [HttpGet]
    public async Task<IActionResult> Profile()
    {
        // Get the currently authenticated user
        var user = await userManager.GetUserAsync(User);
        if (user == null)
        {
            return RedirectToAction("Login", "Account");
        }

        // Fetch Bio and LicenseNumber of the doctor, filtering by UserId
        var doctor = await context.Doctors
            .Where(d => d.UserId == user.Id)
            .Include(d => d.Specialization)
            .Select(d => new {
                d.Bio,
                d.LicenseNumber,
                ClinicName = d.Clinic.Name,
                ClinicPrice = d.Clinic.Price,
                City = d.Clinic.Location.City,
                Area = d.Clinic.Location.Area,
                Address = d.Clinic.Location.Address,
                GpsLoc = d.Clinic.Location.GpsLoc,
                Name = d.User.Name,
                Email = d.User.Email,
                PhoneNumber = d.User.PhoneNumber,
                SpecializationName = d.Specialization.Name,
                Gender = d.User.Gender

            }) // Select both Bio and LicenseNumber fields
            .FirstOrDefaultAsync();

        // If doctor is not found, handle it
        if (doctor == null)
        {
            ModelState.AddModelError("", "Doctor not found.");
            return RedirectToAction("Error", "Home");
        }

        // Construct the view model with Bio and LicenseNumber
        var model = new DoctorProfileViewModel
        {
            Bio = doctor.Bio,
            LicenseNumber = doctor.LicenseNumber,
            ClinicName = doctor.ClinicName, // Pass the clinic name to the view model
            ClinicPrice = doctor.ClinicPrice,
            City = doctor.City,
            Area = doctor.Area,
            Address = doctor.Address,
            GpsLoc = doctor.GpsLoc,
            SpecializationName = doctor.SpecializationName, 
            Name = doctor.Name,
            Email = doctor.Email,
            PhoneNumber = doctor.PhoneNumber,
            Gender = doctor.Gender
        };

        // Return the view with the model containing Bio and LicenseNumber
        return View(model);
    }


    // GET Action for displaying the Edit form
    // GET Action for displaying the Edit form
    [HttpGet]
    public async Task<IActionResult> EditProfile()
    {
        // Get the currently authenticated user
        var user = await userManager.GetUserAsync(User);
        if (user == null)
        {
            return RedirectToAction("Login", "Account");
        }

        // Fetch doctor details using the current user's Id
        var doctor = await context.Doctors
            .Where(d => d.UserId == user.Id)
            .Include(d => d.Specialization)
            .Select(d => new {
                d.Bio,
                d.LicenseNumber,
                ClinicName = d.Clinic.Name,
                ClinicPrice = d.Clinic.Price,
                City = d.Clinic.Location.City,
                Area = d.Clinic.Location.Area,
                Address = d.Clinic.Location.Address,
                GpsLoc = d.Clinic.Location.GpsLoc,
                Name = d.User.Name,
                Email = d.User.Email,
                PhoneNumber = d.User.PhoneNumber,
                SpecializationName = d.Specialization.Name,
                Gender = d.User.Gender
            })
            .FirstOrDefaultAsync();

        if (doctor == null)
        {
            ModelState.AddModelError("", "Doctor not found.");
            return RedirectToAction("Error", "Home");
        }

        // Fill the view model with the fetched data and apply validations
        var model = new DoctorProfileEditViewModel
        {
            Bio = doctor.Bio,
            LicenseNumber = doctor.LicenseNumber,
            ClinicName = doctor.ClinicName,
            ClinicPrice = doctor.ClinicPrice,
            City = doctor.City,
            Area = doctor.Area,
            Address = doctor.Address,
            GpsLoc = doctor.GpsLoc,
            SpecializationName = doctor.SpecializationName,
            Name = doctor.Name,
            Email = doctor.Email,
            PhoneNumber = doctor.PhoneNumber,
            Gender = doctor.Gender
        };

        return View(model); // Return the EditProfile view with the model
    }


    // POST Action for saving the changes
    // POST Action for saving the changes
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> EditProfile(DoctorProfileEditViewModel model)
    {
        if (!ModelState.IsValid)
        {
            return View(model); // Return the same view if validation fails
        }

        // Get the current user
        var user = await userManager.GetUserAsync(User);
        if (user == null)
        {
            return RedirectToAction("Login", "Account");
        }

        // Fetch the doctor entity based on UserId
        var doctor = await context.Doctors
            .Include(d => d.Clinic)
            .ThenInclude(c => c.Location)
            .Include(d => d.User)
            .FirstOrDefaultAsync(d => d.UserId == user.Id);

        if (doctor == null)
        {
            ModelState.AddModelError("", "Doctor not found.");
            return RedirectToAction("Error", "Home");
        }

        // تحقق من أن Clinic و Location ليسا null
        if (doctor.Clinic == null)
        {
            doctor.Clinic = new Clinic();
        }
        if (doctor.Clinic.Location == null)
        {
            doctor.Clinic.Location = new Location();
        }

        // Update the doctor and clinic information
        doctor.Bio = model.Bio;
        doctor.LicenseNumber = model.LicenseNumber;
        doctor.Clinic.Name = model.ClinicName;
        doctor.Clinic.Price = model.ClinicPrice;
        doctor.Clinic.Location.City = model.City;
        doctor.Clinic.Location.Area = model.Area;
        doctor.Clinic.Location.Address = model.Address;
        doctor.Clinic.Location.GpsLoc = model.GpsLoc;

        // Update user information
        doctor.User.Name = model.Name;
        doctor.User.Email = model.Email;
        doctor.User.PhoneNumber = model.PhoneNumber;
        doctor.User.Gender = model.Gender;

        // Save the changes to the database
        await context.SaveChangesAsync();

        // Redirect to the profile page after successful update
        return RedirectToAction("Profile");
    }



}