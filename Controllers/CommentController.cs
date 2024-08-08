using api.Interfaces;
using api.Mappers;
using api.Repository;
using Microsoft.AspNetCore.Mvc;

namespace api.Controllers;

[Route("api/comment")]
[ApiController]
public class CommentController(ICommentRepository commentRepository) : ControllerBase
{

    [HttpGet]
    public async Task<IActionResult> GetAll()
    {
        var comments = await commentRepository.GetAllAsync();
        var commentsDto = comments.Select(comment => comment.ToCommentDto());
        return Ok(commentsDto);
    }

    [HttpGet]
    [Route("{id}")]
    public async Task<IActionResult> GetById([FromRoute] int id)
    {
        var comment = await commentRepository.GetByIdAsync(id);
        if (comment == null)
        {
            return NotFound();
        }

        return Ok(comment.ToCommentDto());
    }
}