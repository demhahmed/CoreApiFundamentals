using CoreCodeCamp.Data;

namespace CoreCodeCamp.Models
{
    public class TalkModel 
    {
        public int TalkId { get; set; }
        public string Title { get; set; }
        public string Abstract { get; set; }
        public int Level { get; set; }
        public Speaker Speaker { get; set; }
    }
}