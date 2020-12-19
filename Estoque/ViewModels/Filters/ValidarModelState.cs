using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using System.Linq;

namespace api_sis_evenda.Estoque.ViewModels.Filters
{
    public class ValidarModelState : ActionFilterAttribute
    {     
        public override void OnActionExecuting(ActionExecutingContext context)
        {
            if (!context.ModelState.IsValid)
            {
                var erros = new ValidaCamposViewModel(context.ModelState.SelectMany(sm => sm.Value.Errors).Select(s => s.ErrorMessage).ToList());
                context.Result = new BadRequestObjectResult(erros);
            }
        }
    }
}