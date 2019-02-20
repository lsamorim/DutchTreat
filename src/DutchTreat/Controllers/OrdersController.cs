using AutoMapper;
using DutchTreat.Data;
using DutchTreat.Data.Entities;
using DutchTreat.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Net;

namespace DutchTreat.Controllers
{
    [Route("api/[Controller]")]
    public class OrdersController : ControllerBase
    {
        private readonly IDutchRepository _repository;
        private readonly ILogger<OrdersController> _logger;
        private readonly IMapper _mapper;

        public OrdersController(IDutchRepository repository, ILogger<OrdersController> logger, IMapper mapper)
        {
            _repository = repository;
            _logger = logger;
            _mapper = mapper;
        }

        [HttpGet]
        //[ProducesResponseType(200)]
        //[ProducesResponseType(500)]
        public ActionResult<IEnumerable<OrderViewModel>> Get()
        {
            try
            {
                var orders = _repository.GetAllOrders();
                var orderViewModels = _mapper.Map<IEnumerable<OrderViewModel>>(orders);
                return Ok(orderViewModels);
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
                var order = _repository.GetOrderById(id);

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
        public ActionResult<OrderViewModel> Post([FromBody] OrderViewModel orderViewModel)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    var order = _mapper.Map<OrderViewModel, Order>(orderViewModel);

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
