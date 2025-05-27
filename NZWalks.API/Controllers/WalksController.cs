using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using NZWalks.API.Data;
using NZWalks.API.Models.Domain;
using NZWalks.API.Models.DTO;
using NZWalks.API.Repositories;

namespace NZWalks.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class WalksController : ControllerBase
    {
        private readonly IMapper mapper;
        private readonly IWalkRepository walkRepository;
        private readonly IRegionRepository regionRepository;
        private readonly NZWalksDbContext dbContext;

        public WalksController(
            IMapper mapper,
            IWalkRepository walkRepository,
            IRegionRepository regionRepository,
            NZWalksDbContext dbContext)
        {
            this.mapper = mapper;
            this.walkRepository = walkRepository;
            this.regionRepository = regionRepository;
            this.dbContext = dbContext;
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] AddWalkRequestDto addWalkRequestDto)
        {
            // Map DTO to Domain Model
            var walkDomainModel = mapper.Map<Walk>(addWalkRequestDto);

            await walkRepository.CreateAsync(walkDomainModel);

            // Map Domain Model back to DTO
            return Ok(mapper.Map<WalkDto>(walkDomainModel));
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var walksDomainModel = await walkRepository.GetWalksAsync();

            // Map Domain Model to DTO
            return Ok(mapper.Map<List<WalkDto>>(walksDomainModel));
        }

        [HttpGet]
        [Route("{id:Guid}")]
        public async Task<IActionResult> GetById([FromRoute] Guid id)
        {
            var walkDomainModel = await walkRepository.GetByIdAsync(id);

            if (walkDomainModel == null)
            {
                return NotFound();
            }

            // Map Domain Model to DTO
            return Ok(mapper.Map<WalkDto>(walkDomainModel));
        }

        [HttpPut]
        [Route("{id:Guid}")]
        public async Task<IActionResult> Update([FromRoute] Guid id, [FromBody] UpdateWalkRequestDto updateWalkRequestDto)
        {
            // Validate the incoming request
            if (!await ValidateUpdateWalkAsync(updateWalkRequestDto))
            {
                return BadRequest(ModelState);
            }

            // Convert DTO to Domain Model
            var walkDomainModel = mapper.Map<Walk>(updateWalkRequestDto);

            // Update Walk using repository
            walkDomainModel = await walkRepository.UpdateAsync(id, walkDomainModel);

            if (walkDomainModel == null)
            {
                return NotFound();
            }

            // Convert Domain Model to DTO
            return Ok(mapper.Map<WalkDto>(walkDomainModel));
        }

        private async Task<bool> ValidateUpdateWalkAsync(UpdateWalkRequestDto updateWalkRequestDto)
        {
            if (updateWalkRequestDto == null)
            {
                ModelState.AddModelError(nameof(updateWalkRequestDto),
                    $"{nameof(updateWalkRequestDto)} cannot be null");
                return false;
            }

            if (string.IsNullOrWhiteSpace(updateWalkRequestDto.Name))
            {
                ModelState.AddModelError(nameof(updateWalkRequestDto.Name),
                    $"{nameof(updateWalkRequestDto.Name)} is required");
            }

            if (string.IsNullOrWhiteSpace(updateWalkRequestDto.Description))
            {
                ModelState.AddModelError(nameof(updateWalkRequestDto.Description),
                    $"{nameof(updateWalkRequestDto.Description)} is required");
            }

            if (updateWalkRequestDto.RegionId == Guid.Empty)
            {
                ModelState.AddModelError(nameof(updateWalkRequestDto.RegionId),
                    $"{nameof(updateWalkRequestDto.RegionId)} is required");
            }

            if (updateWalkRequestDto.DifficultyId == Guid.Empty)
            {
                ModelState.AddModelError(nameof(updateWalkRequestDto.DifficultyId),
                    $"{nameof(updateWalkRequestDto.DifficultyId)} is required");
            }

            var region = await regionRepository.GetByIdAsync(updateWalkRequestDto.RegionId);
            if (region == null)
            {
                ModelState.AddModelError(nameof(updateWalkRequestDto.RegionId),
                    $"{nameof(updateWalkRequestDto.RegionId)} is invalid");
            }

            var difficulty = await dbContext.Difficulties.FirstOrDefaultAsync(x => 
                x.Id == updateWalkRequestDto.DifficultyId);
            if (difficulty == null)
            {
                ModelState.AddModelError(nameof(updateWalkRequestDto.DifficultyId),
                    $"{nameof(updateWalkRequestDto.DifficultyId)} is invalid");
            }

            if (ModelState.ErrorCount > 0)
            {
                return false;
            }

            return true;
        }
    }
}