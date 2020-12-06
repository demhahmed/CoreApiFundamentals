using System.ComponentModel.DataAnnotations;

namespace CoreCodeCamp.Data
{
  public class Talk
  {
    public int TalkId { get; set; }
    public Camp Camp { get; set; }
    [Required, StringLength(100)]
    public string Title { get; set; }
    [Required, StringLength(400, MinimumLength = 10)]
    public string Abstract { get; set; }
    [Range(100, 300)]
    public int Level { get; set; }
    [Required]
    public Speaker Speaker { get; set; }
  }
}