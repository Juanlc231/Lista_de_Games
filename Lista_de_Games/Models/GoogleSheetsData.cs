namespace Lista_de_Games.Models
{
    public class GoogleSheetsData
    {
        public IList<string> Names { get; set; } = new List<string>();
        public IList<string> Games { get; set; } = new List<string>();
        public IList<string> FinalNotesGames { get; set; } = new List<string>();
        public IList<string> NoteReceivedGames { get; set; } = new List<string>();
        public IList<int> TotalGamesPlayed { get; set; } = new List<int>();
        public IList<string> NotesAssignedGames { get; set; } = new List<string>();
        public IList<IList<string>> Notes { get; set; } = new List<IList<string>>();

    }
}
