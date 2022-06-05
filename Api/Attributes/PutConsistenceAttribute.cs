using Api.Filters;
using Microsoft.AspNetCore.Mvc;

namespace Api.Attributes
{
    public class PutConsistenceAttribute : TypeFilterAttribute
    {
        public PutConsistenceAttribute() : base(typeof(PutConsistenceFilter))
        {
        }
    }
}