using backen_dotnet.Data;
using backen_dotnet.Interfaces;
using backen_dotnet.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using Microsoft.EntityFrameworkCore;
namespace backen_dotnet.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CandidatureController : ControllerBase
    {
        private readonly UserManager<AppUser> _userManager;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly ICandidatureRepository _candidatureRepository;
        private readonly IOffreRepository _offreRepository;
        private readonly IMailService _mailService;

        public CandidatureController(UserManager<AppUser> userManager,
                                     IHttpContextAccessor httpContextAccessor,
                                     ICandidatureRepository candidatureRepository,
                                     IOffreRepository offreRepository,
                                     IMailService mailService)
        {

            _userManager = userManager;
            _httpContextAccessor = httpContextAccessor;
            _candidatureRepository = candidatureRepository;
            _offreRepository = offreRepository;
            _mailService = mailService;
        }
        [HttpPost]
        [Route("SendMail")]
        public async Task<IActionResult> SendMail(MailData mailData)
        {
            bool result = await _mailService.SendMailAsync(mailData);

            if (result)
            {
                return Ok(new { message = "Email sent successfully." });
            }
            else
            {
                return StatusCode(500, new { message = "Failed to send email." });
            }
        }

        [HttpPost("Postuler"), DisableRequestSizeLimit]
        [Authorize]
        public async Task<IActionResult> AddCandidature( [FromForm] string titre,  IFormFile Cv,  IFormFile LettreMotivation)
        {
            var appUser = await _userManager.GetUserAsync(_httpContextAccessor.HttpContext.User);
            var offre = await _offreRepository.GetByTitreAsync(titre);
            if (offre == null)
            {
                return BadRequest("Offre not Found");
            }
            var userCandidatures = await _candidatureRepository.GetUserOffre(appUser);
            if (userCandidatures.Any(e => e.Titre.ToLower()== titre.ToLower()))
            {
                return BadRequest("Candidature déja postulé");
            }
            if(Cv == null && Cv.Length == 0)
            {
                return BadRequest("Invalid Cv");
            }
            if(LettreMotivation == null && LettreMotivation.Length == 0)
            {
                return BadRequest("Invalid Lettre de motivation");
            }
            var folderName = Path.Combine("Ressources","CV");

            var folderLMName = Path.Combine("Ressources", "Letrre_de_motivation");

            var pathToSave = Path.Combine(Directory.GetCurrentDirectory(), folderName);
            var pathLMToSave = Path.Combine(Directory.GetCurrentDirectory(), folderLMName);

            if (!Directory.Exists(pathToSave))
            {
                Directory.CreateDirectory(pathToSave);
            }
            if (!Directory.Exists(pathLMToSave))
            {
                Directory.CreateDirectory(pathLMToSave);
            }
            //var fileName = Path.GetFileNameWithoutExtension(Path.GetRandomFileName()) + Path.GetExtension(Cv.);

            var fileName = Cv.FileName;
            var fileLMname = LettreMotivation.FileName;

            var fullLMpath = Path.Combine(pathLMToSave, fileLMname);
            var dbLMPath = Path.Combine(folderLMName, fileLMname);

            var fullPath = Path.Combine(pathToSave, fileName);
            var dbPath = Path.Combine(folderName, fileName);

       
            using (var stream = new FileStream(fullPath, FileMode.Create))
            {
                await Cv.CopyToAsync(stream);
            }
            using (var stream = new FileStream(fullLMpath, FileMode.Create))
            {
                await LettreMotivation.CopyToAsync(stream);
            }
            // Récupérer les compétences requises
            var requiredSkills = string.Join(", ", offre.RequiredSkills);
            double scoreAPI = 0.0;
            string status = "Rejected"; // Par défaut, statut "Rejeté"

            // Appel à l'API Python pour analyser le CV et les compétences requises
            using (var client = new HttpClient())
            {
                var form = new MultipartFormDataContent();
                form.Add(new StringContent(requiredSkills), "required_skills");
                var stream = new FileStream(fullPath, FileMode.Open, FileAccess.ReadWrite, FileShare.ReadWrite);

                form.Add(new StreamContent(stream), "cv", fileName);

                var response = await client.PostAsync("http://127.0.0.1:5000/compare", form);
                if (!response.IsSuccessStatusCode)
                {
                    return StatusCode(500, "Erreur lors de l'analyse du CV.");
                }
                var apiResult = await response.Content.ReadAsStringAsync();
                Console.WriteLine($"apiResult : '{apiResult}'");
                if (double.TryParse(apiResult, System.Globalization.NumberStyles.Any, System.Globalization.CultureInfo.InvariantCulture, out double scorePython))
                {
                    scoreAPI = scorePython;
                    Console.WriteLine("scorePython :", scorePython);

                    status = scoreAPI >= 5 ? "Accepted ATS" : "Rejected ATS";
                }
                else
                {
                    Console.WriteLine("Conversion de apiResult en double échouée.");
                }
                Console.WriteLine("scoreAPI :", scoreAPI);

                // Traitez le résultat renvoyé par l'API si nécessaire.

            }

            var candidatureModel = new Candidature
            {
                OffreId = offre.Id,
                AppUserId = appUser.Id,
                CvFileName = fileName,
                LettreMotivationName = fileLMname,
                AplliedDate = DateTime.Now,
                Score = scoreAPI,
                Status = status,
                Decision = "En attente",


            };
            await _candidatureRepository.CreateAsync(candidatureModel);
            if(candidatureModel == null)
            {
                return StatusCode(500,"Could not Create");
            }
            else
            {
                var mailData = new MailData()
                {
                    FromEmail = "chrouf.est@gmail.com",
                    ToEmails = appUser.Email,
                    Subject = "Thanks For Applying To"+ offre.Titre,
                    Body = "We have received your application for the " +
                    offre.Titre +
                    " position and will give it careful consideration." +
                    " We will get back to you as soon as possible." +
                    "\r\n\r\nFurthermore, in the event that we do not retain your application, " +
                    " we would like to keep your CV for 2 year(s) in our CV library " +
                    "so that we may contact you again if we have a position matching your profile." +
                    " If you do not wish this, you can inform us by email."  +
                    "All best,"
                };
                bool result = await _mailService.SendMailAsync(mailData);
                if (result)
                {
                    return Ok(new { message = "Email sent successfully." });
                }
                else
                {
                    return StatusCode(500, new { message = "Failed to send email." });
                }
            }
        }
        public class DecisionRequest
        {
            public string Decision { get; set; }
        }
        [HttpPut("update-decision/{appUserId}/{offreId}")]
        public async Task<IActionResult> UpdateDecision([FromRoute] string appUserId, [FromRoute] int offreId, [FromBody] DecisionRequest decisionRequest)
        {

            var success = await _candidatureRepository.UpdateDecisionAsync(appUserId,offreId, decisionRequest.Decision);
            if (!success)
            {
                return NotFound("Candidature non trouvée.");
            }

            return Ok("Decision mise à jour avec succès.");
        }
        [HttpGet("cv/{name}")]
        public IActionResult Get(string name)
        {
            var folderName = Path.Combine("Ressources", "CV");
            var pathToSave = Path.Combine(Directory.GetCurrentDirectory(), folderName);
            var fullPath = Path.Combine(pathToSave, name);

            var stream = new FileStream(fullPath, FileMode.Open, FileAccess.ReadWrite, FileShare.ReadWrite);
            return File(stream, "application/pdf");
        }
        [HttpGet("lm/{name}")]
        public IActionResult Getlm(string name)
        {
            var folderName = Path.Combine("Ressources", "Letrre_de_motivation");
            var pathToSave = Path.Combine(Directory.GetCurrentDirectory(), folderName);


            var fullPath = Path.Combine(pathToSave, name);

            var stream = new FileStream(fullPath, FileMode.Open, FileAccess.ReadWrite, FileShare.ReadWrite);
            return File(stream, "application/pdf");
        }

        [HttpGet("GetCandidaturesByUSer")]

        [Authorize]
        public async Task<IActionResult> GetUserCandidatures()
        {
            var appUser = await _userManager.GetUserAsync(_httpContextAccessor.HttpContext.User);

            var candidatures = await _candidatureRepository.GetCandidaturesByUser(appUser);
            var candidatureData = candidatures.Select(candidature => new
            {
                OffreTitre = candidature.Offre.Titre,
                LettreMotivationName = candidature.LettreMotivationName,
                CvFileName = candidature.CvFileName,
            });
            return Ok(candidatures);
        }
        [HttpGet("{id}")]
        [Authorize]
        public async Task<IActionResult> GetUserCandidaturesByOffer([FromRoute] int id)
        {
            var offer = await _offreRepository.GetByIdAsync(id);

            var candidatures = await _candidatureRepository.GetCandidaturesByOffer(offer);

            return Ok(candidatures);

           
        }
        [HttpDelete("Delete/{appUserId}/{offreId}")]
        [Authorize]
        public async Task<IActionResult> DeleteCandidature(string appUserId, int offreId)
        {
            var result = await _candidatureRepository.DeleteAsync(appUserId, offreId);

            if (result==null)
            {
                return NotFound(new { message = "Candidature not found." });
            }

            return Ok(new { message = "Candidature deleted successfully." });
        }
        [HttpGet("{appUserId}/{offreId}")]
        [Authorize]
        public async Task<IActionResult> GetCandidatureByUserName([FromRoute] string appUserId, [FromRoute] int offreId)
        {
            var candidature = await _candidatureRepository.GetCandidatureById(appUserId, offreId);
            JsonConvert.SerializeObject(candidature, Formatting.None,
                        new JsonSerializerSettings()
                        {
                            ReferenceLoopHandling = ReferenceLoopHandling.Ignore
                        });
          
            return Ok(candidature);
        }
    }
}
