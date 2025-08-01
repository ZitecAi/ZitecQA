using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PortalIDSFTestes.elementos.login
{
    public class LoginElements
    {

        public string campoEmail { get;  } = "#email";
        public string campoSenha { get; } = "#password";
        public string loginBtn { get; } = "//button[text()='Entrar']";
        public string errorMessage { get; } = "#erroraccess";


    }
}
