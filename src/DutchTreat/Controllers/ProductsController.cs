using DutchTreat.Data;
using DutchTreat.Data.Entities;
using DutchTreat.Data.Repository;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Net;

namespace DutchTreat.Controllers
{
    [Route("api/[Controller]")]
    //[ApiController]
    //[Produces("application/json")]
    public class ProductsController : ControllerBase
    {
        private readonly ILogger<ProductsController> _logger;
        private readonly IRepository<Product> _productRepository;

        public ProductsController(ILogger<ProductsController> logger,
            IRepository<Product> productRepository)
        {
            _logger = logger;
            _productRepository = productRepository;
        }

        [HttpGet]
        //[ProducesResponseType(200)]
        //[ProducesResponseType(500)]
        public ActionResult<IEnumerable<Product>> Get()
        {
            try
            {
                return Ok(_productRepository.GetAll());
            }
            catch (Exception ex)
            {
                _logger.LogError($"Something wrong happen: {ex}");
                return StatusCode((int)HttpStatusCode.InternalServerError);
            }
        }
    }
}
