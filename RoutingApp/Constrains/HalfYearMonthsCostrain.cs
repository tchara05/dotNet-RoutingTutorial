using System.Text.RegularExpressions;

namespace RoutingApp.Constrains
{
    public class HalfYearMonthsCostrain : IRouteConstraint
    {

        public bool Match(HttpContext? httpContext, IRouter? route, string routeKey, RouteValueDictionary values, RouteDirection routeDirection)
        {
            if (!values.ContainsKey(routeKey)) //month
            {
                return false; // not match
            }

            string? value = Convert.ToString(values[routeKey]);

            Regex regex = new Regex("^(jan|feb|mar|apr|may)$");

            if (regex.IsMatch(value))
            {
                return true;
            }

            return false;
        }

    }
}
