using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using NZWalks.API.Data;
using NZWalks.API.Models.Domain;
using NZWalks.API.Models.DTO;
using Microsoft.EntityFrameworkCore;
using NZWalks.API.Repositories;

namespace NZWalks.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class RegionsController : ControllerBase
    {
        private readonly NZWalksDbContext dbContext;
        private readonly IRegionRepository regionRepository;

        public RegionsController(NZWalksDbContext dbContext, IRegionRepository regionRepository)
        {
            this.dbContext = dbContext;
            this.regionRepository = regionRepository;
        }

        //GET ALL REGIONS
        //GET: https://localhost:7226/api/regions
        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            //get data from database-domain models
            var regionsDomain = await regionRepository.GetAllAsync();
            //map domin models to dto
            //return dto's
            var regionsDto = new List<RegionDto>();
            foreach (var regionDomain in regionsDomain)
            {
                regionsDto.Add(new RegionDto()
                {
                    Id = regionDomain.Id,
                    Code = regionDomain.Code,
                    Name = regionDomain.Name,
                    RegionImageUrl = regionDomain.RegionImageUrl,
                });
            }


         //return dto
            return Ok(regionsDto);
        }

        //GET SINGLE REGION
        //GET: https://localhost:7226/api/regions/{id}
        [HttpGet]
        [Route("{id:Guid}")]
        
        public async Task<IActionResult> GetById([FromRoute]Guid id)
        {
            //var region = dbContext.Regions.Find(id); only allows you pass pry key
            //get region domain model from database
            var regionDomain = await regionRepository.GetByIdAsync(id);//allows you pass others
            
            if (regionDomain == null)
                return NotFound();
            //map domain to dto
            //map domin models to dto
            //return dto's
            
            var regionDto = new RegionDto
            {
                Id = regionDomain.Id,
                Code = regionDomain.Code,
                Name = regionDomain.Name,
                RegionImageUrl = regionDomain.RegionImageUrl,
            };
           //return dto
            return Ok(regionDto);
        }

        //POST: creat new region
        //POST: https://localhost:7226/api/regions
        [HttpPost]
        public async Task<IActionResult> CreateRegion([FromBody] AddRegionRequestDto addRegionRequestDto)
        {
            //map dto to domain
            var regionDomainModel = new Region
            {
                Code = addRegionRequestDto.Code,
                Name = addRegionRequestDto.Name,
                RegionImageUrl = addRegionRequestDto.RegionImageUrl

            };
           regionDomainModel= await regionRepository.CreateAsync(regionDomainModel);
            //map domain back to dto
            var regionDto = new RegionDto
            {

                Code = regionDomainModel.Code,
                Name = regionDomainModel.Name,
                RegionImageUrl = regionDomainModel.RegionImageUrl

            };
            return CreatedAtAction(nameof(GetById), new { id = regionDomainModel.Id}, regionDto);
        }

        //PUT: update region
        //PUt:https://localhost:7226/api/regions/{id}
        [HttpPut]
        [Route("{id:Guid}")]
        public async Task<IActionResult> Update([FromRoute]Guid id, [FromBody] UpdateRegionsRequestDto updateRegionsRequestDto)
        {
            //map dto to domain
            var regionDomainModel = new Region
            {
                Code = updateRegionsRequestDto.Code,
                Name = updateRegionsRequestDto.Name,
                RegionImageUrl = updateRegionsRequestDto.RegionImageUrl
            };

            //check if it exists
           regionDomainModel= await regionRepository.UpdateAsync(id, regionDomainModel);

            if (regionDomainModel == null)
                return NotFound();

            //convert domain to dto
            var regionDto = new RegionDto
            {
                Id = regionDomainModel.Id,
                Code = regionDomainModel.Code,
                Name = regionDomainModel.Name,
                RegionImageUrl = regionDomainModel.RegionImageUrl
            };
            return Ok(regionDto);
        }

        //DEL: delete region
        //DEL:https://localhost:7226/api/regions/{id}
        [HttpDelete]
        [Route("{id:guid}")]
        public async Task<IActionResult> Delete([FromRoute] Guid id)
        {
            var regionDomainModel = await regionRepository.DeleteAsync(id);
            if (regionDomainModel == null) return NotFound();


            //return deleted region
            //mapping domain to dto
            var regionDto = new RegionDto
            {
                Id = regionDomainModel.Id,
                Code = regionDomainModel.Code,
                Name = regionDomainModel.Name,
                RegionImageUrl = regionDomainModel.RegionImageUrl
            };
            return Ok(regionDto);


        }
    }
}
