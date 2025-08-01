using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TesteOperacoesOperacoes;
using TesteOperacoesOperacoes.Model;
namespace TesteOperacoesOperacoes.Util
{
    public class Util
    {
        public static List<Usuario> GetUsuariosForTest()
        {
            var usuarios = new List<TesteOperacoesOperacoes.Model.Usuario>();

            usuarios.Add(new TesteOperacoesOperacoes.Model.Usuario("qazitec01@gmail.com", "Testeqa01?!", TesteOperacoesOperacoes.Model.Usuario.NivelEnum.Master));
            usuarios.Add(new TesteOperacoesOperacoes.Model.Usuario("jessica.tavares@aluno.ifsp.edu.br", "Jehtavares?123", TesteOperacoesOperacoes.Model.Usuario.NivelEnum.Consultoria));
            usuarios.Add(new TesteOperacoesOperacoes.Model.Usuario("caiooliweira@gmail.com", "id2021", TesteOperacoesOperacoes.Model.Usuario.NivelEnum.Gestora));
            usuarios.Add(new TesteOperacoesOperacoes.Model.Usuario("jessica.vitoria.tavares044@gmail.com", "Jehtavares?123", TesteOperacoesOperacoes.Model.Usuario.NivelEnum.Denver));
            return usuarios;
        }

    }
}
