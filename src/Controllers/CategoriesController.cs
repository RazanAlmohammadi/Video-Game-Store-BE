namespace FusionTech.src.Controllers
{
    [ApiController]
    [Route("/api/v1/[controller]")]
    public class CategoriesController : ControllerBase
    {
        protected readonly ICategoryService _categoryService;

        public CategoriesController(ICategoryService categoryService)
        {
            _categoryService = categoryService;
        }

        [Authorize(Policy = "admin")]
        [HttpPost]
        public async Task<ActionResult<CategoryBasicDto>> CreateOne(
            [FromBody] CategoryCreateDto createDto
        )
        {
            var categoryCreated = await _categoryService.CreateOneAsync(createDto);
            return Created($"api/v1/categories/{categoryCreated.CategoryId}", categoryCreated);
        }

       [HttpGet]
       public async Task<ActionResult<CategoryListDto>> GetAllAsync([FromQuery] PaginationOptions paginationOptions)
       {
            // Get paginated category list
            var (categories, totalCount) = await _categoryService.GetAllAsync(paginationOptions);

                // Map categories to detailed DTOs
            var categoriesDto = categories.Select(c => new CategoryDetailedDto
               {
                   CategoryId = c.CategoryId,
                   CategoryName = c.CategoryName,
                   VideoGameInfos = c.VideoGameInfos.Select(v => new VideoGameInfoReadDto
                   {
                       VideoGameInfoId = v.VideoGameInfoId,
                       GameName = v.GameName,
                       Description = v.Description,
                       YearOfRelease = v.YearOfRelease,
                       TotalRating = v.TotalRating,
                       GamePicturePath = v.GamePicturePath,
                       VideoGameVersions = v.VideoGameVersions.Select(ver => new VideoGameVersionReadDto
                       {
                           VideoGameVersionId = ver.VideoGameVersionId,
                           Price = ver.Price,
                           GameConsoleId = ver.GameConsoleId
                       }).ToList()
                   }).ToList()
               }).ToList();

               // Create response DTO
               var response = new CategoryListDto
               {
                   Categories = categoriesDto, 
                   TotalCategory = totalCount
               };

               return Ok(response);
           }

        [HttpGet("{CategoryName}")]
        public async Task<ActionResult<List<CategoryDetailedDto>>> GetCategoryDetailsByNameAsync([FromRoute] string CategoryName)
        {
            var category = await _categoryService.GetCategoryDetailsByNameAsync(CategoryName);
            return Ok(category);
        }

        [Authorize(Policy = "admin")]
        [HttpDelete("{Id}")]
        public async Task<ActionResult<CategoryBasicDto>> DeleteOne([FromRoute] Guid Id)
        {
            var category = await _categoryService.GetByIdAsync(Id);
            await _categoryService.DeleteOneAsync(category.CategoryId);
            return Ok(category);
        }

        [Authorize(Policy = "admin")]
        [HttpPut("{Id}")]
        public async Task<ActionResult> UpdateCategory(Guid id, CategoryUpdateDto updateDto)
        {
            await _categoryService.UpdateOneAsync(id, updateDto);
            return NoContent();
        }
    }
}
