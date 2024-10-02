
using FusionTech.src.Service.Store;
using Microsoft.AspNetCore.Mvc;
using static FusionTech.src.DTO.StoreDTO;

namespace FusionTech.src.Controllers
{
    [ApiController]
    [Route("/api/v1/[controller]")]
    public class StoreController : ControllerBase
    {
        protected readonly IStoreService _storeService;

        public StoreController(IStoreService storeService)
        {
            _storeService = storeService;
        }

        // Creates a new store
        [HttpPost]
        public async Task<ActionResult<StoreReadDto>> CreateOne([FromBody] StoreCreateDto createDto)
        {
            var storeCreated = await _storeService.CreateOneAsync(createDto);
            return Created($"/api/v1/store/{storeCreated.StoreId}", storeCreated);
        }

        // Retrieves all stores
        [HttpGet]
        public async Task<ActionResult<List<StoreReadDto>>> GetAllStores()
        {
            var storeList = await _storeService.GetAllAsync();
            return Ok(storeList);
        }

        // Retrieves a store by ID
        [HttpGet("{id}")]
        public async Task<ActionResult<StoreReadDto>> GetStoreById([FromRoute] Guid id)
        {
            var store = await _storeService.GetByIdAsync(id);
            if (store == null)
            {
                return NotFound();
            }
            return Ok(store);
        }

        // Updates a store by ID
        [HttpPut("{id}")]
        public async Task<ActionResult> UpdateStore([FromRoute] Guid id, [FromBody] StoreUpdateDto updateDto)
        {
            if (id != updateDto.StoreId)
            {
                return BadRequest("ID mismatch.");
            }

            var isUpdated = await _storeService.UpdateOneAsync(updateDto);
            if (!isUpdated)
            {
                return NotFound();
            }
            return NoContent();
        }

        // Deletes a store by ID
        [HttpDelete("{id}")]
        public async Task<ActionResult> DeleteStore([FromRoute] Guid id)
        {
            var isDeleted = await _storeService.DeleteOneAsync(id);
            if (!isDeleted)
            {
                return NotFound();
            }
            return NoContent();
        }
    }
}