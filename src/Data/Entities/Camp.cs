
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace CoreCodeCamp.Data
{
  public class Camp
  {
    public int CampId { get; set; }
    [Required, StringLength(100)]
    public string Name { get; set; }
    [Required, StringLength(20)]
    public string Moniker { get; set; }
    public Location Location  { get; set; }
    public DateTime EventDate { get; set; } = DateTime.MinValue;
    public int Length { get; set; } = 1;
    public ICollection<Talk> Talks { get; set; }
  }
}