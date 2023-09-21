using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using TekhneCafe.Business.Abstract;
using TekhneCafe.Business.Consts;
using TekhneCafe.Business.Helpers.FilterServices;
using TekhneCafe.Business.Helpers.HeaderServices;
using TekhneCafe.Business.Models;
using TekhneCafe.Core.DTOs.Product;
using TekhneCafe.Core.Exceptions.Product;
using TekhneCafe.Core.Filters.Product;
using TekhneCafe.DataAccess.Abstract;
using TekhneCafe.DataAccess.Helpers.Transaction;
using TekhneCafe.Entity.Concrete;

namespace TekhneCafe.Business.Concrete
{
    public class ProductManager : IProductService
    {
        private readonly IProductDal _productDal;
        private readonly IProductAttributeDal _productAttributeDal;
        private readonly ITransactionManagement _transactionManagement;
        private readonly IImageService _imageService;
        private readonly IMapper _mapper;
        private readonly IHttpContextAccessor _httpContext;
        private readonly string _minioBaseUrl;
        public ProductManager(IProductDal productDal, IMapper mapper, IHttpContextAccessor httpContext, IProductAttributeDal productAttributeDal,
            ITransactionManagement transactionManagement, IImageService imageService, IConfiguration configuration)
        {
            _productDal = productDal;
            _mapper = mapper;
            _httpContext = httpContext;
            _productAttributeDal = productAttributeDal;
            _transactionManagement = transactionManagement;
            _imageService = imageService;
            _minioBaseUrl = configuration.GetValue<string>("Minio:Endpoint");
        }

        public async Task CreateProductAsync(ProductAddDto productAddDto)
        {
            Product product = _mapper.Map<Product>(productAddDto);
            string imagePath = await UploadProductImageAsync(MinioBuckets.ProductImage);
            product.ImagePath = imagePath;
            try
            {
                await _productDal.AddAsync(product);
            }
            catch
            {
                if (product.ImagePath != null)
                    await RemoveProductImageAsync(imagePath.Replace(MinioBuckets.ProductImage + "/", ""));
                throw new ProductInternalServerException();
            }
        }

        private async Task<string> UploadProductImageAsync(string bucketName)
        {
            UploadImageRequest request = new()
            {
                BucketName = bucketName,
                Image = _httpContext.HttpContext.Request.Form.Files.FirstOrDefault()
            };
            if (request.Image is null)
                return null;
            string imagePath = await _imageService.UploadImageAsync(request);
            return imagePath;
        }

        private async Task RemoveProductImageAsync(string path)
            => await _imageService.RemoveImageAsync(new RemoveImageRequest() { BucketName = MinioBuckets.ProductImage, ObjectName = path.Replace(MinioBuckets.ProductImage + "/", "") });

        public async Task DeleteProductAsync(string id)
        {
            Product product = await _productDal.GetProductIncludeAllAsync(id);
            ThrowErrorIfProductNotFound(product);
            product.IsDeleted = true;
            using (var transaction = await _transactionManagement.BeginTransactionAsync())
            {
                try
                {
                    foreach (var item in product.ProductAttributes)
                        await _productAttributeDal.HardDeleteAsync(item);
                    await _productDal.SafeDeleteAsync(product);
                    await transaction.CommitAsync();
                }
                catch
                {
                    throw new ProductInternalServerException();
                }
            }
        }

        public List<ProductListDto> GetAllProducts(ProductRequestFilter filter)
        {
            var filteredResult = FilterProducts(filter);
            return _mapper.Map<List<ProductListDto>>(filteredResult.ResponseValue);
        }

        public async Task<ProductDetailDto> GetProductByIdAsync(string id)
        {
            Product product = await _productDal.GetProductIncludeAllAsync(id);
            ThrowErrorIfProductNotFound(product);
            product.ImagePath = !string.IsNullOrEmpty(product.ImagePath) ? string.Concat(_minioBaseUrl, "/", product.ImagePath) : null;
            return _mapper.Map<ProductDetailDto>(product);
        }

        private ProductResponseFilter<List<Product>> FilterProducts(ProductRequestFilter filter)
        {
            var query = GetNotDeletedProducts();
            var filteredResult = new ProductFilterService().FilterProducts(query, filter);
            filteredResult.ResponseValue.ForEach(_ => _.ImagePath = !string.IsNullOrEmpty(_.ImagePath) ? string.Concat(_minioBaseUrl, "/", _.ImagePath) : null);
            new HeaderService(_httpContext).AddToHeaders(filteredResult.Headers);
            return filteredResult;
        }

        private IQueryable<Product> GetNotDeletedProducts()
         => _productDal.GetAll(_ => !_.IsDeleted)
            .Include(_ => _.ProductAttributes)
            .ThenInclude(_ => _.Attribute)
            .Include(_ => _.Category)
            .AsNoTracking()
            .AsSingleQuery();

