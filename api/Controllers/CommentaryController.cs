using api.Interfaces;
using api.Mappers;
using Microsoft.AspNetCore.Mvc;

namespace api.Controllers;

[Route("api/comment")]
[ApiController]
public class CommentaryController : ControllerBase
{
    private readonly ICommentRepository _commentRepository;

    public CommentaryController(ICommentRepository commentRepository)
    {
        _commentRepository = commentRepository;
    }

    [HttpGet]
    public async Task<IActionResult> GetAll()
    {
        var comments = await _commentRepository.GetAllAsync();
        var commentsDto = comments.Select(c => c.ToCommentDto());
        return Ok(commentsDto);
    }
}