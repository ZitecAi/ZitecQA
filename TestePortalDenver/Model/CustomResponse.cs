using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace TestePortalDenver.Model
{
    public class CustomResponse
    {
        public int Code { get; set; }
        public bool Success { get; set; }
        public object Model { get; set; }
        public List<string> Errors { get; set; }
        public override string ToString()
        {
            string lstErros = Errors != null ? String.Join(" ", Errors) : "";
            return $"Code: {Code}, Success: {Success} Model: {JsonConvert.SerializeObject(Model)} Errors: {lstErros}";
        }



    }
}
