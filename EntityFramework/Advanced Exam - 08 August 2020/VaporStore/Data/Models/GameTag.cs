namespace VaporStore.Data.Models
{
    using System.ComponentModel.DataAnnotations.Schema;

    public class GameTag
    {
        [ForeignKey(nameof(Game))]
        public int GameId { get; set; }

        public Game Game { get; set; } = null!;

        [ForeignKey(nameof(Tag))]
        public int TagId { get; set; }

        public Tag Tag { get; set; } = null!;
    }
}
