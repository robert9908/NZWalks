using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi.Validations;
using NZWalks.API.CustomActionFilters;
using NZWalks.API.Data;
using NZWalks.API.Models.Domain;
using NZWalks.API.Models.DTO;
using NZWalks.API.Repositories;
using System.Text.Json;

namespace NZWalks.API.Controllers
{
    // https://localhost:1234/api/regions
    [Route("api/[controller]")]
    [ApiController]
    public class RegionsController : ControllerBase
    {
        private readonly NZWalksDbContext dbContext;
        private readonly IRegionRepository regionRepository;
        private readonly IMapper mapper;
        private readonly ILogger<RegionsController> logger;

        public RegionsController(NZWalksDbContext dbContext, IRegionRepository regionRepository,
            IMapper mapper,
            ILogger<RegionsController> logger)
        {
            this.dbContext = dbContext;
            this.regionRepository = regionRepository;
            this.mapper = mapper;
            this.logger = logger;
        }
        //GET ALL REGIONS
        //GET: https://localhost:portnumber/api/regions 
        [HttpGet]
        //[Authorize(Roles ="Reader")]
        public async Task<IActionResult> GetAll()
        {
            try
            {
                throw new Exception("This is a custom exception");
                //Get Data From Database
                var regionsDomain = await regionRepository.GetAllAsync();


                logger.LogInformation($"Finished GetAll Regions request with data {JsonSerializer.Serialize(regionsDomain)}");
                //Return DTOs
                return Ok(mapper.Map<List<RegionDto>>(regionsDomain));
            }
            catch (Exception ex)
            {
                logger.LogError(ex, ex.Message);
                throw;
            }
        }

        //GET SINGLE REGION
        // GET: https://localhost:portnumber/api/regions/{id}

        [HttpGet]
        [Route("{id}: Guid")]
        [Authorize(Roles = "Reader")]
        public async Task<IActionResult> GetById([FromRoute] Guid id)
        {
            //var region =  dbContext.Regions.Find(id);

            var regionDomain = await regionRepository.GetByIdAsync(id);
            if (regionDomain == null)
                return NotFound();
           


            return Ok(mapper.Map<RegionDto>(regionDomain));
        }

        //POST To Create a New Region
        //POST: https://localhost:portnumber/api/region

        [HttpPost]
        [ValidateModel]
        [Authorize(Roles = "Writer")]
        public async Task<IActionResult> Create([FromBody] AddRegionRequestDto addRegionRequestDto)
        {

            //Map or Convert DTO to Domain Model
            var regionDomainModel = mapper.Map<Region>(addRegionRequestDto);

            //Use Domain Model To Create Region

            regionDomainModel = await regionRepository.CreateAsync(regionDomainModel);


            //Map Domain Model Back to DTO

            var regionDto = mapper.Map<RegionDto>(regionDomainModel);
            return CreatedAtAction(nameof(GetById), new { id = regionDomainModel.Id }, regionDto);
        }

        [HttpPut]
        [Route("{id}: Guid")]
        [ValidateModel]
        [Authorize(Roles = "Writer")]
        public async Task<IActionResult> Update([FromRoute] Guid id, [FromBody] UpdateRegionRequestDto updateRegionRequestDto)
        {
            var regionDomainModel = mapper.Map<Region>(updateRegionRequestDto);

            //Check if region exists
            regionDomainModel = await regionRepository.UpdateAsync(id, regionDomainModel);

            if (regionDomainModel == null)
                return NotFound();

            //Map DTO to Domain Model

            return Ok(mapper.Map<RegionDto>(regionDomainModel));
        }

        //Delete region
        //DELET: https://localhost:portnumber/api/regions/{id}

        [HttpDelete]
        [Route("{id}: Guid")]
        [Authorize(Roles = "Writer")]
        public async Task<IActionResult> Delete([FromRoute] Guid id)
        {
            var regionDomainModel = await regionRepository.DeleteAsync(id);

            //Delete region

            //return deleted region back
            //map Domain Model to DTO 

            return Ok(mapper.Map<RegionDto>(regionDomainModel));   

        }
   }
}
