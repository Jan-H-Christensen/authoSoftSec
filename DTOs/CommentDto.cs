using ssd_authorization_solution.Entities;

namespace ssd_authorization_solution.DTOs;

public class CommentDto
{
    public int Id { get; set; }
    public string Content { get; set; }
    public int ArticleId { get; set; }
    public AuthorDto Author { get; set; }

    public static CommentDto FromEntity(Comment entity)
    {
        return new CommentDto
        {
            Id = entity.Id,
            Content = entity.Content,
            ArticleId = entity.ArticleId,
            Author = AuthorDto.FromEntity(entity.Author),
        };
    }
}

