using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ssd_authorization_solution.DTOs;
using ssd_authorization_solution.Entities;

namespace MyApp.Namespace;

[Route("api/[controller]")]
[ApiController]
public class ArticleController : ControllerBase
{
    private readonly AppDbContext db;

    public ArticleController(AppDbContext ctx)
    {
        this.db = ctx;
    }

    [HttpGet]
    [AllowAnonymous]
    public IEnumerable<ArticleDto> Get()
    {
        return db.Articles.Include(x => x.Author).Select(ArticleDto.FromEntity);
    }

    [HttpGet(":id")]
    [AllowAnonymous]
    public ActionResult<ArticleDto> GetById(int id)
    {
        var article = db
            .Articles.Include(x => x.Author)
            .Where(x => x.Id == id)
            .Select(ArticleDto.FromEntity)
            .SingleOrDefault();
        if (article == null)
        {
            return NotFound();
        }

        return article;

    }

    [HttpPost]
    [Authorize(Roles = "Writer")]
    public ActionResult<ArticleDto> Post([FromBody] ArticleFormDto dto)
    {
        var userName = HttpContext.User.Identity?.Name;
        var author = db.Users.Single(x => x.UserName == userName);
        var entity = new Article
        {
            Title = dto.Title,
            Content = dto.Content,
            Author = author,
            CreatedAt = DateTime.Now
        };
        var created = db.Articles.Add(entity).Entity;
        db.SaveChanges();
        return ArticleDto.FromEntity(created);
    }

    [HttpPut(":id")]
    [Authorize(Roles = "Writer,Editor")]
    public ActionResult<ArticleDto> Put(int id, [FromBody] ArticleFormDto dto)
    {
        var userName = HttpContext.User.Identity?.Name;
        var entity = db
            .Articles
            .Include(x => x.Author)
            .Single(x => x.Id == id);

        if (entity == null)
        {
            return NotFound();
        }

        // Check if user is Writer and the author of the article
        if (User.IsInRole("Writer") && entity.Author.UserName != userName)
        {
            return Forbid();
        }

        entity.Title = dto.Title;
        entity.Content = dto.Content;
        var updated = db.Articles.Update(entity).Entity;
        db.SaveChanges();
        return ArticleDto.FromEntity(updated);
    }

    [HttpDelete(":id")]
    [Authorize(Roles = "Editor")]
    public ActionResult Delete(int id)
    {
        var entity = db.Articles.Find(id);
        if (entity == null)
        {
            return NotFound();
        }

        db.Articles.Remove(entity);
        db.SaveChanges();
        return NoContent();
    }
}
