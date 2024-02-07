using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Mvc;
using Slutprojekt_ASP.Net.Models;

// For more information on enabling MVC for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace Slutprojekt_ASP.Net.Controllers
{
    public class HomeController : Controller
    {
        // GET: /<controller>/
        public IActionResult Index(string returnUrl = "")
        {
            //Sparar eventuell return url som man får med första gången
            ViewData["ReturnUrl"] = returnUrl;
            return View();
        }

        public IActionResult Kontakt()
        {
            //Ser till så att databasen inte blir överbelastad genom att stämga förbindelsen när man avslutar sessionen
            using (PersonalContext db = new PersonalContext())
            {
                //Skapar en personallista
                List<Personalen> personallista = db.Personalen.ToList();
                return View(personallista);
            }
        }

        public IActionResult Om(int Id)
        {
            using(PersonalContext db = new PersonalContext())
            {
                Personalen personalen = db.Personalen.Find(Id);
                return View(personalen);
            }
        }

        [HttpPost]
        public async Task<IActionResult> Index(HomeModell homeModell, string returnUrl="")
        {
            //Kontrollerar resultatet av kollen på användarnamn och lösenordet
            bool validUser = CheckUser(homeModell);

            //Stämmer användarnamnet och lösenordet ska detta ske:
            if(validUser == true)
            {
                //Allt stämmer, logga in användaren
                var identity = new ClaimsIdentity(CookieAuthenticationDefaults.AuthenticationScheme);
                identity.AddClaim(new Claim(ClaimTypes.Name, homeModell.UserName));

                //Loggar in användaren asynkront, eftersom det kan ta en stud att logga in användaren så frysr vi inte sidan medans man blir inloggad
                await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, new ClaimsPrincipal(identity));

                //Om man får med en returnUrl kommer man till den, om inte kommer man till kundsidan när man är inloggad
                if (returnUrl != "")
                    return Redirect(returnUrl);
                else
                    return RedirectToAction("Index", "KundSida");
            }
            // Stämmer inte användarnamnet och lösenordet ska detta ske:
            else
            {
                ViewBag.ErrorMessage = "Inloggningen godkänndes inte";

                //Ser till så man inte tappar bort return url:en om inloggningen misslyckades
                ViewData["ReturnUrl"] = returnUrl;
                return View();
            }
        }

        //Skickar användarnamn och lösenord. Kollar så att dem stämmer
        private bool CheckUser(HomeModell homeModell)
        {
            if (homeModell.UserName.ToUpper() =="ADMIN" && homeModell.Password=="pwd")
                return true;
            else
                return false;
        }


        
    }
}

