using System.Security.Claims;

namespace Lista_de_Games.Service
{
    public class LoginService
    {
        private readonly GoogleSheetsService _googleSheetsService;
        private readonly AssignService _assignService = new AssignService();

        public LoginService(GoogleSheetsService googleSheetsService)
        {
            _googleSheetsService = googleSheetsService; 
        }

        public async Task<bool> ValidationUser(string userName) 
        { 
            if(userName == null)        
                return false;

            var users = await _googleSheetsService.GetNames();

           if(users.Any(n => n.Equals(userName, StringComparison.OrdinalIgnoreCase)))
               return true;
 
            return false;
        } //função para validar se tem o nome do usuário na lista de usuários

        public async Task<ClaimsPrincipal> AuthenticateUser(string userName) //função para salvar o dados básico do usuários que logan com cookie
        {
            var dados = await _googleSheetsService.LoadAllData();
            var users = _assignService.AssignUsers(dados);
            var currentUser = _assignService.AssignCurrentUser(users, userName);

            if (currentUser == null)
                return null;

            var datasUser = new List<Claim>
              {
                 new Claim(ClaimTypes.Name, userName),
                 new Claim("Position", currentUser.Position.ToString())
              };

            var identity = new ClaimsIdentity(datasUser, "CookieAuth");
            return new ClaimsPrincipal(identity);
        }
    }
}
