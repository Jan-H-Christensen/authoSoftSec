using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace ssd_authorization_solution.Entities;

[Index("AuthorId", Name = "IX_Articles_AuthorId")]
public class Article
{
    [Key] public int Id { get; set; }

    public string Title { get; set; }
    public string Content { get; set; }
    public string AuthorId { get; set; } = null!;
    public IdentityUser Author { get; set; }
    public DateTime CreatedAt { get; set; }

    [InverseProperty("Article")] public virtual ICollection<Comment> Comments { get; set; } = new List<Comment>();
}