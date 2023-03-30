namespace VaporStore.Data.Models
{
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using VaporStore.Data.Models.Enums;

    public class Purchase
    {
        [Key]
        public int Id { get; set; }

        public PurchaseType Type { get; set; }

        public string ProductKey { get; set; } = null!;

        public DateTime Date { get; set; }

        [ForeignKey(nameof(Card))]    
        public int CardId { get; set; }

        public Card Card { get; set; } = null!;

        [ForeignKey(nameof(Game))]
        public int GameId { get; set; }

        public Game Game { get; set; } = null!;
    }
}
