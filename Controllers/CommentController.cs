using api.DTOs.Comment;
using api.Interfaces;
using api.Mappers;
using api.Repository;
using Microsoft.AspNetCore.Mvc;

namespace api.Controllers;

[Route("api/comment")]
[ApiController]
public class CommentController(ICommentRepository commentRepository, IStockRepository stockRepository) : ControllerBase
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

    [HttpPost("{stockId}")]
    public async Task<IActionResult> Create([FromRoute] int stockId, CreateCommentDTO commentDto)
    {
        if (!await stockRepository.StockExists(stockId))
        {
            return BadRequest("Stock does not exist");
        }

        var commentModel = commentDto.ToCommentFromCreate(stockId);
        await commentRepository.CreateASync(commentModel);

        return CreatedAtAction(nameof(GetById), new { id = commentModel.Id }, commentModel.ToCommentDto());
    }

    [HttpPut]
    [Route("{commentId}")]
    public async Task<IActionResult> Update([FromRoute] int commentId, [FromBody] UpdateCommentRequestDTO updateDto)
    {
        var comment = await commentRepository.UpdateAsync(commentId, updateDto.ToCommentFromUpdate());
        if (comment == null)
        {
            return NotFound("Comment Not Found");
        }

        return Ok(comment.ToCommentDto());
    }
    
}