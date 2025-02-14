using Auth.Data;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;

namespace Auth.Controllers
{
    [Route("api/[controller]")]
    [Authorize]
    [ApiController]
    public class RecommendationController : ControllerBase
    {
        private readonly ApplicationDbContext _context;

        public RecommendationController(ApplicationDbContext context)
        {
            _context = context;
        }

        [HttpGet]
        public async Task<ActionResult<UserTrackRecommendation>> GetUserRecommendation()
        {
            var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if (userId == null)
                return Unauthorized();

            var recommendation = await _context.UserTrackRecommendations
                .Include(r => r.Track)
                .Where(r => r.UserId == userId)
                .OrderByDescending(r => r.CreatedAt)
                .FirstOrDefaultAsync();

            if (recommendation == null)
                return NotFound(new { message = "No recommendation found. Please complete the assessment first." });

            return Ok(new UserTrackRecommendation
            {
               
            });
        }
    }
}
