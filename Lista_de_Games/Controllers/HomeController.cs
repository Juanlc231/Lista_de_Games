using Lista_de_Games.Models;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;
using Lista_de_Games.Extensions;
using Lista_de_Games.Service;
using Lista_de_Games.ViewModel;
using Microsoft.AspNetCore.Authorization;

namespace Lista_de_Games.Controllers
{
    [Authorize]
    public class HomeController : Controller
    {
        private readonly GoogleSheetsService _googleSheetsService;
        private readonly AssignService _assignService = new AssignService();

        public HomeController(GoogleSheetsService googleSheetsService)
        {
            _googleSheetsService = googleSheetsService;
        }

        public async Task<IActionResult> Index()
        {
            var dados = await _googleSheetsService.LoadAllData();
            var result = _assignService.ResultBuilderData(dados, User.Identity.Name);
            var viewModel = new UsersViewModel(result.Games, result.Users, result.CurrentUser);

            return View(viewModel);
        }

        public async Task<IActionResult> Players()
        {
            var dados = await _googleSheetsService.LoadAllData();
            var Users = _assignService.AssignUsers(dados);

            return View(Users);
        }

        [HttpGet]
        public async Task<IActionResult> Edit()
        {
            var dados = await _googleSheetsService.LoadAllData();
            var Users = _assignService.AssignUsers(dados);

            return this.JsonModal("Edit", Users);
        }

        [HttpPost]
        public async Task<IActionResult> Edit(string positionGame, string note)
        {
            if (positionGame == null || note == null)
                return BadRequest("Dados necessarios inválidos");

            try
            {
                var positionPlayer = User.FindFirst("Position")?.Value ?? throw new Exception("Posição do usuário não encontrada.");

                await _googleSheetsService.Edit(positionPlayer, positionGame, note);
                return Ok();
            }
            catch (Exception ex)
            {
                return BadRequest(ex);
            }
        }

        [HttpGet]
        public async Task<IActionResult> InfoUser()
        {
            return this.JsonModal("ModalUser");
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
