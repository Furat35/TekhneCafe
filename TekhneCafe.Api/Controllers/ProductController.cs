using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using TekhneCafe.Api.ActionFilters;
using TekhneCafe.Business.Abstract;
using TekhneCafe.Business.ValidationRules.FluentValidations.Product;
using TekhneCafe.Core.Consts;
using TekhneCafe.Core.DTOs.Product;
using TekhneCafe.Core.Filters.Product;

namespace TekhneCafe.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class ProductController : ControllerBase
    {
        private readonly IProductService _productService;

        public ProductController(IProductService productService)
        {
            _productService = productService;
        }

        /// <summary>
        /// Gets a product by its ID.
        /// </summary>
        /// <param name="id">The ID of the product to retrieve.</param>
        /// <returns>The product details.</returns>
        /// <response code="200">Success</response>
        /// <response code="400">Bad request</response>
        /// <response code="404">Product not found</response>
        /// <response code="500">Server error</response>
        [HttpGet("{id}")]
        [TypeFilter(typeof(ModelValidationFilterAttribute), Arguments = new object[] { "id" })]
        [Authorize]
        public async Task<IActionResult> GetProductById(string id)
        {
            ProductDetailDto productList = await _productService.GetProductByIdAsync(id);
            return Ok(productList);
        }

        /// <summary>
        /// Creates a new product.
        /// </summary>
        /// <param name="productAddDto">The DTO containing the product details to create.</param>
        /// <returns>A message indicating the success of the creation.</returns>
        /// <response code="200">Success</response>
        /// <response code="400">Bad request</response>
        /// <response code="500">Server error</response>
        [HttpPost]
        [Route("create")]
        [Authorize(Roles = $"{RoleConsts.CafeService}, {RoleConsts.CafeAdmin}")]
        [TypeFilter(typeof(FluentValidationFilterAttribute<ProductAddDtoValidator, ProductAddDto>), Arguments = new object[] { "productAddDto" })]
        public async Task<IActionResult> CreateProduct([FromForm] ProductAddDto productAddDto)
        {
            await _productService.CreateProductAsync(productAddDto);
            return StatusCode(StatusCodes.Status201Created);
        }

        /// <summary>
        /// Gets all products.
        /// </summary>
        /// <returns>The list of all products.</returns>
        [HttpGet]
        [Authorize]
        public IActionResult GetAllProducts([FromQuery] ProductRequestFilter filter)
        {
            var products = _productService.GetAllProducts(filter);
            return Ok(products);
        }

        /// <summary>
        /// Deletes a product by its ID.
        /// </summary>
        /// <param name="id">The ID of the product to delete.</param>
        /// <returns>A message indicating the success of the deletion.</returns>
        [HttpDelete("{id}")]
        [Authorize(Roles = $"{RoleConsts.CafeService}, {RoleConsts.CafeAdmin}")]
        [TypeFilter(typeof(ModelValidationFilterAttribute), Arguments = new object[] { "id" })]
        public async Task<IActionResult> DeleteProduct(string id)
        {

            await _productService.DeleteProductAsync(id);
            return Ok();

        }

        /// <summary>
        /// Updates a product.
        /// </summary>
        /// <param name="productUpdateDto">The DTO containing the updated product data.</param>
        /// <returns>A message indicating the success of the update.</returns>
        [HttpPut]
        [Authorize(Roles = $"{RoleConsts.CafeService}, {RoleConsts.CafeAdmin}")]
        [TypeFilter(typeof(FluentValidationFilterAttribute<ProductUpdateDtoValidator, ProductUpdateDto>), Arguments = new object[] { "productUpdateDto" })]
        public async Task<IActionResult> UpdateProduct([FromForm] ProductUpdateDto productUpdateDto)
        {
            await _productService.UpdateProductAsync(productUpdateDto);
            return Ok();
        }

        /// <summary>
        /// Gets products by category.
        /// </summary>
        /// <param name="categoryId">The ID of the category to filter products.</param>
        /// <returns>The list of products in the specified category.</returns>
        /// <response code="200">Success</response>
        /// <response code="400">Bad request</response>
        /// <response code="500">Server error</response>
        [HttpGet("category/{categoryId}")]
        [Authorize]
        public IActionResult GetProductsByCategory(string categoryId)
        {
            var products = _productService.GetProductsByCategory(categoryId);
            return Ok(products);
        }
    }
}
