using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using TekhneCafe.Api.ActionFilters;
using TekhneCafe.Business.Abstract;
using TekhneCafe.Business.ValidationRules.FluentValidations.Attribute;
using TekhneCafe.Core.Consts;
using TekhneCafe.Core.DTOs.Attribute;
using TekhneCafe.Core.Filters.Attribute;

namespace TekhneCafe.Api.Controllers
{
    /// <summary>
    /// Controller to manage attributes.
    /// </summary>
    [ApiController]
    [Route("api/[controller]")]
    [Authorize]
    public class AttributeController : ControllerBase
    {
        private readonly IAttributeService _attributeService;

        public AttributeController(IAttributeService attributeService)
        {
            _attributeService = attributeService;
        }

        /// <summary>
        /// Creates a new attribute.
        /// </summary>
        /// <param name="attributeAddDto">The data for the new attribute.</param>
        /// <returns>A response indicating the result of the attribute creation.</returns>
        ///  <response code="201">Attribute created</response>
        /// <response code="400">Invalid attribute</response>
        /// <response code="500">Server error</response>
        [HttpPost]
        [Authorize(Roles = $"{RoleConsts.CafeService}, {RoleConsts.CafeAdmin}")]
        [TypeFilter(typeof(FluentValidationFilterAttribute<AttributeAddDtoValidator, AttributeAddDto>), Arguments = new object[] { "attributeAddDto" })]
        public async Task<IActionResult> CreateAttribute(AttributeAddDto attributeAddDto)
        {
            await _attributeService.CreateAttributeAsync(attributeAddDto);
            return StatusCode(StatusCodes.Status201Created);
        }

        /// <summary>
        /// Retrieves a list of all attributes.
        /// </summary>
        /// <returns>A list of attributes.</returns>
        /// <response code="200">Success</response>
        /// <response code="400">Bad request</response>
        /// <response code="500">Server error</response>
        [HttpGet]
        [Authorize]
        public IActionResult GetAllAttributes([FromQuery] AttributeRequestFilter filters)
        {
            var attributes = _attributeService.GetAllAttributes(filters);
            return Ok(attributes);
        }

        /// <summary>
        /// Retrieves an attribute by its ID.
        /// </summary>
        /// <param name="id">The ID of the attribute.</param>
        /// <returns>The attribute with the specified ID.</returns>  
        /// <response code="200">Success</response>
        /// <response code="404">Attribute not found</response>
        /// <response code="500">Server error</response>
        [HttpGet("{id}")]
        [Authorize]
        [TypeFilter(typeof(ModelValidationFilterAttribute), Arguments = new object[] { "id" })]
        public async Task<IActionResult> GetAttributeById(string id)
        {
            var attribute = await _attributeService.GetAttributeByIdAsync(id);
            return Ok(attribute);
        }

        /// <summary>
        /// Deletes an attribute by its ID.
        /// </summary>
        /// <param name="id">The ID of the attribute to delete.</param>
        /// <returns>A message indicating the success of the deletion.</returns>
        [HttpDelete("{id}")]
        [Authorize(Roles = $"{RoleConsts.CafeService}, {RoleConsts.CafeAdmin}")]
        [TypeFilter(typeof(ModelValidationFilterAttribute), Arguments = new object[] { "id" })]
        public async Task<IActionResult> DeleteAttribute(string id)
        {
            await _attributeService.DeleteAttributeAsync(id);
            return Ok();
        }

        /// <summary>
        /// Updates an attribute.
        /// </summary>
        /// <param name="attributeUpdateDto">The DTO containing the updated attribute data.</param>
        /// <returns>A message indicating the success of the update.</returns>
        [HttpPut]
        [Authorize(Roles = $"{RoleConsts.CafeService}, {RoleConsts.CafeAdmin}")]
        [TypeFilter(typeof(FluentValidationFilterAttribute<AttributeUpdateDtoValidator, AttributeUpdateDto>), Arguments = new object[] { "attributeUpdateDto" })]
        public async Task<IActionResult> UpdateAttribute([FromBody] AttributeUpdateDto attributeUpdateDto)
        {
            await _attributeService.UpdateAttributeAsync(attributeUpdateDto);
            return Ok();
        }
    }
}