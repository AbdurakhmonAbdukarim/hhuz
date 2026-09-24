// Controllers/DiscussionsController.cs
using System.Security.Claims;
using hhuz.Dto;
using hhuz.Service;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace hhuz.Controllers;

[Authorize]
[ApiController]
[Route("api/discussions")]
public class DiscussionsController : ControllerBase
{
    private readonly DiscussionService _discussionService;

    public DiscussionsController(DiscussionService discussionService)
    {
        _discussionService = discussionService;
    }

    private string GetUserId()
    {
        return User.FindFirst(ClaimTypes.NameIdentifier)?.Value
            ?? throw new UnauthorizedAccessException();
    }

    [HttpGet("position/{positionId}")]
    [AllowAnonymous]
    public async Task<IActionResult> GetByPosition(string positionId)
    {
        var posts = await _discussionService.GetByPositionAsync(positionId);
        var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

        var dtos = posts.Select(p => new DiscussionPostDto
        {
            Id = p.Id,
            Content = p.Content,
            UserName = p.User.Username,
            CreatedAt = p.CreatedAt,
            IsAuthor = p.UserId == userId
        }).ToList();

        return Ok(dtos);
    }

    [HttpPost("create")]
    public async Task<IActionResult> CreatePost([FromBody] CreatePostDto dto)
    {
        if (string.IsNullOrWhiteSpace(dto.Content))
            return BadRequest("Content is required");

        var userId = GetUserId();

        try
        {
            var post = await _discussionService.CreatePostAsync(
                dto.PositionId,
                userId,
                dto.Content
            );

            return Ok(new DiscussionPostDto
            {
                Id = post.Id,
                Content = post.Content,
                UserName = post.User.Username,
                CreatedAt = post.CreatedAt,
                IsAuthor = true
            });
        }
        catch (Exception ex)
        {
            return BadRequest(ex.Message);
        }
    }

    [HttpDelete("{postId}")]
    public async Task<IActionResult> DeletePost(string postId)
    {
        var userId = GetUserId();

        try
        {
            var success = await _discussionService.DeletePostAsync(postId, userId);
            if (!success)
                return NotFound();

            return Ok();
        }
        catch (UnauthorizedAccessException)
        {
            return Forbid();
        }
    }
}