using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using NZWalks.API.CustomActionFilters;
using NZWalks.API.Models.Domain;
using NZWalks.API.Models.DTO;
using NZWalks.API.Repositories;
using System.Net;

namespace NZWalks.API.Controllers
{
    // /api/walks
    [Route("api/[controller]")]
    [ApiController]
    public class WalksController : ControllerBase
    {
        private readonly IMapper mapper;
        private readonly IWalkRepository walkRepository;

        public WalksController(IMapper mapper, IWalkRepository walkRepository)
        {
            this.mapper = mapper;
            this.walkRepository = walkRepository;
        }
        //Create Walk
        //POST: /api/walks

        [HttpPost]
        [ValidateModel]
        public async Task<IActionResult> Create([FromBody] AddWalkRequestDto addWalkRequestDto)
        {
            //Map Dto to Domain Model
            var walkDomainModel = mapper.Map<Walk>(addWalkRequestDto);

            await walkRepository.CreateAsync(walkDomainModel);
            return Ok(mapper.Map<WalkDto>(walkDomainModel));
        }

        //GET Walks
        // GET: /api/walks?filterOn=Name&filterQuery=Track&sortBy=Name&Ascending=true&pageNumber=1&pageSize=10
        [HttpGet]
        public async Task<IActionResult> GetAll([FromQuery] string? filterOn, [FromQuery] string? filterQuery,
            [FromQuery] string? sortBy, [FromQuery] bool? isAscending,
            [FromQuery] int pageNumber = 1, [FromQuery] int pageSize = 1000)
        {
            var walksDomain = await walkRepository.GetAllAsync(filterOn, filterQuery, sortBy,
                isAscending ?? true, pageNumber, pageSize);

            //Create an exception
            throw new Exception("This is a new exception");

            return Ok(mapper.Map<List<WalkDto>>(walksDomain));
 
        }   


        //GET walks by id
        [HttpGet]
        [Route("id: Guid")]

        public async Task<IActionResult> GetById([FromRoute] Guid id)
        {
            var walkDomainModel = await walkRepository.GetByIdAsync(id);

            if (walkDomainModel == null) { return NotFound(); }

            return Ok(mapper.Map<WalkDto>(walkDomainModel));
        }

        //Update Walk by id
        //PUT: /api/Walks/{id}

        [HttpPut]
        [Route("id: Guid")]
        [ValidateModel]
        public async Task<IActionResult> Update([FromRoute] Guid id, UpdateWalkRequestDto updateWalkRequestDto)
        {
            //Map DTO to Domain Model 
            var walkDomainModel = mapper.Map<Walk>(updateWalkRequestDto);

            await walkRepository.UpdateAsync(id, walkDomainModel);

            if (walkDomainModel == null) { return NotFound(); }

            return Ok(mapper.Map<WalkDto>(walkDomainModel));
        }

        [HttpDelete]
        [Route("id: Guid")]
        public async Task<IActionResult> Delete([FromRoute] Guid id)
        {
            var deleteWalkDomainModel = await walkRepository.DeleteAsync(id);
            if (deleteWalkDomainModel == null)
            { return NotFound(); }

            return Ok(mapper.Map<WalkDto>(deleteWalkDomainModel));
        }
    }
}
