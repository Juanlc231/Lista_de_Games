using Lista_de_Games.Models;

namespace Lista_de_Games.Service
{
    public class AssignService
    {
        List<Users> users = new List<Users>();
        List<Games> games = new List<Games>();
        Users user = new Users();

        public List<Users> AssignUsers(GoogleSheetsData data) //vai receber os dados do google sheets e atribuir aos usuarios
        {
            if (data == null)
                return new List<Users>();

            for (int i = 0; i < data.Names.Count; i++)
            {
                users.Add(new Users()
                {
                    Name = data.Names[i],
                    NoteReceived = data.NoteReceivedGames[i],
                    NoteAssigned = data.NotesAssignedGames[i],
                    TotalGamesPlayed = data.TotalGamesPlayed[i]
                });
            }
            AssignUserGames(data);
            AssingnPositionUser(data);
            AssignUserNotesGames(data);

            return users;
        }

        private void AssignUserGames(GoogleSheetsData data) //atribui os 3 jogos escolhidos de cada jogador
        {
            for (int i = 0; i < data.Games.Count; i++)
            {
                int userIndex = i / 3;
                users[userIndex].Games.Add(data.Games[i]);
            }
        }

        private void AssingnPositionUser(GoogleSheetsData data) //essa funcao gera a posicao de cada usuario
        {
            int i = 0;
            for (char c = 'B'; c <= 'K'; c++)
            {
                if (i >= users.Count)
                    break;

                users[i].Position = c.ToString();
                i++;
            }
        }

        private void AssignUserNotesGames(GoogleSheetsData data) //atribui as notas notas dadas para os jogos
        {
            for (int i = 0; i < users.Count; i++)
            {
                for (int j = 0; j < data.Notes.Count; j++)
                {
                    users[i].Notes.Add(data.Notes[j][i]);
                }
            }
        }

        public List<Games> AssignGames(GoogleSheetsData Data)// essa funcao atribiu os dados aos jogos, como nome, nota final, posicao etc
        {
            if (Data == null)
                return new List<Games>();

            for (int i = 0; i < Data.Games.Count; i++)
            {
                games.Add(new Games()
                {
                    Name = Data.Games[i],
                    FinalNote = Data.FinalNotesGames[i],
                    Position = (i + 11).ToString()
                });
            }

            AssignGameIndicator(Data);
            return games;
        }

        private void AssignGameIndicator(GoogleSheetsData data) //essa funcao atribui o indicador de cada jogo, ou seja, qual usuario escolheu aquele jogo
        {
            for (int i = 0; i < data.Games.Count; i++)
            {
                int userIndex = i / 3;
                games[i].GameIndicator = users[userIndex].Name;
            }
        }

        public Users AssignCurrentUser(List<Users> users, string name)//essa funcao recebe a lista de usuarios e o nome do usuario logado, e retorna o usuario logado com seus dados
        {
            if (users == null || !users.Any())
                return new Users();

            var user = users.First(u => u.Name.Equals(name, StringComparison.OrdinalIgnoreCase)) ?? new Users(); //adicionar o claim para obter a posicao do usuario logado

            return user;
        }

        public DadosAssingsResult ResultBuilderData(GoogleSheetsData dados,string name)//essa funcao recebe os dados do google sheets e o nome do usuario logado, e retorna um objeto com a lista de usuarios, jogos e o usuario logado
        {
            var users = AssignUsers(dados);
            var games = AssignGames(dados);
            user = AssignCurrentUser(users, name);

            return new DadosAssingsResult()
            {
                Users = users,
                Games = games,
                CurrentUser = user
            };
        }
    }
}
