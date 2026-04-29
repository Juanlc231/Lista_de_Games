using Microsoft.AspNetCore.Mvc;

namespace Lista_de_Games.Extensions
{
    public static class ControllerExtensions
    {
        public static IActionResult JsonModal(this Controller controller, string viewName)
        {
            return controller.PartialView(viewName);
        }

        public static IActionResult JsonModal(this Controller controller, string viewName, object model)
        {
            return controller.PartialView(viewName, model);
        }
    }
}