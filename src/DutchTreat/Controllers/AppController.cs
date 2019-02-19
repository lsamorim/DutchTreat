using DutchTreat.Services;
using DutchTreat.ViewModels;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DutchTreat.Controllers
{
    public class AppController : Controller
    {
        private readonly IMailService _mailService;

        public AppController(IMailService mailService)
        {
            _mailService = mailService;
        }

        public IActionResult Index()
        {
            return View();
        }

        [HttpGet("contact")]
        public IActionResult Contact()
        {
            //throw new InvalidOperationException("Bad thing happen");
            return View();
        }

        [HttpPost("contact")]
        public IActionResult Contact(ContactViewModel model)
        {
            if (ModelState.IsValid)
            {
                // Send the email
                _mailService.SendMail("lsamorim.it@gmail.com", model.Subject, $"From: {model.Name}, Message: {model.Message}");
                ViewBag.UserMessage = "Mail Sent!";
                ModelState.Clear();
            }

            return View();
        }

        public IActionResult About()
        {
            return View();
        }
    }
}
