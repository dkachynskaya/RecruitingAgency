using AutoMapper;
using BLL.Contracts;
using BLL.DTOs;
using DAL.Contracts;
using DAL.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BLL.Services
{
    public class CategoryService: ICategoryService
    {
        private readonly IUnitOfWork uow;
        public CategoryService(IUnitOfWork uow)
        {
            this.uow = uow;
        }

        public async Task<List<CategoryDTO>> GetAllCategories()
        {
            var categories = await uow.Category.GetAll();

            if (categories == null)
                throw new ArgumentException("Not found");
            return Mapper.Map<IEnumerable<Category>, IEnumerable<CategoryDTO>>(categories).ToList();

        }

        public async Task CreateCategory(CategoryDTO categoryDTO)
        {
            var category = AutoMapper.Mapper.Map<CategoryDTO, Category>(categoryDTO);
            if (categoryDTO.Name.Length > 0)
            {
                await uow.Category.Post(category);
            }
            else throw new ArgumentException("Wrong data");
        }

        public async Task EditCategory(CategoryDTO categoryDTO)
        {
            var category = AutoMapper.Mapper.Map<CategoryDTO, Category>(categoryDTO);
            if (categoryDTO.Name.Length > 0)
            {
                await uow.Category.Update(category);
            }
            else throw new ArgumentException("Wrong data");
        }

        public async Task<CategoryDTO> GetCategoryById(int categoryId)
        {
            return AutoMapper.Mapper.Map<Category, CategoryDTO>(await uow.Category.GetById(categoryId));
        }

        public void Dispose()
        {
            uow?.Dispose();
        }
    }
}
