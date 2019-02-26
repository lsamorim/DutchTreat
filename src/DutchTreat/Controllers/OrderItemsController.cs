using AutoMapper;
using DutchTreat.Data;
using DutchTreat.Data.Entities;
using DutchTreat.Data.Repository;
using DutchTreat.Data.UoW;
using DutchTreat.ViewModels;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace DutchTreat.Controllers
{
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    [Route("api/orders/{orderId}/items")]
    public class OrderItemsController : ControllerBase
    {
        private readonly UserManager<StoreUser> _userManager;
        private readonly ILogger<OrderItemsController> _logger;
        private readonly IUnitOfWork _uow;
        private readonly IRepository<Order> _orderRepository;
        private readonly IMapper _mapper;

        public OrderItemsController(UserManager<StoreUser> userManager,
            ILogger<OrderItemsController> logger,
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
        public ActionResult<IEnumerable<OrderItemViewModel>> Get(int orderId)
        {
            try
            {
                var order = _orderRepository.GetFirst(
                    o =>
                    o.User.UserName == User.Identity.Name &&
                    o.Id == orderId,
                    includeProperties: "Items");

                if (order != null)
                {
                    var orderItemsViewModel = _mapper.Map<IEnumerable<OrderItemViewModel>>(order.Items);
                    return Ok(orderItemsViewModel);
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

        [HttpGet("{id:int}")]
        public ActionResult<OrderItemViewModel> Get(int orderId, int id)
        {
            try
            {
                var order = _orderRepository.GetFirst(
                   o =>
                   o.User.UserName == User.Identity.Name &&
                   o.Id == orderId,
                   includeProperties: "Items");

                if (order != null)
                {
                    var orderItem = order.Items.Where(i => i.Id == id).FirstOrDefault();

                    if (orderItem != null)
                    {
                        return Ok(_mapper.Map<OrderItemViewModel>(orderItem));
                    }
                    else
                    {
                        return NotFound();
                    }
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
        public async Task<ActionResult<OrderItemViewModel>> Post([FromRoute] int orderId, [FromBody] OrderItemViewModel orderItemViewModel)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    using (_uow)
                    {
                        var order = _orderRepository.GetFirst(
                       o =>
                       o.User.UserName == User.Identity.Name &&
                       o.Id == orderId,
                       includeProperties: "Items");

                        if (order != null)
                        {
                            var orderItem = _mapper.Map<OrderItem>(orderItemViewModel);
                            order.Items.Add(orderItem);
                            _orderRepository.Update(order);

                            if (await _uow.CommitAsync())
                            {
                                orderItemViewModel = _mapper.Map<OrderItemViewModel>(orderItem);

                                return Created($"{HttpContext.Request.Path}/{orderItemViewModel.Id}", orderItemViewModel);
                            }
                        }
                        else
                        {
                            return NotFound();
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
