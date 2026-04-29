using Lista_de_Games.Models;
using Google.Apis.Auth.OAuth2;
using Google.Apis.Sheets.v4;
using Google.Apis.Sheets.v4.Data;

namespace Lista_de_Games.Service
{
    public class GoogleSheetsService
    {
        private readonly IConfiguration _configuration;
        private SheetsService _service;

        public GoogleSheetsService(IConfiguration configuration)
        {
            _configuration = configuration;
            CriarServico();
        }

        private void CriarServico() //esse servico é responsável por criar a conexão com a API do Google Sheets Ele lê o caminho para as credenciais, carrega o arquivo e cria um serviço do Google Sheets que é usado para fazer solicitações à API e realizar operacoes no Sheets
        {
            var credentials = _configuration["GoogleSheets:CredentialsJson"];
    
            GoogleCredential credential;
            using (var stream = new MemoryStream(System.Text.Encoding.UTF8.GetBytes(credentials)))
            {
                credential = GoogleCredential.FromStream(stream).CreateScoped(SheetsService.Scope.Spreadsheets);
                _service = new SheetsService(new Google.Apis.Services.BaseClientService.Initializer()
                {
                    HttpClientInitializer = credential,
                    ApplicationName = "Teste",
                });
            }
        }

        public async Task<IList<IList<string>>> GetNotes() //essa funcao é responsavel por pegar todas as notas dos jogos na tebela
        {
            var range = $"{_configuration["GoogleSheets:SheetName"]}!B11:K40";
            var request = _service.Spreadsheets.Values.Get(_configuration["GoogleSheets:SpreadsheetId"], range);
            var response = await request.ExecuteAsync();

            if (response.Values == null || response.Values.Count == 0)
                return new List<IList<string>>();

            var notes = new List<IList<string>>();

            for (var i = 0; i < response.Values.Count; i++)
            {
                var l = new List<string>();

                for (var j = 0; j < response.Values[i].Count; j++)
                {
                    l.Add(response.Values[i][j].ToString()!);
                }
                notes.Add(l);
            }

            return notes;
        }

        public async Task<IList<string>> GetNames() //essa funcao é utilizado para pegar os nomes dos individuos da planilha
        {
            var range = $"{_configuration["GoogleSheets:SheetName"]}!B1:K1";
            var request = _service.Spreadsheets.Values.Get(_configuration["GoogleSheets:SpreadsheetId"], range);
            var response = await request.ExecuteAsync();

            if (response.Values == null || response.Values.Count == 0)
                return new List<string>();

            var names = new List<string>();

            for (var i = 0; i < response.Values[0].Count; i++)
            {
                names.Add(response.Values[0][i]?.ToString()?.Substring(9).Trim() ?? $"Nome do {i}° esta nulo");
            }

            return names;
        }

        public async Task<IList<string>> GetGames() //essa funcao é utilizada para pegar os nomes dos jogos da planilha
        {
            var range = $"{_configuration["GoogleSheets:SheetName"]}!A11:A40";
            var request = _service.Spreadsheets.Values.Get(_configuration["GoogleSheets:SpreadsheetId"], range);
            var response = await request.ExecuteAsync();

            if (response.Values == null || response.Values.Count == 0)
                return new List<string>();

            var games = new List<string>();

            for (var i = 0; i < response.Values.Count; i++)
            {
                games.Add(response.Values[i][0].ToString() ?? $"O{i}° jogo não esta atribuido");
            }

            return games;
        }

        public async Task<IList<string>> GetFinalNotesGames() //essa funcao é utilizada para pegar as notas finais dos jogos da planilha
        {
            var range = $"{_configuration["GoogleSheets:SheetName"]}!L11:L40";
            var request = _service.Spreadsheets.Values.Get(_configuration["GoogleSheets:SpreadsheetId"], range);
            var response = await request.ExecuteAsync();

            if (response.Values == null || response.Values.Count == 0)
                return new List<string>();

            var notes = new List<string>();

            for (var i = 0; i < response.Values.Count; i++)
            {
                notes.Add(response.Values[i][0].ToString()!);
            }

            return notes;
        }

        public async Task<IList<string>> GetNoteReceivedGames() //essa funcao é utilizada para pegar as notas recebidas dos jogos indicados pelos jogadores
        {
            var range = $"{_configuration["GoogleSheets:SheetName"]}!B6:K6";
            var request = _service.Spreadsheets.Values.Get(_configuration["GoogleSheets:SpreadsheetId"], range);
            var response = await request.ExecuteAsync();

            if (response.Values == null || response.Values.Count == 0)
                return new List<string>();

            var notesReceived = new List<string>();

            for (var i = 0; i < response.Values[0].Count; i++)
            {
                notesReceived.Add(response.Values[0][i]?.ToString()!);
            }

            return notesReceived;
        }

