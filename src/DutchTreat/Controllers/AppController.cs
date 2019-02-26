using DutchTreat.Data;
using DutchTreat.Data.Entities;
using DutchTreat.Data.Repository;
using DutchTreat.Services;
using DutchTreat.ViewModels;
using Microsoft.AspNetCore.Authorization;
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
        private readonly IRepository<Product> _productRepository;

        public AppController(IMailService mailService, IRepository<Product> productRepository)
        {
            _mailService = mailService;
            _productRepository = productRepository;
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

        [Authorize]
        [HttpGet("shop")]
        public IActionResult Shop()
        {
            var results = _productRepository.GetAll(o => o.OrderBy(x => x.Title));

            return View(results.ToList());
        }
    }
}
