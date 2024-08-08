using api.Data;
using api.Interfaces;
using api.Models;
using Microsoft.EntityFrameworkCore;

namespace api.Repository;

public class CommentRepository(ApplicationDbContext context) : ICommentRepository
{
    public async Task<List<Comment>> GetAllAsync()
    {
        return await context.Comment.ToListAsync();
    }
}