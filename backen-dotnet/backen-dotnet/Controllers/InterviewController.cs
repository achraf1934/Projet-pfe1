using Azure.Core;
using backen_dotnet.Data;
using backen_dotnet.Dtos.Account;
using backen_dotnet.Dtos.Interview;
using backen_dotnet.Interfaces;
using backen_dotnet.Models;
using backen_dotnet.Repository;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace backen_dotnet.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class InterviewController : ControllerBase
    {
        private readonly IInterviewRepository _interviewRepository;
        private readonly ApplicationDbContext _context;

        public InterviewController(IInterviewRepository interviewRepository,
                                   ApplicationDbContext context)
        {
            _interviewRepository = interviewRepository;
            _context = context;
        }

        // Créer un entretien
        [HttpPost("schedule")]
        public async Task<IActionResult> ScheduleInterview([FromBody] InterviewRequest request)
        {
            var existingInterview = await _context.Interviews
       .FirstOrDefaultAsync(i => i.AppUserId == request.AppUserId && i.OffreId == request.OffreId);

            if (existingInterview != null)
            {
                // Retourne un message d'erreur si l'entretien existe déjà
                return BadRequest("An interview already exists for this user and offer.");
            }

            var isSlotAvailable = await _interviewRepository.ScheduleInterview(request.AppUserId, request.OffreId, request.AppointmentDateTime);

            if (!isSlotAvailable)
            {
                return BadRequest("Le créneau horaire est déjà occupé.");
            }

            return Ok("Entretien programmé avec succès.");
        }
        [HttpGet("get-interviews/{appUserId}/{offreId}")]
        public async Task<IActionResult> GetInterviewsByCandidat(string appUserId, int offreId)
        {
            var interviews = await _context.Interviews
            .FirstOrDefaultAsync(i => i.AppUserId == appUserId && i.OffreId == offreId);
            return Ok(interviews);
        }
        [HttpGet("get-interviews-byOffer/{offreId}")]
        public async Task<IActionResult> GetInterviewsByOffer([FromRoute] int offreId)
        {
            var interviews = await _context.Interviews
                .Include(i=> i.AppUser)
             .Include(i => i.Offre)
             .Where(i=> i.OffreId == offreId)
            .ToListAsync();
            Console.WriteLine(interviews);
            return Ok(interviews);
        }
        [HttpGet("get-interviews-byUser/{appUserId}")]
        public async Task<IActionResult> GetInterviewsByOffer([FromRoute] string appUserId)
        {
            var interviews = await _context.Interviews
                .Include(i => i.AppUser)
             .Include(i => i.Offre)
             .Where(i => i.AppUserId == appUserId)
            .ToListAsync();
            Console.WriteLine(interviews);
            return Ok(interviews);
        }
        [HttpPut("{id}")]
        public async Task<IActionResult> Update([FromRoute] int id, [FromBody] UpdateInterviewDto updateDto)
        {
            var InterviewModel = await _interviewRepository.UpdateAsync(id, updateDto);
            if (InterviewModel == null)
            {
                return NotFound();
            }
            return Ok(InterviewModel);
        }
    }
}
