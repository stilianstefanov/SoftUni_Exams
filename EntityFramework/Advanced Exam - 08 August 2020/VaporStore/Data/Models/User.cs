namespace VaporStore.Data.Models
{
    using System.ComponentModel.DataAnnotations;

    public class User
    {
        public User()
        {
            Cards = new HashSet<Card>();
        }

        [Key]
        public int Id { get; set; }

        [MaxLength(ValidationConstants.UserUserNameMaxLength)]
        public string Username { get; set; } = null!;

        public string FullName { get; set; } = null!;

        public string Email { get; set; } = null!;

        public int Age { get; set; }

        public virtual ICollection<Card> Cards { get; set; } = null!;
    }
}
