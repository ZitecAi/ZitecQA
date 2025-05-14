using EnviarMensagemSlack.Utils;
using Microsoft.Graph.Models;
using System;
using System.Collections.Generic;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using System;
using System.Net.Http;
using System.Threading.Tasks;


namespace EnviarMensagemSlack
{
    class Program
    {
        static async Task Main(string[] args)
        {
            await Utils.EnviarMensagemSlack.MandarMsgFundoQaAsync();
        }
    }
}