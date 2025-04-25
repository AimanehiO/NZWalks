using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using NZWalks.API.CustomActionFilters;
using NZWalks.API.Data;
using NZWalks.API.Models.Domain;
using NZWalks.API.Models.DTO;
using NZWalks.API.Repositories;


namespace NZWalks.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class WalksController : ControllerBase
    {
        private readonly IMapper mapper;
        private readonly IWalksRepository walkRepository;
        public WalksController(IMapper mapper, IWalksRepository walkRepository)
        {
            this.mapper = mapper;
            this.walkRepository = walkRepository;

        }
        //CREATE walk
        //POST: https://localhost:7226/api/walks
        [HttpPost]
        [ValidateModel]
        public async Task<IActionResult> Create([FromBody] AddWalksRequestDto addWalksRequestDto)
        {
                //map dto to domain
                var walkDomainModel = mapper.Map<Walk>(addWalksRequestDto);
                var createdWalk = await walkRepository.CreateAsync(walkDomainModel);
                //map domain to dto
                var createdWalkDto = mapper.Map<WalkDto>(createdWalk);
                return Ok(createdWalkDto);
        
        }

        //GET walks
        //GET:/api/walks?fitlerOn=Name&fiterQuery=Track&sortBy=Name&sortOrder=Name&isAscending=true&pageNumber=1&pageSize=10
        [HttpGet]
        public async Task<IActionResult> GetAll([FromQuery] string? filterOn, [FromQuery] string? filterQuery,
            [FromQuery] string? sortBy, [FromQuery] bool? isAscending,
            [FromQuery] int pagenumber =1, [FromQuery] int pageSize = 1000)
        {
           var walksDomainModel= await walkRepository.GetAllAsync(filterOn,filterQuery, sortBy, isAscending ?? true, 
               pagenumber, pageSize );//if isAscending is null, default to true

            //create exception
            throw new Exception("This is a new exception");

            //map domain model to dto
            return Ok(mapper.Map<List<WalkDto>>(walksDomainModel));
        }

        //GET walk by Id
        //GET:
        [HttpGet]
        [Route("{id:Guid}")]
        public async Task<IActionResult> GetById([FromRoute]Guid id)
        {
            var walksDomainModel = await walkRepository.GetByIdAsync(id);
            if (walksDomainModel == null)
                return NotFound();
            return Ok(mapper.Map<WalkDto>(walksDomainModel));
        }

        //UPDATE
        //PUT
        [HttpPut]
        [Route("{id:Guid}")]
        [ValidateModel]
        public async Task<IActionResult> UpdateAysnc([FromRoute] Guid id, UpdateWalkRequestDTO updateWalkRequestDTO)
        {
            
                var walkDomainModel = mapper.Map<Walk>(updateWalkRequestDTO);

                walkDomainModel = await walkRepository.UpdateAsync(id, walkDomainModel);

                if (walkDomainModel == null)
                    return NotFound();

                return Ok(mapper.Map<WalkDto>(walkDomainModel));
          
        }
        
        //delete
        //del
        [HttpDelete]
        [Route("{id:Guid}")]
        public async Task<IActionResult> DeleteAsync([FromRoute] Guid id)
        {
            var walkDomainModel = await walkRepository.DeleteAsync(id);
            if (walkDomainModel == null)
                return NotFound();

            return Ok (mapper.Map<WalkDto>(walkDomainModel));
        }
    }
}
