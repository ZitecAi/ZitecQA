using Microsoft.Playwright;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;

namespace TestePortal.Pages.ControleInternoPage
{
    public class ControleInternoPoliticas
    {
        public static async Task<Model.Pagina> Politicas (IPage Page)
        {
            var pagina = new Model.Pagina();
            var listErros = new List<string>();
            int errosTotais = 0;

            try
            {
                var portalLink = TestePortalIDSF.Program.Config["Links:Portal"];
                var ControlePoliticas = await Page.GotoAsync(portalLink + "/Controles%20Internos/Politicas.aspx");

                if (ControlePoliticas.Status == 200)
                {
                    Console.Write("Políticas - Controle Interno : ");
                    pagina.Nome = "Políticas - Controle Interno";
                    pagina.StatusCode = ControlePoliticas.Status;
                    pagina.Listagem = "❓";
                    pagina.BaixarExcel = "❓";
                    pagina.InserirDados = "❓";
                    pagina.Excluir = "❓";
                    pagina.Reprovar = "❓";
                    pagina.Acentos = Utils.Acentos.ValidarAcentos(Page).Result;

                    if (pagina.Acentos == "❌")
                    {
                        errosTotais++;
                    }

                    //await Page.PauseAsync();
                    await Page.GetByRole(AriaRole.Cell, new() { Name = " Administrativo_ID" }).ClickAsync();
                    var page1 = await Page.RunAndWaitForPopupAsync(async () =>
                    {
                        var download = await Page.RunAndWaitForDownloadAsync(async () =>
                        {
                            await Page.GetByRole(AriaRole.Link, new() { Name = "Codigo de Etica.pdf" }).ClickAsync();
                        });
                        pagina.BaixarExcel = await Utils.Excel.ValidarDownloadAsync(download, "PortalIDSF.xlsx");
                        if (pagina.BaixarExcel == "❌")
                        {
                            errosTotais++;
                        }
                    });
                    await page1.CloseAsync();


                    await Page.GetByRole(AriaRole.Cell, new() { Name = " Auditoria_Interna" }).ClickAsync();
                    var page2 = await Page.RunAndWaitForPopupAsync(async () =>
                    {
                        var download1 = await Page.RunAndWaitForDownloadAsync(async () =>
                        {
                            await Page.GetByRole(AriaRole.Link, new() { Name = "01-18-01_1 - Regulamento da" }).ClickAsync();
                        });
                        pagina.BaixarExcel = await Utils.Excel.ValidarDownloadAsync(download1, "PortalIDSF.xlsx");
                        if (pagina.BaixarExcel == "❌")
                        {
                            errosTotais++;
                        }

                    });
                    await page2.CloseAsync();


                    await Page.GetByRole(AriaRole.Cell, new() { Name = " Compliance" }).ClickAsync();
                    var page3 = await Page.RunAndWaitForPopupAsync(async () =>
                    {
                        var download2 = await Page.RunAndWaitForDownloadAsync(async () =>
                        {
                            await Page.GetByRole(AriaRole.Link, new() { Name = "-02-01_1_v3 - Politica de PLDFT.pdf" }).ClickAsync();
                        });
                        pagina.BaixarExcel = await Utils.Excel.ValidarDownloadAsync(download2, "PortalIDSF.xlsx");
                        if (pagina.BaixarExcel == "❌")
                        {
                            errosTotais++;
                        }
                    });
                    await page3.CloseAsync();


                    var page4 = await Page.RunAndWaitForPopupAsync(async () =>
                    {
                        var download3 = await Page.RunAndWaitForDownloadAsync(async () =>
                        {
                            await Page.GetByRole(AriaRole.Link, new() { Name = "01-02-02_2_v3 - Manual da" }).ClickAsync();
                        });
                        pagina.BaixarExcel = await Utils.Excel.ValidarDownloadAsync(download3, "PortalIDSF.xlsx");
                        if (pagina.BaixarExcel == "❌")
                        {
                            errosTotais++;
                        }
                    });
                    await page4.CloseAsync();


                    var page5 = await Page.RunAndWaitForPopupAsync(async () =>
                    {
                        var download4 = await Page.RunAndWaitForDownloadAsync(async () =>
                        {
                            await Page.GetByRole(AriaRole.Link, new() { Name = "-02-03_2_v3 - Manual de KYC.pdf" }).ClickAsync();
                        });
                        pagina.BaixarExcel = await Utils.Excel.ValidarDownloadAsync(download4, "PortalIDSF.xlsx");
                        if (pagina.BaixarExcel == "❌")
                        {
                            errosTotais++;
                        }
                    });
                    await page5.CloseAsync();


                    var page6 = await Page.RunAndWaitForPopupAsync(async () =>
                    {
                        var download5 = await Page.RunAndWaitForDownloadAsync(async () =>
                        {
                            await Page.GetByRole(AriaRole.Link, new() { Name = "01-02-04_2_v3 - Manual de KYP" }).ClickAsync();
                        });
                        pagina.BaixarExcel = await Utils.Excel.ValidarDownloadAsync(download5, "PortalIDSF.xlsx");
                        if (pagina.BaixarExcel == "❌")
                        {
                            errosTotais++;
                        }
                    });
                    await page6.CloseAsync();


                    var page7 = await Page.RunAndWaitForPopupAsync(async () =>
                    {
                        var download6 = await Page.RunAndWaitForDownloadAsync(async () =>
                        {
                            await Page.GetByRole(AriaRole.Link, new() { Name = "01-02-05_2_v3 - Manual de" }).ClickAsync();
                        });
                        pagina.BaixarExcel = await Utils.Excel.ValidarDownloadAsync(download6, "PortalIDSF.xlsx");
                        if (pagina.BaixarExcel == "❌")
                        {
                            errosTotais++;
                        }
                    });
                    await page7.CloseAsync();


                    var page8 = await Page.RunAndWaitForPopupAsync(async () =>
                    {
                        var download7 = await Page.RunAndWaitForDownloadAsync(async () =>
                        {
                            await Page.GetByRole(AriaRole.Link, new() { Name = "01-02-06_1 - Politica de" }).ClickAsync();
                        });
                        pagina.BaixarExcel = await Utils.Excel.ValidarDownloadAsync(download7, "PortalIDSF.xlsx");
                        if (pagina.BaixarExcel == "❌")
                        {
                            errosTotais++;
                        }
                    });
                    await page8.CloseAsync();


                    var page9 = await Page.RunAndWaitForPopupAsync(async () =>
                    {
                        var download8 = await Page.RunAndWaitForDownloadAsync(async () =>
                        {
                            await Page.GetByRole(AriaRole.Link, new() { Name = "01-02-07_1 - Politica de" }).ClickAsync();
                        });
                        pagina.BaixarExcel = await Utils.Excel.ValidarDownloadAsync(download8, "PortalIDSF.xlsx");
                        if (pagina.BaixarExcel == "❌")
                        {
                            errosTotais++;
                        }
                    });
                    await page9.CloseAsync();


                    var page10 = await Page.RunAndWaitForPopupAsync(async () =>
                    {
                        var download9 = await Page.RunAndWaitForDownloadAsync(async () =>
                        {
                            await Page.GetByRole(AriaRole.Link, new() { Name = "01-02-08_1 - Política de" }).ClickAsync();
                        });
                        pagina.BaixarExcel = await Utils.Excel.ValidarDownloadAsync(download9, "PortalIDSF.xlsx");
                        if (pagina.BaixarExcel == "❌")
                        {
                            errosTotais++;
                        }
                    });
                    await page10.CloseAsync();


                    var page11 = await Page.RunAndWaitForPopupAsync(async () =>
                    {
                        var download10 = await Page.RunAndWaitForDownloadAsync(async () =>
                        {
                            await Page.GetByRole(AriaRole.Link, new() { Name = "01-02-08_1 - Política de" }).ClickAsync();
                        });
                        pagina.BaixarExcel = await Utils.Excel.ValidarDownloadAsync(download10, "PortalIDSF.xlsx");
                        if (pagina.BaixarExcel == "❌")
                        {
                            errosTotais++;
                        }
                    });
                    await page11.CloseAsync();


                    await Page.GetByRole(AriaRole.Cell, new() { Name = " Contabilidade" }).ClickAsync();
                    var page12 = await Page.RunAndWaitForPopupAsync(async () =>
                    {
                        var download11 = await Page.RunAndWaitForDownloadAsync(async () =>
                        {
                            await Page.GetByRole(AriaRole.Link, new() { Name = "01-12-01_1 - Politica de" }).ClickAsync();
                        });
                        pagina.BaixarExcel = await Utils.Excel.ValidarDownloadAsync(download11, "PortalIDSF.xlsx");
                        if (pagina.BaixarExcel == "❌")
                        {
                            errosTotais++;
                        }
                    });
                    await page12.CloseAsync();


                    await Page.GetByRole(AriaRole.Cell, new() { Name = " Custodia & Controladoria" }).ClickAsync();
                    var page13 = await Page.RunAndWaitForPopupAsync(async () =>
                    {
                        var download12 = await Page.RunAndWaitForDownloadAsync(async () =>
                        {
                            await Page.GetByRole(AriaRole.Link, new() { Name = "01-03-01_1 - Politica de" }).ClickAsync();
                        });
                        pagina.BaixarExcel = await Utils.Excel.ValidarDownloadAsync(download12, "PortalIDSF.xlsx");
                        if (pagina.BaixarExcel == "❌")
                        {
                            errosTotais++;
                        }
                    });
                    await page13.CloseAsync();


                    var page14 = await Page.RunAndWaitForPopupAsync(async () =>
                    {
                        var download13 = await Page.RunAndWaitForDownloadAsync(async () =>
                        {
                            await Page.GetByRole(AriaRole.Link, new() { Name = "01-03-02_1 - Politica de" }).ClickAsync();
                        });
                        pagina.BaixarExcel = await Utils.Excel.ValidarDownloadAsync(download13, "PortalIDSF.xlsx");
                        if (pagina.BaixarExcel == "❌")
                        {
                            errosTotais++;
                        }
                    });
                    await page14.CloseAsync();


                    await Page.GetByRole(AriaRole.Cell, new() { Name = " Distribuicao" }).ClickAsync();
                    var page15 = await Page.RunAndWaitForPopupAsync(async () =>
                    {
                        var download14 = await Page.RunAndWaitForDownloadAsync(async () =>
                        {
                            await Page.GetByRole(AriaRole.Link, new() { Name = "01-08-01_1 - Politica de" }).ClickAsync();
                        });
                        pagina.BaixarExcel = await Utils.Excel.ValidarDownloadAsync(download14, "PortalIDSF.xlsx");
                        if (pagina.BaixarExcel == "❌")
                        {
                            errosTotais++;
                        }
                    });
                    await page15.CloseAsync();


                    await Page.GetByRole(AriaRole.Cell, new() { Name = " Escrituracao & Cadastro" }).ClickAsync();
                    var page16 = await Page.RunAndWaitForPopupAsync(async () =>
                    {
                        var download15 = await Page.RunAndWaitForDownloadAsync(async () =>
                        {
                            await Page.GetByRole(AriaRole.Link, new() { Name = "-03-01_1 - Politica de Cadastro.pdf" }).ClickAsync();
                        });
                        pagina.BaixarExcel = await Utils.Excel.ValidarDownloadAsync(download15, "PortalIDSF.xlsx");
                        if (pagina.BaixarExcel == "❌")
                        {
                            errosTotais++;
                        }
                    });
                    await page16.CloseAsync();


                    await Page.GetByRole(AriaRole.Cell, new() { Name = " Governanca" }).ClickAsync();
                    var page17 = await Page.RunAndWaitForPopupAsync(async () =>
                    {
                        var download16 = await Page.RunAndWaitForDownloadAsync(async () =>
                        {
                            await Page.GetByRole(AriaRole.Link, new() { Name = "01-17-01_1 - Politica de" }).ClickAsync();
                        });
                        pagina.BaixarExcel = await Utils.Excel.ValidarDownloadAsync(download16, "PortalIDSF.xlsx");
                        if (pagina.BaixarExcel == "❌")
                        {
                            errosTotais++;
                        }
                    });
                    await page17.CloseAsync();


                    await Page.GetByRole(AriaRole.Cell, new() { Name = " Investidor_Nao_Residente" }).ClickAsync();
                    var page18 = await Page.RunAndWaitForPopupAsync(async () =>
                    {
                        var download17 = await Page.RunAndWaitForDownloadAsync(async () =>
                        {
                            await Page.GetByRole(AriaRole.Link, new() { Name = "Política de Cadastro" }).ClickAsync();
                        });
                        pagina.BaixarExcel = await Utils.Excel.ValidarDownloadAsync(download17, "PortalIDSF.xlsx");
                        if (pagina.BaixarExcel == "❌")
                        {
                            errosTotais++;
                        }
                    });
                    await page18.CloseAsync();


                    await Page.GetByRole(AriaRole.Cell, new() { Name = " Mercado_Capitais" }).ClickAsync();
                    var page19 = await Page.RunAndWaitForPopupAsync(async () =>
                    {
                        var download18 = await Page.RunAndWaitForDownloadAsync(async () =>
                        {
                            await Page.GetByRole(AriaRole.Link, new() { Name = "01-09-01_1 - Politica de" }).ClickAsync();
                        });
                        pagina.BaixarExcel = await Utils.Excel.ValidarDownloadAsync(download18, "PortalIDSF.xlsx");
                        if (pagina.BaixarExcel == "❌")
                        {
                            errosTotais++;
                        }
                    });
                    await page19.CloseAsync();


                    var page20 = await Page.RunAndWaitForPopupAsync(async () =>
                    {
                        var download19 = await Page.RunAndWaitForDownloadAsync(async () =>
                        {
                            await Page.GetByRole(AriaRole.Link, new() { Name = "01-09-02_1 - Politica de" }).ClickAsync();
                        });
                        pagina.BaixarExcel = await Utils.Excel.ValidarDownloadAsync(download19, "PortalIDSF.xlsx");
                        if (pagina.BaixarExcel == "❌")
                        {
                            errosTotais++;
                        }
                    });
                    await page20.CloseAsync();


                    var page21 = await Page.RunAndWaitForPopupAsync(async () =>
                    {
                        var download20 = await Page.RunAndWaitForDownloadAsync(async () =>
                        {
                            await Page.GetByRole(AriaRole.Link, new() { Name = "REGRAS E PARÂMETROS DE ATUAÇÃ" }).ClickAsync();
                        });
                        pagina.BaixarExcel = await Utils.Excel.ValidarDownloadAsync(download20, "PortalIDSF.xlsx");
                        if (pagina.BaixarExcel == "❌")
                        {
                            errosTotais++;
                        }
                    });
                    await page21.CloseAsync();


                    await Page.GetByRole(AriaRole.Cell, new() { Name = " Ouvidoria" }).ClickAsync();
                    var page22 = await Page.RunAndWaitForPopupAsync(async () =>
                    {
                        var download21 = await Page.RunAndWaitForDownloadAsync(async () =>
                        {
                            await Page.GetByRole(AriaRole.Link, new() { Name = "01-11-01_1 - Politica de" }).ClickAsync();
                        });
                        pagina.BaixarExcel = await Utils.Excel.ValidarDownloadAsync(download21, "PortalIDSF.xlsx");
                        if (pagina.BaixarExcel == "❌")
                        {
                            errosTotais++;
                        }
                    });
                    await page22.CloseAsync();


                    await Page.GetByRole(AriaRole.Cell, new() { Name = " Riscos" }).ClickAsync();
                    var page23 = await Page.RunAndWaitForPopupAsync(async () =>
                    {
                        var download22 = await Page.RunAndWaitForDownloadAsync(async () =>
                        {
                            await Page.GetByRole(AriaRole.Link, new() { Name = "01-06-01_1 - Politica" }).ClickAsync();
                        });
                        pagina.BaixarExcel = await Utils.Excel.ValidarDownloadAsync(download22, "PortalIDSF.xlsx");
                        if (pagina.BaixarExcel == "❌")
                        {
                            errosTotais++;
                        }
                    });
                    await page23.CloseAsync();


                    var page24 = await Page.RunAndWaitForPopupAsync(async () =>
                    {
                        var download23 = await Page.RunAndWaitForDownloadAsync(async () =>
                        {
                            await Page.GetByRole(AriaRole.Link, new() { Name = "01-06-02_1 - Politica de" }).ClickAsync();
                        });
                        pagina.BaixarExcel = await Utils.Excel.ValidarDownloadAsync(download23, "PortalIDSF.xlsx");
                        if (pagina.BaixarExcel == "❌")
                        {
                            errosTotais++;
                        }
                    });
                    await page24.CloseAsync();


                    var page25 = await Page.RunAndWaitForPopupAsync(async () =>
                    {
                        var download24 = await Page.RunAndWaitForDownloadAsync(async () =>
                        {
                            await Page.GetByRole(AriaRole.Link, new() { Name = "01-06-03_1 - Politica de Gest" }).ClickAsync();
                        });
                        pagina.BaixarExcel = await Utils.Excel.ValidarDownloadAsync(download24, "PortalIDSF.xlsx");
                        if (pagina.BaixarExcel == "❌")
                        {
                            errosTotais++;
                        }
                    });
                    await page25.CloseAsync();


                    var page26 = await Page.RunAndWaitForPopupAsync(async () =>
                    {
                        var download25 = await Page.RunAndWaitForDownloadAsync(async () =>
                        {
                            await Page.GetByRole(AriaRole.Link, new() { Name = "01-06-04_1 - Politica de Gest" }).ClickAsync();
                        });
                        pagina.BaixarExcel = await Utils.Excel.ValidarDownloadAsync(download25, "PortalIDSF.xlsx");
                        if (pagina.BaixarExcel == "❌")
                        {
                            errosTotais++;
                        }
                    });
                    await page26.CloseAsync();


                    await Page.GetByRole(AriaRole.Cell, new() { Name = " Tecnologia" }).ClickAsync();
                    var page27 = await Page.RunAndWaitForPopupAsync(async () =>
                    {
                        var download26 = await Page.RunAndWaitForDownloadAsync(async () =>
                        {
                            await Page.GetByRole(AriaRole.Link, new() { Name = "01-16-01_1 - Politica" }).ClickAsync();
                        });
                        pagina.BaixarExcel = await Utils.Excel.ValidarDownloadAsync(download26, "PortalIDSF.xlsx");
                        if (pagina.BaixarExcel == "❌")
                        {
                            errosTotais++;
                        }
                    });
                    await page27.CloseAsync();


                    var page28 = await Page.RunAndWaitForPopupAsync(async () =>
                    {
                        var download27 = await Page.RunAndWaitForDownloadAsync(async () =>
                        {
                            await Page.GetByRole(AriaRole.Link, new() { Name = "-16-02_1 - Politica de PCN.pdf" }).ClickAsync();
                        });
                        pagina.BaixarExcel = await Utils.Excel.ValidarDownloadAsync(download27, "PortalIDSF.xlsx");
                        if (pagina.BaixarExcel == "❌")
                        {
                            errosTotais++;
                        }
                    });
                    await page28.CloseAsync();


                    await Page.GetByRole(AriaRole.Cell, new() { Name = " Versoes Disponibilizadas no" }).ClickAsync();
                    var page29 = await Page.RunAndWaitForPopupAsync(async () =>
                    {
                        var download28 = await Page.RunAndWaitForDownloadAsync(async () =>
                        {
                            await Page.GetByRole(AriaRole.Link, new() { Name = "Politica Investimentos" }).ClickAsync();
                        });
                        pagina.BaixarExcel = await Utils.Excel.ValidarDownloadAsync(download28, "PortalIDSF.xlsx");
                        if (pagina.BaixarExcel == "❌")
                        {
                            errosTotais++;
                        }
                    });
                    await page29.CloseAsync();





                }
                else
                {
                    Console.Write("Erro ao carregar a página de Politicas no tópico Controle Interno: ");
                    pagina.Nome = "Políticas - Controle Interno";
                    pagina.StatusCode = ControlePoliticas.Status;
                    errosTotais++;
                    await Page.GotoAsync("https://portal.idsf.com.br/Home.aspx");
                }
            }
            catch (TimeoutException ex)
            {
                Console.WriteLine("Timeout de 2000ms excedido, continuando a execução...");
                Console.WriteLine($"Exceção: {ex.Message}");
                errosTotais++;
                pagina.TotalErros = errosTotais;
                return pagina;
            }
            pagina.TotalErros = errosTotais;
            return pagina;
        }
    }
}