        public async Task<IList<int>> GetTotalGamesPlayed() //essa funcao é utilizada para pegar o numero de jogos jogado por cada player
        {
            var range = $"{_configuration["GoogleSheets:SheetName"]}!B8:K8";
            var request = _service.Spreadsheets.Values.Get(_configuration["GoogleSheets:SpreadsheetId"], range);
            var response = await request.ExecuteAsync();

            if (response.Values == null || response.Values.Count == 0)
                return new List<int>();

            var total = new List<int>();

            for (var i = 0; i < response.Values[0].Count; i++)
            {
                if (int.TryParse(response.Values[0][i].ToString(), out int value))
                    total.Add(value);
            }

            return total;
        }

        public async Task<IList<string>> GetNotesAssignedGames() //essa funcao é utilizada para pegar as notas atribuida dos jogos jogados pelos players 
        {
            var range = $"{_configuration["GoogleSheets:SheetName"]}!B42:K42";
            var request = _service.Spreadsheets.Values.Get(_configuration["GoogleSheets:SpreadsheetId"], range);
            var response = await request.ExecuteAsync();

            if (response.Values == null || response.Values.Count == 0)
                return new List<string>();

            var notesAssigned = new List<string>();

            for (var i = 0; i < response.Values[0].Count; i++)
            {
                notesAssigned.Add(response.Values[0][i].ToString()!);
            }

            return notesAssigned;
        }

        public async Task<GoogleSheetsData> LoadAllData() //essa funcao é utilizada para carregar todos os dados da planilha
        {
            try
            {
                return new GoogleSheetsData
                {
                    Names = await GetNames(),
                    Games = await GetGames(),
                    FinalNotesGames = await GetFinalNotesGames(),
                    NoteReceivedGames = await GetNoteReceivedGames(),
                    TotalGamesPlayed = await GetTotalGamesPlayed(),
                    NotesAssignedGames = await GetNotesAssignedGames(),
                    Notes = await GetNotes()
                };
            }
            catch (Exception ex)
            {
                throw new("Erro ao carregar dados do Google Sheets", ex);
            }
        }

        public async Task Edit(string positionPlayer, string positionGame, string newNote) //essa funcao é utilizada para editar as notas dos jogos na planilha
        {
            try
            {
                var range = $"{_configuration["GoogleSheets:SheetName"]}!{positionPlayer + positionGame}";

                var newValue = new ValueRange
                {
                    Values = new List<IList<object>> { new List<object> { newNote } }
                };

                var editRequest = _service.Spreadsheets.Values.Update(newValue, _configuration["GoogleSheets:SpreadsheetId"], range);
                editRequest.ValueInputOption = SpreadsheetsResource.ValuesResource.UpdateRequest.ValueInputOptionEnum.USERENTERED;

                var updateResponse = await editRequest.ExecuteAsync();

                await Formatation(positionPlayer, positionGame, newNote);
            }
            catch (Exception ex)
            {
                throw new("Erro ao editar nota dos games", ex);
            }
        }

        private async Task Formatation(string column, string row, string note)//essa funcao é utilizada para formatar as notas dos jogos na planilha, utilizando cores para diferenciar as notas
        {
            try
            {
                int columnIndex = column[0] - 'A';
                int rowIndex = int.Parse(row) - 1;
                float floatNote = float.TryParse(note, out float result) ? result : 0;

                Color color = new Color { Red = 0f, Green = 0f, Blue = 0f };

                if (floatNote < 5)
                {
                    color = new Color { Red = 1f, Green = 0f, Blue = 0f };
                }
                else if (floatNote >= 5 && floatNote < 7)
                {
                    color = new Color { Red = 1f, Green = 0.6f, Blue = 0.2f };
                }
                else if (floatNote >= 7 && floatNote < 9)
                {
                    color = new Color { Red = 0f, Green = 0.69f, Blue = 0.94f };
                }
                else if (floatNote >= 9)
                {
                    color = new Color { Red = 0f, Green = 0.69f, Blue = 0.31f };
                }

                var requests = new List<Request> {
                    new Request { RepeatCell = new RepeatCellRequest { Range = new GridRange
                    {
                    SheetId = 0,
                    StartRowIndex = rowIndex,
                    EndRowIndex = rowIndex + 1,
                    StartColumnIndex = columnIndex,
                    EndColumnIndex = columnIndex + 1
                },
                Cell = new CellData
                {
                    UserEnteredFormat = new CellFormat
                    {
                        HorizontalAlignment = "CENTER",
                        TextFormat = new TextFormat { Bold = true, ForegroundColor = color}
                    }
                },
                 Fields = "userEnteredFormat(horizontalAlignment,textFormat)" }}};

                var batchUpdateRequest = new BatchUpdateSpreadsheetRequest { Requests = requests };

                await _service.Spreadsheets.BatchUpdate(batchUpdateRequest, _configuration["GoogleSheets:SpreadsheetId"]).ExecuteAsync();
            }
            catch (Exception ex)
            {
                throw new("Erro ao formatar a notas", ex);
            }
        }
    }
}
