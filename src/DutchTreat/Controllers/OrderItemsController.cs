using AutoMapper;
using DutchTreat.Data;
using DutchTreat.Data.Entities;
using DutchTreat.ViewModels;
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
    [Route("api/orders/{orderId}/items")]
    public class OrderItemsController : ControllerBase
    {
        private readonly IDutchRepository _repository;
        private readonly ILogger<OrderItemsController> _logger;
        private readonly IMapper _mapper;

        public OrderItemsController(IDutchRepository repository, ILogger<OrderItemsController> logger, IMapper mapper)
        {
            _repository = repository;
            _logger = logger;
            _mapper = mapper;
        }

        [HttpGet]
        public ActionResult<IEnumerable<OrderItemViewModel>> Get(int orderId)
        {
            try
            {
                var order = _repository.GetOrderById(orderId);

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
                var order = _repository.GetOrderById(orderId);

                if (order != null)
                {
                    var orderItem = order.Items.Where(i => i.Id == id).FirstOrDefault();

                    if(orderItem != null)
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
        public ActionResult<OrderItemViewModel> Post([FromRoute] int orderId, [FromBody] OrderItemViewModel orderItemViewModel)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    var order = _repository.GetOrderById(orderId);

                    if (order != null)
                    {
                        var orderItem = _mapper.Map<OrderItem>(orderItemViewModel);
                        order.Items.Add(orderItem);
                        
                        if (_repository.SaveChanges())
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
