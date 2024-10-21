using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using Project.Models;
using Project.ViewModels; // For your ProfileViewModel

public class AccountController : Controller
{
    private readonly SignInManager<User> signInManager;
    private readonly UserManager<User> userManager;

    public AccountController(SignInManager<User> signInManager, UserManager<User> userManager)
    {
        this.signInManager = signInManager;
        this.userManager = userManager;
    }


    [HttpGet]
    public IActionResult Login()
    {
        return View();
    }

    // [AllowAnonymous]
    [HttpPost]
    public async Task<IActionResult> Login(LoginViewModel model)
    {
        if(ModelState.IsValid)
        {
            var result = await signInManager.PasswordSignInAsync(model.Email,model.Password,model.RememberMe,false);

            if(result.Succeeded)
            {
                return RedirectToAction("Index","Home");
            }
            else
            {
                ModelState.AddModelError("","Email or password is incorrect");
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
    public async Task<IActionResult> Register(RegisterViewModel model)
    {
        if (ModelState.IsValid)
        {
            User user = new User
            {
                Name = model.Name,
                Email = model.Email,
                UserName = model.Email,
                PhoneNumber = model.PhoneNumber,
                Db = model.Birthdate,
                Gender = model.Gender,
                // Type = "PATIENT"
            };
            var result = await userManager.CreateAsync(user, model.Password);

            if (result.Succeeded)
            {
                await userManager.AddToRoleAsync(user,"User"); // assigning role to user
                return RedirectToAction("Login", "Account");
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
        return RedirectToAction("Index","Home");
    }

    public IActionResult VerifyEmail()
    {
        return View();
    }
    public IActionResult ChangePassword()
    {
        return View();
    }



    ///////////////////
    [HttpGet]
    public async Task<IActionResult> Profile()
    {
        // Get the logged-in user
        var user = await userManager.GetUserAsync(User);

        if (user == null)
        {
            return RedirectToAction("Login", "Account");
        }

        // Prepare the ViewModel with user data
        var model = new ProfileViewModel
        {
            Name = user.Name,
            Email = user.Email,
            PhoneNumber = user.PhoneNumber,
            Birthdate = user.Db, // Adjust if necessary
            Gender = user.Gender
        };

        return View(model); // Pass the model to the view
    }


   
    [HttpGet]
    public async Task<IActionResult> Edit()
    {
        var user = await userManager.GetUserAsync(User);
        if (user == null)
        {
            return RedirectToAction("Login", "Account");
        }

        var model = new EditProfileViewModel
        {
            Name = user.Name,
            Email = user.Email,
            PhoneNumber = user.PhoneNumber,
            Birthdate = user.Db, // Ensure this is correctly mapped
            Gender = user.Gender
        };

        return View(model);
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Edit(EditProfileViewModel model)
    {
        if (ModelState.IsValid)
        {
            var user = await userManager.GetUserAsync(User);
            if (user == null)
            {
                return RedirectToAction("Login", "Account");
            }

            // Update user properties
            user.Name = model.Name;
            user.Email = model.Email;
            user.PhoneNumber = model.PhoneNumber;
            user.Db = (DateTime)model.Birthdate; // Ensure this is correctly mapped
            user.Gender = model.Gender;

            // Optionally update the password if provided


            var updateResult = await userManager.UpdateAsync(user);
            if (updateResult.Succeeded)
            {
                TempData["SuccessMessage"] = "Profile updated successfully."; // Optional success message
                return RedirectToAction("Index", "Home");
            }

            // Handle errors
            foreach (var error in updateResult.Errors)
            {
                ModelState.AddModelError(string.Empty, error.Description);
            }
        }

        return View(model); // Return the model back to the view if validation fails
    }






}