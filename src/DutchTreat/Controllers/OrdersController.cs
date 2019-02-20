using AutoMapper;
using DutchTreat.Data;
using DutchTreat.Data.Entities;
using DutchTreat.ViewModels;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Net;
using System.Threading.Tasks;

namespace DutchTreat.Controllers
{
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    [Route("api/[Controller]")]
    public class OrdersController : ControllerBase
    {
        private readonly IDutchRepository _repository;
        private readonly ILogger<OrdersController> _logger;
        private readonly IMapper _mapper;
        private readonly UserManager<StoreUser> _userManager;

        public OrdersController(IDutchRepository repository, ILogger<OrdersController> logger, IMapper mapper,
            UserManager<StoreUser> userManager)
        {
            _repository = repository;
            _logger = logger;
            _mapper = mapper;
            _userManager = userManager;
        }

        [HttpGet]
        //[ProducesResponseType(200)]
        //[ProducesResponseType(500)]
        public ActionResult<IEnumerable<OrderViewModel>> Get()
        {
            try
            {
                var username = User.Identity.Name;

                var orders = _repository.GetAllOrdersByUser(username);
                var ordersViewModel = _mapper.Map<IEnumerable<OrderViewModel>>(orders);
                return Ok(ordersViewModel);
            }
            catch (Exception ex)
            {
                _logger.LogError($"Something wrong happen: {ex}");
                return StatusCode((int)HttpStatusCode.InternalServerError);
            }
        }

        [HttpGet("{id:int}")]
        public ActionResult<OrderViewModel> Get(int id)
        {
            try
            {
                var order = _repository.GetOrderById(User.Identity.Name, id);

                if (order != null)
                {
                    var orderViewModel = _mapper.Map<OrderViewModel>(order);
                    return Ok(orderViewModel);
                }
                else
                {
                    return NotFound();
                }
            }
            catch (Exception ex)
            {
                _logger.LogError($"Something wrong happen: {ex}");
                return StatusCode((int)HttpStatusCode.InternalServerError);
            }
        }

        [HttpPost]
        public async Task<ActionResult<OrderViewModel>> Post([FromBody] OrderViewModel orderViewModel)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    var currentUser = await _userManager.FindByNameAsync(User.Identity.Name);
                    var order = _mapper.Map<OrderViewModel, Order>(orderViewModel);
                    order.User = currentUser;
                    _repository.AddEntity(order);
                    if (_repository.SaveChanges())
                    {
                        orderViewModel = _mapper.Map<Order, OrderViewModel>(order);

                        return Created($"{HttpContext.Request.Path}/{orderViewModel.OrderId}", orderViewModel);
                    }
                }
                else
                {
                    return BadRequest(ModelState);
                }

            }
            catch (Exception ex)
            {
                _logger.LogError($"Something wrong happen: {ex}");
                return StatusCode((int)HttpStatusCode.InternalServerError);
            }

            return BadRequest();
        }
    }
}
