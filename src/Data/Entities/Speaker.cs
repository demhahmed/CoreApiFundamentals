using System.ComponentModel.DataAnnotations;

namespace CoreCodeCamp.Data
{
  public class Speaker
  {
    public int SpeakerId { get; set; }
    [Required, StringLength(20)]
    public string FirstName { get; set; }
    [StringLength(20)]
    public string LastName { get; set; }
    [StringLength(20)]
    public string MiddleName { get; set; }
    [StringLength(20)]
    public string Company { get; set; }
    [StringLength(200)]
    public string CompanyUrl { get; set; }
    [StringLength(200)]
    public string BlogUrl { get; set; }
    [StringLength(200)]
    public string Twitter { get; set; }
    [StringLength(200)]
    public string GitHub { get; set; }
  }
}