using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace findox.Domain.Models.Service
{
    public class ApiReponse
    {
        public ApiReponse()
        {
            Erros = new Dictionary<string, string[]>();
            ValidationErros = new Dictionary<string, string[]>();
        }

        public string? Status => ValidationErros.Any() || Erros.Any() ? "Error" : "Success"; 
        public object? Data { get; set; }
        public IDictionary<string, string[]> ValidationErros { get; set; }
        public IDictionary<string, string[]> Erros { get; set; }
        public bool hasValidationErros => ValidationErros.Any();
        public bool hasErros => Erros.Any();
    }
}