using FluentValidation;
using Microsoft.AspNetCore.Mvc;
using PractTest.API.Shared;
using PractTest.Application.DTOs;
using PractTest.Application.Interfaces.Customers;

namespace PractTest.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CustomersController : ControllerBase
    {
        private readonly ICustomerService _customerService;
        private readonly ILogger<CustomersController> _logger;
        private readonly IValidator<CreateCustomerDto> _createCustomerValidator;

        public CustomersController(ICustomerService customerService, ILogger<CustomersController> logger, IValidator<CreateCustomerDto> createCustomerValidator)
        {
            _customerService = customerService;
            _logger = logger;
            _createCustomerValidator = createCustomerValidator;
        }

        [HttpGet]
        [ProducesResponseType(typeof(APIResponse<IEnumerable<CustomerDto>>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(APIResponse<string>), StatusCodes.Status204NoContent)]
        [ProducesResponseType(typeof(APIResponse<string>), StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult> GetAllCustomers()
        {
            try
            {
                var customers = await _customerService.GetAllCustomerAsync();
                if (!customers.Any())
                {
                    return StatusCode(StatusCodes.Status204NoContent, APIResponse<string>.Error("No customers found", string.Empty, StatusCodes.Status204NoContent));
                }
                return Ok(APIResponse<IEnumerable<CustomerDto>>.Success(customers, "Customers retrieved successfully."));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message);
                return StatusCode(StatusCodes.Status500InternalServerError, APIResponse<string>.Error("Error in retrieving customers", ex.Message));
            }
        }

        [Route("{id}")]
        [HttpGet]
        [ProducesResponseType(typeof(APIResponse<CustomerDto>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(APIResponse<string>), StatusCodes.Status404NotFound)]
        [ProducesResponseType(typeof(APIResponse<string>), StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult> GetCustomerById([FromRoute] int id)
        {
            try
            {
                var customer = await _customerService.GetCustomerByIdAsync(id);
                if (customer is null)
                {
                    return StatusCode(StatusCodes.Status404NotFound, APIResponse<string>.Error("No customer found.", string.Empty, StatusCodes.Status404NotFound));
                }
                return Ok(APIResponse<CustomerDto>.Success(customer, "Customer retrieved successfully."));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error in retrieving customer with ID {CustomerId}", id);
                return StatusCode(StatusCodes.Status500InternalServerError, APIResponse<string>.Error("Error in retrieving customer", ex.Message));
            }
        }

        [HttpPost]
        [ProducesResponseType(typeof(APIResponse<CustomerDto>), StatusCodes.Status201Created)]
        [ProducesResponseType(typeof(APIResponse<string>), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(APIResponse<string>), StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult> CreateCustomer([FromBody] CreateCustomerDto customerDto)
        {
            try
            {
                var validatorResult = await _createCustomerValidator.ValidateAsync(customerDto);
                if (!validatorResult.IsValid)
                {
                    return BadRequest(APIResponse<IList<FluentValidation.Results.ValidationFailure>>.Error("Validation errors.", validatorResult.Errors));
                }

                var createdCustomer = await _customerService.CreateCustomerAsync(customerDto);
                if (createdCustomer is null)
                {
                    //return StatusCode(StatusCodes.Status304NotModified, APIResponse<string>.Error("Customer not created.", string.Empty, StatusCodes.Status304NotModified));
                    return BadRequest(APIResponse<string>.Error("Customer not created.", string.Empty));
                }
                //return Ok(APIResponse<CustomerDto>.Success(createdCustomer, "Customer created successfully"));
                return CreatedAtAction(nameof(CreateCustomer), new { id = createdCustomer.Id }, APIResponse<CustomerDto>.Success(createdCustomer, "Customer created successfully"));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error creating customer");
                return StatusCode(StatusCodes.Status500InternalServerError, APIResponse<string>.Error("Error in creating customer", ex.Message));
            }
        }

        [HttpPut("{id}")]
        [ProducesResponseType(typeof(APIResponse<CustomerDto>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(APIResponse<string>), StatusCodes.Status404NotFound)]
        [ProducesResponseType(typeof(APIResponse<string>), StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult> UpdateCustomer([FromRoute] int id, [FromBody] UpdateCustomerDto customerDto)
        {
            try
            {
                var result = await _customerService.UpdateCustomerAsync(id, customerDto);
                if (result is null)
                {
                    return NotFound(APIResponse<string>.Error("Customer not found or update failed.", string.Empty));
                }
                return Ok(APIResponse<CustomerDto>.Success(result, "Customer updated successfully."));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error updating customer");
                return StatusCode(StatusCodes.Status500InternalServerError, APIResponse<string>.Error("Error in updating customer", ex.Message));
            }
        }

        [HttpPatch("{id}")]
        [ProducesResponseType(typeof(APIResponse<CustomerDto>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(APIResponse<string>), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(APIResponse<string>), StatusCodes.Status404NotFound)]
        [ProducesResponseType(typeof(APIResponse<string>), StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult> UpdateLoyaltyPoints([FromRoute] int id, [FromQuery] int loyalty)
        {
            if (loyalty < 0)
            {
                return BadRequest(APIResponse<string>.Error("Loyalty points should be positive.", string.Empty));
            }
            try
            {
                var result = await _customerService.UpdateLoyaltyPointsAsync(id, loyalty);
                if (result is null)
                {
                    return NotFound(APIResponse<string>.Error("Customer not found or update failed.", string.Empty));
                }
                return Ok(APIResponse<CustomerDto>.Success(result, "Loyalty points updated successfully."));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error updating loyalty points");
                return StatusCode(StatusCodes.Status500InternalServerError, APIResponse<string>.Error("Error in updating loyalty points", ex.Message));
            }
        }
    }
}
