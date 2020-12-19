using System;
using System.Collections.Generic;

namespace api_sis_evenda.Estoque.ViewModels
{
    public class ValidaCamposViewModel
    {
        public IList<string> Erros { get; private set; }

        public ValidaCamposViewModel(IList<string> erros)
        {
            Erros = erros;
        }
    }
}
