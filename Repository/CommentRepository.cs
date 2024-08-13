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

    public async Task<Comment?> GetByIdAsync(int id)
    {
        return await context.Comment.FindAsync(id);
    }

    public async Task<Comment> CreateASync(Comment comment)
    {
        await context.Comment.AddAsync(comment);
        await context.SaveChangesAsync();
        return comment;
    }

    public async Task<Comment?> UpdateAsync(int id, Comment comment)
    {
        var existingComment = await context.Comment.FindAsync(id);
        if (existingComment == null)
        {
            return null;
        }

        existingComment.Title = comment.Title;
        existingComment.Content = comment.Content;

        await context.SaveChangesAsync();
        return existingComment;
    }

    public async Task<Comment?> DeleteAsync(int id)
    {
        var existingComment = await context.Comment.FindAsync(id);
        if (existingComment == null)
        {
            return null;
        }

        context.Comment.Remove(existingComment);
        await context.SaveChangesAsync();

        return existingComment;
    }
}