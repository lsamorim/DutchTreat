using AutoMapper;
using DutchTreat.Data;
using DutchTreat.Data.Entities;
using DutchTreat.Data.Repository;
using DutchTreat.Data.UoW;
using DutchTreat.Services;
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
        private readonly UserManager<StoreUser> _userManager;
        private readonly ILogger<OrdersController> _logger;
        private readonly IUnitOfWork _uow;
        private readonly IRepository<Order> _orderRepository;
        private readonly IMapper _mapper;

        public OrdersController(UserManager<StoreUser> userManager,
            ILogger<OrdersController> logger,
            IUnitOfWork uow,
            IRepository<Order> orderRepository,
            IMapper mapper)
        {
            _userManager = userManager;
            _logger = logger;
            _uow = uow;
            _orderRepository = orderRepository;
            _mapper = mapper;
        }

        [HttpGet]
        //[ProducesResponseType(200)]
        //[ProducesResponseType(500)]
        public ActionResult<IEnumerable<OrderViewModel>> Get()
        {
            try
            {
                var username = User.Identity.Name;

                var orders = _orderRepository.Get(
                    f => f.User.UserName == username,
                    includeProperties: "Items");

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
                var order = _orderRepository.Get(
                    o =>
                    o.User.UserName == User.Identity.Name &&
                    o.Id == id,
                    includeProperties: "Items");

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
                    
                    using (_uow)
                    {
                        _orderRepository.Create(order);
                        if (await _uow.CommitAsync())
                        {
                            orderViewModel = _mapper.Map<Order, OrderViewModel>(order);

                            return Created($"{HttpContext.Request.Path}/{orderViewModel.OrderId}", orderViewModel);
                        }
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
