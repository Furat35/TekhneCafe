using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using TekhneCafe.Api.ActionFilters;
using TekhneCafe.Business.Abstract;
using TekhneCafe.Business.ValidationRules.FluentValidations.Category;
using TekhneCafe.Core.Consts;
using TekhneCafe.Core.DTOs.Category;

namespace TekhneCafe.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize]
    public class CategoryController : ControllerBase
    {
        private readonly ICategoryService _categoryService;

        public CategoryController(ICategoryService categoryService)
        {
            _categoryService = categoryService;
        }

        /// <summary>
        /// Creates a new category.
        /// </summary>
        /// <param name="categoryAddDto">The data for the new category.</param>
        /// <returns>A response indicating the result of the category creation.</returns>
        /// <response code="201">Category created</response>
        /// <response code="400">Invalid category</response>
        /// <response code="500">Server error</response>
        [HttpPost]
        [Authorize(Roles = $"{RoleConsts.CafeService}, {RoleConsts.CafeAdmin}")]
        [TypeFilter(typeof(FluentValidationFilterAttribute<CategoryAddDtoValidator, CategoryAddDto>), Arguments = new object[] { "categoryAddDto" })]
        public async Task<IActionResult> CreateCategory(CategoryAddDto categoryAddDto)
        {
            await _categoryService.CreateCategoryAsync(categoryAddDto);
            return StatusCode(StatusCodes.Status201Created);
        }

        /// <summary>
        /// Retrieves a list of all categories.
        /// </summary>
        /// <returns>A list of categories.</returns>
        /// <response code="200">Success</response>
        /// <response code="400">Bad request</response>
        /// <response code="500">Server error</response>
        [HttpGet]
        [Authorize]
        public IActionResult GetAllCategories()
        {
            var categories = _categoryService.GetAllCategory();
            return Ok(categories);
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
        public async Task<IActionResult> GetCategoryById(string id)
        {
            var attribute = await _categoryService.GetCategoryByIdAsync(id);
            return Ok(attribute);
        }

        /// <summary>
        /// Deletes a category by its ID.
        /// </summary>
        /// <param name="id">The ID of the category to delete.</param>
        /// <returns>A message indicating the success of the deletion.</returns>
        /// <response code="200">Success</response>
        /// <response code="500">Server error</response>
        [HttpDelete("{id}")]
        [Authorize(Roles = $"{RoleConsts.CafeService}, {RoleConsts.CafeAdmin}")]
        [TypeFilter(typeof(ModelValidationFilterAttribute), Arguments = new object[] { "id" })]
        public async Task<IActionResult> DeleteCategory(string id)
        {
            await _categoryService.DeleteCategoryAsync(id);
            return Ok();
        }

        /// <summary>
        /// Updates an attribute.
        /// </summary>
        /// <param name="categoryUpdateDto">The DTO containing the updated attribute data.</param>
        /// <returns>A message indicating the success of the update.</returns>
        [HttpPut]
        [Authorize(Roles = $"{RoleConsts.CafeService}, {RoleConsts.CafeAdmin}")]
        [TypeFilter(typeof(FluentValidationFilterAttribute<CategoryUpdateDtoValidator, CategoryUpdateDto>), Arguments = new object[] { "categoryUpdateDto" })]

        public async Task<IActionResult> UpdateCategory([FromBody] CategoryUpdateDto categoryUpdateDto)
        {
            await _categoryService.UpdateCategoryAsync(categoryUpdateDto);
            return Ok();
        }
    }
}