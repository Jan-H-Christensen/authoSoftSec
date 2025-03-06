using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace ssd_authorization_solution.Entities;

[Index("AuthorId", Name = "IX_Comments_AuthorId")]
[Index("ArticleId", Name = "IX_Comments_ArticleId")]
public class Comment
{
    [Key] public int Id { get; set; }

    public string Content { get; set; }
    public string AuthorId { get; set; } = null!;
    public IdentityUser Author { get; set; }

    public int ArticleId { get; set; }

    [ForeignKey("ArticleId")]
    [InverseProperty("Comments")]
    public virtual Article Article { get; set; } = null!;
}