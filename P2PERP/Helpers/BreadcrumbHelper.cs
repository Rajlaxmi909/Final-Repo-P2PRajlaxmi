using System;
using System.ComponentModel;
using System.Linq;
using System.Reflection;
using System.Web.Mvc;

namespace P2PERP.Helpers
{
    public static class BreadcrumbHelper
    {
        public static string GetActionDisplayName(this HtmlHelper html)
        {
            var routeData = html.ViewContext.RouteData.Values;

            var action = routeData["action"]?.ToString();
            var controller = routeData["controller"]?.ToString();

            if (string.IsNullOrEmpty(action) || string.IsNullOrEmpty(controller))
                return string.Empty;

            // Get current controller type
            var controllerType = Assembly.GetExecutingAssembly()
                .GetTypes()
                .FirstOrDefault(t =>
                    typeof(Controller).IsAssignableFrom(t) &&
                    t.Name.Equals(controller + "Controller", StringComparison.OrdinalIgnoreCase));

            if (controllerType == null)
                return action;

            // Get current action method
            var method = controllerType.GetMethods(BindingFlags.Instance | BindingFlags.Public)
                .FirstOrDefault(m =>
                    m.Name.Equals(action, StringComparison.OrdinalIgnoreCase));

            // Read DisplayName attribute
            var displayNameAttr = method?
                .GetCustomAttributes(typeof(DisplayNameAttribute), false)
                .FirstOrDefault() as DisplayNameAttribute;

            return displayNameAttr?.DisplayName ?? action;
        }
    }
}
