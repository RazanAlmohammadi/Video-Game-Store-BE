namespace FusionTech.src.Controllers
{
    [ApiController]
    [Route("api/v1/[controller]")]
    public class SupplyController : ControllerBase
    {
        protected readonly ISupplyService _supplyService;

        public SupplyController(ISupplyService service)
        {
            _supplyService = service;
        }

        // Get all supplies
        [HttpGet]
        public async Task<ActionResult<SupplyReadDto>> GetSupplies(
            [FromQuery] PaginationOptions paginationOptions
        )
        {
            var paginatedSupplies = await _supplyService.GetAllAsync(paginationOptions);
            return Ok(paginatedSupplies);
        }

        // Get supply by ID
        [HttpGet("{id}")]
        public async Task<ActionResult> GetSupplyById(Guid id)
        {
            var supplyItem = await _supplyService.GetByIdAsync(id);
            if (supplyItem == null)
            {
                return NotFound();
            }
            return Ok(supplyItem);
        }

        // Create a new supply
        [HttpPost]
        [Authorize(Policy = "EmployeeOrAdmin")]
        public async Task<ActionResult<SupplyReadDto>> CreateOne(SupplyCreateDto createDto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var supplyCreated = await _supplyService.CreateOneAsync(createDto);
            return CreatedAtAction(
                nameof(GetSupplyById),
                new { id = supplyCreated.SupplyId },
                supplyCreated
            );
        }

        // Update supply
        [HttpPut("{id}")]
        [Authorize(Policy = "EmployeeOrAdmin")]
        public async Task<ActionResult> UpdateSupply(Guid id, SupplyUpdateDto updateDto) // Use SupplyUpdateDto
        {
            var isUpdated = await _supplyService.UpdateOnAsync(id, updateDto);
            if (!isUpdated)
            {
                return NotFound();
            }
            return NoContent();
        }

        // Delete supply
        [HttpDelete("{id}")]
        [Authorize(Policy = "EmployeeOrAdmin")]
        public async Task<ActionResult> DeleteSupply(Guid id)
        {
            var isDeleted = await _supplyService.DeleteOneAsync(id);
            if (!isDeleted)
            {
                return NotFound();
            }
            return NoContent();
        }
    }
}
