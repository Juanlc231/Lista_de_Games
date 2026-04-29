using Microsoft.AspNetCore.Mvc;
using Lista_de_Games.Service;
using Microsoft.AspNetCore.Authentication;

namespace Lista_de_Games.Controllers
{
    public class LoginController : Controller
    {
        private readonly GoogleSheetsService _googleSheetsService;
        private readonly LoginService _loginService;

        public LoginController(GoogleSheetsService _googleService)
        {
            _googleSheetsService = _googleService;
            _loginService = new LoginService(_googleSheetsService);
        }

        public IActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Login(string userName)
        {
            try
            {
                var validation = await _loginService.ValidationUser(userName);

                if (!validation)
                {
                    var mensagem = "Usuário invalido.";
                    throw new Exception(mensagem);
                }

                var datasUsers = await _loginService.AuthenticateUser(userName);
                await HttpContext.SignInAsync("CookieAuth", datasUsers);

                return RedirectToAction("Index", "Home");
            } 
            catch(Exception ex) 
            { 
                return View("Index", $"Ocorreu um erro ao tentar fazer login: {ex.Message}");
            } 
        }

        public async Task<IActionResult> Logout()
        {
            await HttpContext.SignOutAsync("CookieAuth");
            return RedirectToAction("Index");
        }
    }
}
