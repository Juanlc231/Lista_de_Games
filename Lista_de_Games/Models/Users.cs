namespace Lista_de_Games.Models
{
    public class Users
    {
        public string Name { get; set; }
        public List<string> Games { get; set; } = new List<string>();
        public List<string> Notes { get; set; } = new List<string>();
        public int TotalGamesPlayed { get; set; }
        public string NoteAssigned { get; set; }
        public string NoteReceived { get; set; }
        public string Position { get; set; }
    }
}
