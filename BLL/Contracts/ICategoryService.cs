using BLL.DTOs;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace BLL.Contracts
{
    public interface ICategoryService: IDisposable
    {
        Task<List<CategoryDTO>> GetAllCategories();
        Task CreateCategory(CategoryDTO categoryDTO);
        Task EditCategory(CategoryDTO categoryDTO);
        Task<CategoryDTO> GetCategoryById(int categoryId);
    }
}
