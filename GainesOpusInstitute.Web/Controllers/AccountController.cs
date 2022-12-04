using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using AutoMapper;
using GainesOpusInstitute.DataEntity.Entity;
using GainesOpusInstitute.Repository;
using GainesOpusInstitute.Web.Models;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace GainesOpusInstitute.Web.Controllers
{
    public class AccountController : Controller
    {
        private readonly RoleManager<Role> _roleManager;
        private readonly UserManager<User> _userManager;
        private readonly IAccountManager _accountManager;
        private readonly IMapper _mapper;
        //private readonly ISMTPService _emailSender;
        private readonly IHttpContextAccessor _contextAccessor;
        //private readonly IApplicationUser _userService;
        private SignInManager<User> _signInManager;

        private readonly ILogger<AccountController> _Logger;

        public AccountController(IMapper mapper, UserManager<User> userManager, RoleManager<Role>
            roleManager, ILogger<AccountController> logger,  IAccountManager accountManager, SignInManager<User> signInManager, 
            IHttpContextAccessor httpContextAccessor)
        {
            _mapper = mapper;
            //_userService = userService;
            //_emailSender = emailSender;
            _accountManager = accountManager;
            _roleManager = roleManager;
            _userManager = userManager;
            _Logger = logger;
            _signInManager = signInManager;
            _contextAccessor = httpContextAccessor;
        }


        [AllowAnonymous]
        public IActionResult Login(string returnUrl = "")
        {
            var login = new LoginViewModel();
            login.ReturnUrl = returnUrl;
            return View(login);
        }
        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Login(LoginViewModel login)
        {
            if (ModelState.IsValid)
            {
                User appUser = await _userManager.FindByEmailAsync(login.Email);
                if (appUser == null)
                {
                    throw new Exception("Email not found");
                }
                //  var result = await _signInManager.PasswordSignInAsync(appUser, login.Password, false, false);
                var result = await _signInManager.PasswordSignInAsync(appUser, login.Password, login.RememberMe, lockoutOnFailure: true);
                //var result = await _signInManager.PasswordSignInAsync(appUser, login.Password, false, false);
                if (result != null)
                {
                    //Course/RegisterCourse

                    var currentUser = _contextAccessor.HttpContext.User.Identity.Name;
                    return RedirectToAction("RegisterCourse", "Course");
                    //var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
                    //var user = _userService.GetById(userId);
                    //if (user.UserName == "admin")
                    //{
                    // return RedirectToAction("Course", "Index");
                    //}
                    //else
                    //{
                    //    return RedirectToAction("RegisterCourse", "Course");
                    //}
                    //if (User.HasClaim(Claims.User, "true"))
                    //{
                    //    return RedirectToAction("RegisterCourse", "Course");
                    //}



                    //   await _signInManager.SignOutAsync();
                    //  return RedirectToAction();
                }

                ModelState.AddModelError(nameof(login.Email), "Login Failed: Invalid Email or password");
            }
            return View(login);
        }
        public IActionResult Register()
        {
            return View(new RegisterViewModel());
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        //[Authorize(Policy = Claims.Admin)]
        public async Task<IActionResult> Register([FromForm] RegisterViewModel model)
        {
            if (ModelState.IsValid)
            {
                var user = new User
                {
                    UserName = model.Email,
                    Email = model.Email,
                     Surname = model.Surname,
                     Othernames = model.Othernames,
                     Instrument = model.Instrument,
                };
                var result = await _userManager.CreateAsync(user, model.Password);
                if (result.Succeeded)
                {
                    // any body registering is user
                    var results = await _userManager.AddClaimAsync(user, new Claim(Claims.User, "true"));
                    //switch (model.SelectedRole)
                    //{
                    //    //case Claims.Admin:
                    //    //    await _userManager.AddClaimAsync(user, new Claim(Claims.Admin, "true"));
                    //        // _userManager.AddToRole(user.UserName, "User");
                    //        //break;
                    //    case Claims.User:
                    //        await _userManager.AddClaimAsync(user, new Claim(Claims.User, "true"));
                    //        break;
                    //}
                    TempData["Message"] = $"Successfully created the user {model.Email}";
                    return RedirectToAction("Index", "Home");
                    //return RedirectToAction(nameof(Register));
                }

                TempData["Error"] = $"An Error has occured while creating the user {model.Email}, try using a new email or password.";
            }
            return View(model);
        }
        [HttpGet]
        [AllowAnonymous]
        public IActionResult ForgotPassword()
        {
            return View();
        }

        [HttpPost]
        [AllowAnonymous]
        public async Task<IActionResult> ForgotPassword(ForgotPasswordViewModel model)
        {
            if (ModelState.IsValid)
            {
                // Find the user by email
                var user = await _userManager.FindByEmailAsync(model.Email);
                // If the user is found AND Email is confirmed
                if (user != null && await _userManager.IsEmailConfirmedAsync(user))
                {
                    // Generate the reset password token
                    var token = await _userManager.GeneratePasswordResetTokenAsync(user);
                    // Build the password reset link
                    var passwordResetLink = Url.Action("ResetPassword", "Account", new { email = model.Email, token = token }, Request.Scheme);
                    //send mail for Password reset
                    var subject = "Details for resetting your password";
                    var message = "Hi" +" " +user.Othernames+"," +"</br>";
                    message += "</br>You've asked to reset your password on Mia Asaba.</b>";                 
                    message += "</br><a href="+passwordResetLink+ ">Just click  or copy the link below.</a></br>";
                    message += "</br>Regards";                   
                   // await _emailSender.SendEmailAsync(model.Email, subject, message, "");
                    // Log the password reset link
                    _Logger.Log(LogLevel.Warning, passwordResetLink);
                    // Send the user to Forgot Password Confirmation view
                    return View("ForgotPasswordConfirmation");                                
                }                
                return View("ForgotPasswordConfirmation");
            }
            return View(model);
        }

        [HttpGet]
        [AllowAnonymous]
        public IActionResult ResetPassword(string token, string email)
        {
            // If password reset token or email is null, most likely the
            // user tried to tamper the password reset link
            if (token == null || email == null)
            {
                ModelState.AddModelError("", "Invalid password reset token");
            }
            return View();
        }

        [HttpPost]
        [AllowAnonymous]
        public async Task<IActionResult> ResetPassword(ResetPasswordViewModel model)
        {
            if (ModelState.IsValid)
            {
                // Find the user by email
                var user = await _userManager.FindByEmailAsync(model.Email);

                if (user != null)
                {
                    // reset the user password
                    var result = await _userManager.ResetPasswordAsync(user, model.Token, model.Password);
                    if (result.Succeeded)
                    {
                        return View("ResetPasswordConfirmation");
                    }
                    // Display validation errors. For example, password reset token already
                    // used to change the password or password complexity rules not met
                    foreach (var error in result.Errors)
                    {
                        ModelState.AddModelError("", error.Description);
                    }
                    return View(model);
                }

                // To avoid account enumeration and brute force attacks, don't
                // reveal that the user does not exist
                return View("ResetPasswordConfirmation");
            }
            // Display validation errors if model state is not valid
            return View(model);
        }



        public async Task<IActionResult> Roles()
        {

            var roles = await _accountManager.GetRoles();

            List<RoleModel> roleList = _mapper.Map<List<RoleModel>>(roles);
            return View(roleList);
        }

        public ViewResult RoleList()
        {
            var result = _roleManager.Roles;
            List<RoleModel> RoleList = _mapper.Map<List<RoleModel>>(result);
            return View(RoleList);
        }

        public IActionResult CreateRole() => View();


        [HttpPost]
        public async Task<IActionResult> CreateRole([Required]string name)
        {
            if (ModelState.IsValid)
            {
                //  IdentityResult result = await roleManager.CreateAsync(name);
                var role = new Role
                {
                    Name = name
                };
                // var mappedUser = _mapper.Map<Role>(name);
                IdentityResult result = await _roleManager.CreateAsync(role);

                //  RoleModel roles = _mapper.Map<RoleModel>(result);

                if (result.Succeeded)
                {
                    return RedirectToAction("RoleList");
                }

                else
                {
                    Errors(result);
                }
                // TempData["Message"] = result;
                //return View(roles);
            }
            return View(name);
        }
        private void Errors(IdentityResult result)
        {
            foreach (IdentityError error in result.Errors)
                ModelState.AddModelError("", error.Description);
        }

        public async Task<IActionResult> Update(int id)
        {
            //  IdentityRole role = await roleManager.FindByIdAsync(id);
            var rolee = await _accountManager.GetRoleByIdAsync(id);
            List<User> members = new List<User>();
            List<User> nonMembers = new List<User>();

            var listOfUsers = _userManager.Users;
            foreach (User user in listOfUsers)
            {
                var list = await _userManager.IsInRoleAsync(user, rolee.Name) ? members : nonMembers;
                list.Add(user);
            }
            // RoleEditModel smodel = _mapper.Map<RoleEditModel>(rolee);
            var me = new RoleEditModel
            {
                Role = rolee,
                Members = members,
                NonMembers = nonMembers
            };

            return View(me);
        }
        [HttpPost]
        public async Task<IActionResult> Update(RoleModificationModel model)
        {
            IdentityResult result;
            if (ModelState.IsValid)
            {
                foreach (string userId in model.AddIds ?? new string[] { })
                {
                    User user = await _userManager.FindByIdAsync(userId);
                    if (user != null)
                    {
                        result = await _userManager.AddToRoleAsync(user, model.RoleName);
                        if (!result.Succeeded)
                            TempData[""] = "";
                        //  Errors(result);
                    }
                }
                foreach (string userId in model.DeleteIds ?? new string[] { })
                {
                    User user = await _userManager.FindByIdAsync(userId);
                    if (user != null)
                    {
                        result = await _userManager.RemoveFromRoleAsync(user, model.RoleName);
                        if (!result.Succeeded)
                            TempData[""] = "";
                        // Errors(result);
                    }
                }
            }

            if (ModelState.IsValid)
                return RedirectToAction(nameof(Roles));
            else
                return await Update(model.RoleId);
        }
        [HttpPost]
        public async Task<IActionResult> Logout()
        {
            if (User?.Identity.IsAuthenticated == true)
            {
                await _signInManager.SignOutAsync();
                // return RedirectToPage(); // A redirect ensures that the cookies has gone.
                return RedirectToAction("Index", "Home");
            }
           // return Page();
            // await _signInManager.Context.SignOutAsync();
           // await _signInManager.Context.SignOutAsync();
            return RedirectToAction("Index", "Home");
        }
        //void Errors(IdentityResult result)
        //{
        //    foreach (IdentityError error in result.Errors)
        //        ModelState.AddModelError("", error.Description);
        //}
    }
}