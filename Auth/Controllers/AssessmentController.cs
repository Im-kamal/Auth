using Auth.Data;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;

namespace Auth.Controllers
{
    [Route("api/[controller]")]

    [ApiController]
    public class AssessmentController : ControllerBase
    {
        private readonly ApplicationDbContext _context;

        public AssessmentController(ApplicationDbContext context)
        {
            _context = context;
        }
        
        [HttpGet("GetQuestions")]
        public async Task<ActionResult<IEnumerable<Question>>> GetQuestions()
        {
            var questions = await _context.Questions
                .Include(q => q.Options) 
                .ToListAsync();

            return Ok(questions);
        }

        [HttpPost("SubmitAnswer")]
        public async Task<ActionResult<UserAnswer>> SubmitAnswer(UserAnswer userAnswer)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var questionExists = await _context.Questions.AnyAsync(q => q.Id == userAnswer.QuestionId);
            var optionExists = await _context.QuestionOptions.AnyAsync(o => o.Id == userAnswer.SelectedOptionId);

            if (!questionExists || !optionExists)
            {
                return NotFound("السؤال أو الخيار غير موجود.");
            }

            _context.UserAnswers.Add(userAnswer);
            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(GetQuestions), new { id = userAnswer.Id }, userAnswer);
        }
    }
}

