using Api.Attributes;
using Application.Commands.Products;
using Domain.AggregatesModel.ProductAggregate;
using Domain.Dtos.Product;
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
    [ApiExplorerSettings(GroupName = "management-product")]
    public class ProductsController : ControllerBase
    {
        private readonly IMediator _mediator;
        private readonly ILogger _logger;

        public ProductsController(ILogger<ProductsController> logger, IMediator mediator)
        {
            _logger = logger;
            _mediator = mediator;
        }

        [HttpPost("")]
        [ProducesResponseType(StatusCodes.Status201Created)]
        public async Task<IActionResult> CreateProduct([FromBody] CreateProductCommand command)
        {
            try
            {
                ProductDto response = await _mediator.Send(command);
                return Created("ProductGet",new { id = response.Id });
            }
            catch (ArgumentException ex)
            {
                _logger.LogError(ex, message: "An error occurred in the Create Product method");
                return BadRequest(ex.Message);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, message: "An error occurred in the Create Product method");
                return StatusCode(500, $"An error has occurred processing the transaction.");
            }
        }

        [HttpGet("")]
        [ProducesResponseType(typeof(Product), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        public async Task<IActionResult> GetAll([FromQuery] int size = 10, [FromQuery] int offset = 1)
        {
            try
            {
                GetProductCommand command = new GetProductCommand()
                {
                    Size = size,
                    Offset = offset
                };

                IEnumerable<Product> response = await _mediator.Send(command);
                return Ok(response);
            }
            catch (ArgumentException ex)
            {
                _logger.LogError(ex, message: "An error occurred in the Get All Product method");
                return BadRequest(ex.Message);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, message: "An error occurred in the Get All Product method");
                return StatusCode(500, $"An error has occurred processing the transaction.");
            }
        }
    }
}