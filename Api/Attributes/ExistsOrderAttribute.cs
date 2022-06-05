using Api.Filters;
using Microsoft.AspNetCore.Mvc;

namespace Api.Attributes
{
    public class ExistsOrderAttribute : TypeFilterAttribute
    {
        public ExistsOrderAttribute() : base(typeof(ExistsOrderFilter))
        {
        }
    }
}