using System.IO;
using System.Web.Mvc;

namespace FoucsBI.Dashboard.Extension
{
    public static class ControllerExtensions
    {
        public static string RenderPartialView(this Controller controller, string viewName, object model)
        {
            if (string.IsNullOrEmpty(viewName))
                return null;

            controller.ViewData.Model = model;

            using (var sw = new StringWriter())
            {
                ViewEngineResult viewResult = ViewEngines.Engines.FindPartialView(controller.ControllerContext, viewName);
                var viewContext = new ViewContext(controller.ControllerContext,viewResult.View, controller.ViewData, controller.TempData, sw);
                viewResult.View.Render(viewContext, sw);
                return sw.GetStringBuilder().ToString();
            }
        }
    }
}