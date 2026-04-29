namespace Lista_de_Games.Models
{
    public class DadosAssingsResult
    {
        public List<Users> Users { get; set; } = new List<Users>();
        public List<Games> Games { get; set; } = new List<Games>();
        public Users CurrentUser { get; set; } = new Users();
    }
}
