
using Microsoft.AspNetCore.Mvc;
using System.Net.Http.Json;
using RaceDay.MVC.ViewModels;

namespace RaceDay.MVC.Controllers
{
    public class AccountController : Controller
    {
        private readonly IHttpClientFactory _httpClientFactory;

        public AccountController(IHttpClientFactory httpClientFactory)
        {
            _httpClientFactory = httpClientFactory;
        }

        [HttpGet]
        public IActionResult Login()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Login(LoginViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            var client = _httpClientFactory.CreateClient("RaceDayAPI");

            var response = await client.PostAsJsonAsync(
                "api/auth/login",
                model);

            if (!response.IsSuccessStatusCode)
            {
                ModelState.AddModelError(
                    string.Empty,
                    "Login failed. Please check your email and password.");

                return View(model);
            }

            var result = await response.Content
                .ReadFromJsonAsync<LoginApiResponse>();

            if (result == null ||
                result.UserID <= 0 ||
                string.IsNullOrWhiteSpace(result.Role))
            {
                ModelState.AddModelError(
                    string.Empty,
                    "The API returned an invalid login response.");

                return View(model);
            }

            HttpContext.Session.SetInt32("UserID", result.UserID);

            HttpContext.Session.SetString("UserRole", result.Role);

            HttpContext.Session.SetString("UserEmail", model.Email);

            if (!string.IsNullOrWhiteSpace(result.Token))
            {
                HttpContext.Session.SetString("AuthToken", result.Token);
            }

            if (result.Role.Equals(
                "Organiser",
                StringComparison.OrdinalIgnoreCase))
            {
                return RedirectToAction("Dashboard", "Organiser");
            }

            if (result.Role.Equals(
                "Participant",
                StringComparison.OrdinalIgnoreCase))
            {
                return RedirectToAction("Index", "Participant");
            }

            HttpContext.Session.Clear();

            ModelState.AddModelError(
                string.Empty,
                "Your account has an unsupported role.");

            return View(model);
        }

        [HttpGet]
        public IActionResult Register()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Register(RegisterViewModel model)
        {
            if (model.Role != "Organiser" &&
                model.Role != "Participant")
            {
                ModelState.AddModelError(
                    nameof(model.Role),
                    "Please select a valid account type.");
            }

            if (!ModelState.IsValid)
            {
                return View(model);
            }

            var client = _httpClientFactory.CreateClient("RaceDayAPI");

            var response = await client.PostAsJsonAsync(
                "api/auth/register",
                new
                {
                    model.FirstName,
                    model.LastName,
                    model.Email,
                    model.Password,
                    model.Phone,
                    model.Role
                });

            if (!response.IsSuccessStatusCode)
            {
                ModelState.AddModelError(
                    string.Empty,
                    "Registration failed. Please check your details or try another email address.");

                return View(model);
            }

            TempData["SuccessMessage"] =
                "Your RaceDay account has been created. Please log in.";

            return RedirectToAction("Login");
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Logout()
        {
            HttpContext.Session.Clear();

            return RedirectToAction("Index", "Home");
        }

        // Represents the login response returned by the API.
        private class LoginApiResponse
        {
            public int UserID { get; set; }

            public string Role { get; set; } = string.Empty;

            public string? Token { get; set; }
        }
    }
}
