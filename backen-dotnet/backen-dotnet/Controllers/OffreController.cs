using backen_dotnet.Data;
using backen_dotnet.Dtos.Offre;
using backen_dotnet.Interfaces;
using backen_dotnet.Mappers;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace backen_dotnet.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class OffreController : ControllerBase
    {
        private readonly ApplicationDbContext _context;
        private readonly IOffreRepository _offreRepository;


        public OffreController(ApplicationDbContext context,
                                IOffreRepository offreRepository)
        {
            _context = context;
            _offreRepository = offreRepository;
        }
        [HttpGet("GetAllOffre")]
        public async Task<IActionResult> GetAll() 
        {
            var offres = await _offreRepository.GetAllAsync();
            var offreDto = offres.Select(s => s.ToOfferDto());
            return Ok(offres);
        }

        [HttpGet("GetOffreStage")]
        public async Task<IActionResult> GetOffreStage()
        {
            var offresStage = await _offreRepository.GetOffreStageAsync();
            return Ok(offresStage.Select(s => s.ToOfferDto()));
        }

        [HttpGet("GetOffreEmplois")]
        public async Task<IActionResult> GetOffreEmplois()
        {
            var offresEmplois = await _offreRepository.GetOffreEmploisAsync();
            return Ok(offresEmplois.Select(s => s.ToOfferDto()));
        }
        [HttpGet("ByDepartement/{departement}")]
        public async Task<IActionResult> GetOffresByDepartement([FromRoute] string departement)
        {
            var offres = await _offreRepository.GetOffresByDepartementAsync(departement);
            if (offres == null || !offres.Any())
            {
                return NotFound("No offers found for the specified department.");
            }
            return Ok(offres);
        }
        [HttpGet("departements")]
        public async Task<IActionResult> GetDepartements()
        {
            var departements = await _offreRepository.GetDepartementsAsync();
            return Ok(departements);
        }
        [HttpGet("{id}")]
        public async Task<IActionResult> GetById([FromRoute] int id)
        {
            var offre = await _offreRepository.GetByIdAsync(id);
            if (offre == null)
            {
                return NotFound();
            }
            return Ok(offre.ToOfferDto());
        }
        [HttpPost("AddOffre")]
        public async Task<IActionResult> Create([FromBody] CreateOffreDto offreDto)
        {
            var offreModel = offreDto.ToOfferFromCreateDto();
            offreModel.PostedDate = DateTime.Now;
            await _offreRepository.CreateAsync(offreModel);
            return CreatedAtAction(nameof(GetById), new {id = offreModel.Id},offreModel.ToOfferDto());
        }
        [HttpPut("{id}")]
        public async Task<IActionResult> Update([FromRoute] int id, [FromBody] UpdateOffreDto updateDto)
        {
            var offreModel = await _offreRepository.UpdateAsync(id, updateDto);
            if (offreModel == null)
            {
                return NotFound();
            }
            return Ok(offreModel.ToOfferDto());
        }
        [HttpDelete("DeleteOffre/{id}")]
        public async Task<IActionResult> Delete([FromRoute] int id)
        {
            var offreModel = await _offreRepository.DeleteAsync(id);
            if(offreModel == null)
            {
                return NotFound();
            }
            return NoContent();
        }

    }
}
