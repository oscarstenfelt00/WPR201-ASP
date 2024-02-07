using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Slutprojekt_ASP.Net.Models;

// For more information on enabling MVC for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace Slutprojekt_ASP.Net.Controllers
{
    //Överlämmnar inloggningen till ASP MVC, ASP får hålla reda på Cookies, Sessioner osv

    //De metoder som finns under authorize är låst, man kan låsa enskillda sidor om man vill
    [Authorize]
    public class KundSidaController : Controller
    {
        // GET: /<controller>/
        public IActionResult Index()
        {

            return View();
        }

        public async Task<IActionResult> SignOutUser()
        {
            await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
            return RedirectToAction("Index", "Home");
        }

        public IActionResult Personal()
        {
            //Ser till så att databasen inte blir överbelastad genom att stämga förbindelsen när man avslutar sessionen
            using (PersonalContext db = new PersonalContext() )
            {
                //Skapar en personallista
                List<Personalen> personallista = db.Personalen.ToList();

                return View(personallista);
            }  
        }

        [HttpGet]
        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Create(Personalen nyPersonal, Personalen newpic)
        {

            if (newpic.uploadedimage == null) // Ingen bild, välj standardbild på servern
            {
                newpic.image = System.IO.File.ReadAllBytes("./Pictures/netsurf.png");
            }
            else            // Användaren har valt en bild i formuläret (som det skall vara)
            {
                
                using (var memoryStream = new System.IO.MemoryStream())
                {
                    await newpic.uploadedimage.CopyToAsync(memoryStream);
                    newpic.image = memoryStream.ToArray();
                }
            }

            using (PersonalContext db = new PersonalContext())
            {
                db.Personalen.Add(nyPersonal);
                //Spara Ändringar
                db.SaveChanges();
            }
            using (PersonalContext db = new PersonalContext())
            {
                db.Personalen.Add(newpic);
                //Spara Ändringar
                db.SaveChanges();
            }
            //Återgå till listan
            return RedirectToAction("Personal", "KundSida");
        }

        //Anropa ett visst Id
        public IActionResult Edit(int Id)
        {
            Personalen pic = new Personalen();
            using (PersonalContext db = new PersonalContext())
            {
                //Söker rätt på ett visst Id
                Personalen personalen = db.Personalen.Find(Id);
                pic = db.Personalen.Find(Id);
                //retunerar id till vyn
                return View(personalen);
            }
        }

        [HttpPost]
        public IActionResult Edit(Personalen personalen, Personalen newpic)
        {
            using (PersonalContext db = new PersonalContext())
            {
                //Uppdatera personalen
                db.Update(personalen);
                db.Update(newpic);
                //Spara Ändringar
                db.SaveChanges();
                //Återgå till listan
                return RedirectToAction("Personal", "KundSida");
            }
        }

        public IActionResult Delete(int Id)
        {
            using (PersonalContext db = new PersonalContext())
            {
                //Söker rätt på ett visst Id
                Personalen personalen = db.Personalen.Find(Id);
                //retunerar id till vyn
                return View(personalen);
            }
        }

        [HttpPost]
        public IActionResult Delete(Personalen personalen)
        {
            using (PersonalContext db = new PersonalContext())
            {
                //Ta bort personalen från listan
                db.Personalen.Remove(personalen);
                //Spara Ändringar
                db.SaveChanges();
                //Återgå till listan
                return RedirectToAction("Personal", "KundSida");
            }
        }
    }
}