        public List<ProductListDto> GetProductsByCategory(string categoryId)
        {
            var products = _productDal.GetProductsByCategory(categoryId);
            products.ForEach(_ => _.ImagePath = !string.IsNullOrEmpty(_.ImagePath) ? string.Concat(_minioBaseUrl, "/", _.ImagePath) : null);
            return _mapper.Map<List<ProductListDto>>(products);
        }

        private static void ThrowErrorIfProductNotFound(Product product)
        {
            if (product is null)
                throw new ProductNotFoundException();
        }

        public async Task UpdateProductAsync(ProductUpdateDto productUpdateDto)
        {
            Product product = await _productDal.GetProductIncludeAttributeAsync(productUpdateDto.Id.ToLower());
            ThrowErrorIfProductNotFound(product);
            product.ModifiedDate = DateTime.Now;
            product.Name = productUpdateDto.Name;
            product.Description = productUpdateDto.Description;
            product.Price = productUpdateDto.Price;
            product.CategoryId = Guid.Parse(productUpdateDto.CategoryId);
            string oldImagePath = product.ImagePath;
            string imagePath = await UploadProductImageAsync(MinioBuckets.ProductImage);
            if (imagePath != null)
                product.ImagePath = imagePath;
            using (var transaction = await _transactionManagement.BeginTransactionAsync())
            {
                try
                {
                    UpdateProductAttribute(productUpdateDto, product);
                    await _productDal.UpdateAsync(product);
                    await transaction.CommitAsync();
                }
                catch (Exception)
                {
                    if (imagePath != null)
                        await RemoveProductImageAsync(imagePath.Replace(MinioBuckets.ProductImage + "/", ""));
                    throw new ProductInternalServerException();
                }
            }
            if (imagePath != null && oldImagePath != null)
                await RemoveProductImageAsync(oldImagePath.Replace(MinioBuckets.ProductImage + "/", ""));
        }

        private void UpdateProductAttribute(ProductUpdateDto productUpdateDto, Product product)
        {
            var newProductAttributeIds = productUpdateDto.ProductAttributes?.Select(attr => attr.AttributeId.ToLower()).ToList();
            var existingAttributeIds = product.ProductAttributes?.Select(attr => attr.AttributeId.ToString()).ToList();
            RemoveAttributesFromProduct(newProductAttributeIds, product);
            UpdateAttributesOfProduct(existingAttributeIds, newProductAttributeIds, product, productUpdateDto);
            AddNewAttributesToProduct(existingAttributeIds, newProductAttributeIds, product, productUpdateDto);
        }

        private void RemoveAttributesFromProduct(List<string> newProductAttributeIds, Product product)
        {
            var attributesToRemove = product.ProductAttributes?.Where(attr => { return !(newProductAttributeIds != null && newProductAttributeIds.Contains(attr.AttributeId.ToString())); }).ToList();
            if (attributesToRemove != null)
                foreach (var attribute in attributesToRemove)
                    product.ProductAttributes?.Remove(attribute);
        }

        private void UpdateAttributesOfProduct(List<string> existingAttributeIds, List<string> newProductAttributeIds, Product product, ProductUpdateDto productUpdateDto)
        {
            IEnumerable<string> attributesToUpdate = null;
            if (existingAttributeIds != null)
                attributesToUpdate = newProductAttributeIds?.Where(attr => existingAttributeIds.Contains(attr));
            if (attributesToUpdate != null)
                foreach (var attributeId in attributesToUpdate)
                {
                    var updatedAttr = productUpdateDto.ProductAttributes?.First(_ => _.AttributeId.ToLower() == attributeId.ToLower());
                    var existingAttr = product.ProductAttributes?.First(_ => _.AttributeId == Guid.Parse(attributeId));
                    if (updatedAttr != null && existingAttr != null)
                        _mapper.Map(updatedAttr, existingAttr);
                }
        }

        private void AddNewAttributesToProduct(List<string> existingAttributeIds, List<string> newProductAttributeIds, Product product, ProductUpdateDto productUpdateDto)
        {
            if (existingAttributeIds is null)
                return;
            var attributesToAdd = newProductAttributeIds?.Except(existingAttributeIds).ToList();
            if (attributesToAdd != null)
                foreach (var attributeId in attributesToAdd)
                {
                    var newAttr = productUpdateDto.ProductAttributes?.First(_ => _.AttributeId.ToLower() == attributeId.ToLower());
                    product.ProductAttributes?.Add(new ProductAttribute { IsRequired = newAttr.IsRequired, Price = newAttr.Price, AttributeId = Guid.Parse(newAttr.AttributeId) });
                }
        }
    }
}
