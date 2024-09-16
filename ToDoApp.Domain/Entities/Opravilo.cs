using System.ComponentModel.DataAnnotations;

namespace ToDoApp.Domain.Entities
{
    public class Opravilo
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "Naslov je obvezen.")]
        [MaxLength(100, ErrorMessage = "Naslov ne sme biti daljši od 100 znakov.")]
        public required string Naslov { get; set; }

        [Required(ErrorMessage = "Opis je obvezen.")]
        [MaxLength(500, ErrorMessage = "Opis ne sme biti daljši od 500 znakov.")]
        public required string Opis { get; set; }

        public DateTime DatumUstvarjanja { get; set; } = DateTime.Now;

        public bool Opravljeno { get; set; }
    }
}