using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using NZWalks.API.Data;
using NZWalks.API.Models.Domain;
using NZWalks.API.Models.DTO;
using Microsoft.EntityFrameworkCore;
using NZWalks.API.Repositories;
using AutoMapper;
using NZWalks.API.CustomActionFilters;
using Microsoft.AspNetCore.Authorization;
using System.Text.Json;

namespace NZWalks.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class RegionsController : ControllerBase
    {
        private readonly NZWalksDbContext dbContext;
        private readonly IRegionRepository regionRepository;
        private readonly IMapper mapper;
        private readonly ILogger<RegionsController> logger;

        public RegionsController(NZWalksDbContext dbContext, IRegionRepository regionRepository, IMapper mapper, ILogger<RegionsController> logger )
        {
            this.dbContext = dbContext;
            this.regionRepository = regionRepository;
            this.mapper = mapper;
            this.logger = logger;
        }

        //GET ALL REGIONS
        //GET: https://localhost:7226/api/regions
        [HttpGet]
        //[Authorize(Roles ="Reader")]
        public async Task<IActionResult> GetAll()
        {
            logger.LogInformation("GetAllRegions Action Method Invoked");
            //get data from database-domain models
            var regionsDomain = await regionRepository.GetAllAsync();
            //map domin models to dto
            //return dto's
            //var regionsDto = new List<RegionDto>();
            //foreach (var regionDomain in regionsDomain)
            //{
            //    regionsDto.Add(new RegionDto()
            //    {
            //        Id = regionDomain.Id,
            //        Code = regionDomain.Code,
            //        Name = regionDomain.Name,
            //        RegionImageUrl = regionDomain.RegionImageUrl,
            //    });
            //}
            //another map domain to dto
            var regionsDto = mapper.Map<List<RegionDto>>(regionsDomain);
            logger.LogInformation($"Finished GetAllRegions request with data: {JsonSerializer.Serialize(regionsDto)}");
            //return dto
            return Ok(regionsDto);
        }

        //GET SINGLE REGION
        //GET: https://localhost:7226/api/regions/{id}
        [HttpGet]
        [Route("{id:Guid}")]
       //[Authorize(Roles = "Reader")]

        public async Task<IActionResult> GetById([FromRoute]Guid id)
        {
            
            //var region = dbContext.Regions.Find(id); only allows you pass pry key
            //get region domain model from database
            var regionDomain = await regionRepository.GetByIdAsync(id);//allows you pass others
            
            if (regionDomain == null)
                return NotFound();
            //map domain to dto
           
            return Ok(mapper.Map<RegionDto>(regionDomain));
        }

        //POST: creat new region
        //POST: https://localhost:7226/api/regions
        [HttpPost]
        [ValidateModel]
       // [Authorize(Roles = "Writer")]
        public async Task<IActionResult> CreateRegion([FromBody] AddRegionRequestDto addRegionRequestDto)
        {
            
                //map dto to domain
                var regionDomainModel = mapper.Map<Region>(addRegionRequestDto);
                //use model to create new region
                regionDomainModel = await regionRepository.CreateAsync(regionDomainModel);
                 //map domain back to dto
                var regionDto = mapper.Map<RegionDto>(regionDomainModel);
                return CreatedAtAction(nameof(GetById), new { id = regionDto.Id }, regionDto);
          
        }

        //PUT: update region
        //PUt:https://localhost:7226/api/regions/{id}
        [HttpPut]
        [Route("{id:Guid}")]
        [ValidateModel]
      //  [Authorize(Roles = "Writer")]
        public async Task<IActionResult> Update([FromRoute] Guid id, [FromBody] UpdateRegionsRequestDto updateRegionsRequestDto)
        {
            
                //map dto to domain
                var regionDomainModel = mapper.Map<Region>(updateRegionsRequestDto);

                //check if it exists
                regionDomainModel = await regionRepository.UpdateAsync(id, regionDomainModel);

                if (regionDomainModel == null)
                    return NotFound();

                //convert domain to dto
                var regionDto = mapper.Map<RegionDto>(regionDomainModel);
                return Ok(regionDto);
          
        }
        //DEL: delete region
        //DEL:https://localhost:7226/api/regions/{id}
        [HttpDelete]
        [Route("{id:guid}")]
       // [Authorize(Roles = "Writer, Reader")]
        public async Task<IActionResult> Delete([FromRoute] Guid id)
        {
            var regionDomainModel = await regionRepository.DeleteAsync(id);
            if (regionDomainModel == null) return NotFound();


            //return deleted region
            //mapping domain to dto
            var regionDto = mapper.Map<RegionDto>(regionDomainModel);
            return Ok(regionDto);


        }
    }
}
