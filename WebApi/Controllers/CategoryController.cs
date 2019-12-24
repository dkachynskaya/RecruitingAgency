using BLL.Contracts;
using BLL.DTOs;
using Microsoft.AspNet.Identity;
using System.Collections.Generic;
using System.Net;
using System.Threading.Tasks;
using System.Web;
using System.Web.Http;
using WebApi.Models.Category;

namespace WebApi.Controllers
{
    [Authorize(Roles = "Admin")]
    [RoutePrefix("api/categories")]
    public class CategoryController : ApiController
    {
        private readonly IUnitOfWorkBLL uow;

        public CategoryController(IUnitOfWorkBLL uow)
        {
            this.uow = uow;
        }

        [HttpPost]
        [Route("create")]
        public async Task<IHttpActionResult> CreateCategory()
        {
            var httpRequest = HttpContext.Current.Request;
            string category = httpRequest.Params["Category"];

            var userId = this.User.Identity.GetUserId();
            if (userId == null)
                return this.Unauthorized();

            if (category.Length < 1)
                ModelState.AddModelError("Body", "Please add category.");

            if (!this.ModelState.IsValid)
                return this.BadRequest(this.ModelState);

            CategoryDTO categoryDTO = new CategoryDTO { Name = category };

            await uow.CategoryService.CreateCategory(categoryDTO);
            return Content(HttpStatusCode.Created, "Category added.");
        }

        [HttpPut]
        [Route("edit")]
        public async Task<IHttpActionResult> EditCategory([FromBody] CategoryEditViewModel newCategory)
        {
            var userId = User.Identity.GetUserId();
            if (userId == null)
                return this.Unauthorized();

            if (newCategory.Name.Length < 1)
                return this.BadRequest("Please add category.");

            var category = await uow.CategoryService.GetCategoryById(newCategory.Id);
            if (category == null)
                return NotFound();

            category.Name = newCategory.Name;

            await uow.CategoryService.EditCategory(category);
            return Ok("Category is edited");
        }

        [HttpGet]
        [AllowAnonymous]
        [Route("getAllCategories")]
        public async Task<IHttpActionResult> GetCategories()
        {
            if (!this.ModelState.IsValid)
                return this.BadRequest(this.ModelState);

            var userId = User.Identity.GetUserId();
            if (userId == null)
                return this.Unauthorized();

            List<CategoryViewModel> categories = AutoMapper.Mapper.Map<IEnumerable<CategoryDTO>, List<CategoryViewModel>>(
            await uow.CategoryService.GetAllCategories());

            if (categories != null)
                return Ok(categories);
            else return NotFound();
        }
    }
}
