using TekhneCafe.Core.DTOs.Category;

namespace TekhneCafe.Business.Abstract
{
    public interface ICategoryService
    {
        Task CreateCategoryAsync(CategoryAddDto categoryAddDto);
        List<CategoryListDto> GetAllCategory();
        Task<CategoryListDto> GetCategoryByIdAsync(string id);
        Task DeleteCategoryAsync(string id);
        Task UpdateCategoryAsync(CategoryUpdateDto categoryUpdateDto);
    }
}
