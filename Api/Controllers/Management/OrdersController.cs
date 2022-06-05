using Api.Attributes;
using Application.Commands.Orders;
using Domain.AggregatesModel.OrderAggregate;
using Domain.Dtos.Order;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Api.Controllers.Management
{
    [ApiController]
    [Route("management/[controller]")]
    [ApiExplorerSettings(GroupName = "management-order")]
    public class OrdersController : ControllerBase
    {
        private readonly IMediator _mediator;
        private readonly ILogger _logger;

        public OrdersController(ILogger<OrdersController> logger, IMediator mediator)
        {
            _logger = logger;
            _mediator = mediator;
        }

        [HttpGet("{Id}", Name = "OrderGet")]
        [ProducesResponseType(typeof(Order), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        public async Task<IActionResult> GetById([FromRoute] GetOrderByIdCommand command)
        {
            try
            {
                Order response = await _mediator.Send(command);
                return Ok(response);
            }
            catch (ArgumentException ex)
            {
                _logger.LogError(ex, message: "An error occurred in the GetById Order method");
                return BadRequest(ex.Message);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, message: "An error occurred in the GetById Order method");
                return StatusCode(500, $"An error has occurred processing the transaction.");
            }
        }

        [HttpGet("")]
        [ProducesResponseType(typeof(Order), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        public async Task<IActionResult> GetAll([FromQuery] int size = 10, [FromQuery] int offset = 1)
        {
            try
            {
                GetOrderCommand command = new GetOrderCommand()
                {
                    Size = size,
                    Offset = offset
                };

                IEnumerable<Order> response = await _mediator.Send(command);
                return Ok(response);
            }
            catch (ArgumentException ex)
            {
                _logger.LogError(ex, message: "An error occurred in the Get All Order method");
                return BadRequest(ex.Message);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, message: "An error occurred in the Get All Order method");
                return StatusCode(500, $"An error has occurred processing the transaction.");
            }
        }

        [HttpPost("")]
        [ProducesResponseType(StatusCodes.Status201Created)]
        public async Task<IActionResult> CreateOrder([FromBody] CreateOrderCommand command)
        {
            try
            {
                OrderDto response = await _mediator.Send(command);
                return CreatedAtRoute("OrderGet", new { id = response.Queue }, response);
            }
            catch (ArgumentException ex)
            {
                _logger.LogError(ex, message: "An error occurred in the Create Order method");
                return BadRequest(ex.Message);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, message: "An error occurred in the Create Order method");
                return StatusCode(500, $"An error has occurred processing the transaction.");
            }
        }

        [HttpPut]
        [ExistsOrder]
        [PutConsistenceAttribute]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        public async Task<IActionResult> UpdateOrder([FromBody] UpdateOrderCommand command)
        {
            try
            {
                OrderDto response = await _mediator.Send(command);
                return Ok(response);
            }
            catch (ArgumentException ex)
            {
                _logger.LogError(ex, message: "An error occurred in the Update Order method");
                return BadRequest(ex.Message);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, message: "An error occurred in the Update Order method");
                return StatusCode(500, $"An error has occurred processing the transaction.");
            }
        }

        [HttpDelete("{Id}")]
        [ExistsOrder]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        public async Task<IActionResult> DeleteOrder([FromRoute] DeleteOrderCommand command)
        {
            try
            {
                var response = await _mediator.Send(command);
                return Ok(response);
            }
            catch (ArgumentException ex)
            {
                _logger.LogError(ex, message: "An error occurred in the  Delete Order method");
                return BadRequest(ex.Message);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, message: "An error occurred in the  Delete Order method");
                return StatusCode(500, $"An error has occurred processing the transaction.");
            }
        }

        [HttpPatch("{Id}")]
        [ProducesResponseType(typeof(Order), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        public async Task<IActionResult> CallOrder([FromRoute] CallOrderCommand command)
        {
            try
            {
                OrderDto response = await _mediator.Send(command);
                return Ok(response);
            }
            catch (ArgumentException ex)
            {
                _logger.LogError(ex, message: "An error occurred in the Call Order method");
                return BadRequest(ex.Message);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, message: "An error occurred in the Call  Order method");
                return StatusCode(500, $"An error has occurred processing the transaction.");
            }
        }
    }
}