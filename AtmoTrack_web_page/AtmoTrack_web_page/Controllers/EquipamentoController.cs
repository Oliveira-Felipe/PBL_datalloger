using AtmoTrack_web_page.DAO;
using AtmoTrack_web_page.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.Diagnostics;

namespace AtmoTrack_web_page.Controllers
{
    public class EquipamentoController : Controller
    {
        public IActionResult Index()
        {
            try
            {
                EquipamentoDAO dao = new EquipamentoDAO();
                var eq = dao.Listagem();


                return View(eq);
            }
            catch (Exception erro)
            {

            }
            return View();
        }

        public IActionResult NovoRegistro()
        {
            try
            {
                ViewBag.cadastroEquipamento = "I";
                EquipamentoDAO dao = new EquipamentoDAO();
                EquipamentoViewModel eq = new EquipamentoViewModel();
                eq.Id = dao.LastId();

                var empresas = dao.GetAllEmpresas().Select(e => new SelectListItem
                {
                    Value = e.Id.ToString(),
                    Text = e.NomeFantasia
                }).ToList();

                ViewBag.Empresas = empresas;

                return View("Form", eq);
            }
            catch (Exception erro)
            {
                var errorViewModel = new ErrorViewModel
                {
                    ErrorMessage = erro.Message,
                    RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier
                };

                return View("Error", errorViewModel);
            }
        }

        public IActionResult Salvar(EquipamentoViewModel em, string cadastroEquipamento)
        {
            try
            {
                var dao = new EquipamentoDAO();
                if (cadastroEquipamento == "I")
                {
                    dao.Inserir(em);
                }
                else
                {
                    dao.Alterar(em);
                }

                return RedirectToAction("Index");
            }
            //catch (SqlException sqlEx)
            //{
            //    // Log or print the specific SQL error message
            //    //Console.WriteLine(sqlEx.Message);  // ou use um logger apropriado
            //    //var errorViewModel = new ErrorViewModel
            //    //{
            //    //    ErrorMessage = sqlEx.Message,
            //    //    RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier
            //    //};
            //    //return View("Error", errorViewModel);
            //}
            catch (Exception erro)
            {
                //var errorViewModel = new ErrorViewModel
                //{
                //    ErrorMessage = erro.Message,
                //    RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier
                //};
                //Console.WriteLine(erro.Message);
                //return View("Error", errorViewModel);
                return View("Error", erro.ToString());
            }
        }

        public IActionResult Editar(int id)
        {
            try
            {
                ViewBag.cadastroEquipamento = "A";
                EquipamentoDAO dao = new EquipamentoDAO();
                EquipamentoViewModel eq = dao.Consulta(id);
                var empresas = dao.GetAllEmpresas().Select(e => new SelectListItem
                {
                    Value = e.Id.ToString(),
                    Text = e.NomeFantasia
                }).ToList();

                ViewBag.Empresas = empresas;

                return View("Form", eq);
            }
            catch (Exception erro)
            {
                return View("Erro", erro.ToString());
            }
        }

        public IActionResult Delete(int id)
        {
            try
            {
                EquipamentoDAO dao = new EquipamentoDAO();
                dao.Excluir(id);
                return RedirectToAction("Index");
            }
            catch (Exception erro)
            {
                return View("Erro", erro.ToString());
            }
        }

        public IActionResult Exibir(int id)
        {
            try
            {
                EquipamentoDAO dao = new EquipamentoDAO();
                var eq = dao.Consulta(id);
                if (eq == null)
                {
                    return NotFound();
                }

                var empresa = dao.ConsultaEmpresa(eq.EmpresaId);

                ViewBag.EmpresaNome = empresa != null ? empresa.NomeFantasia : "Empresa não encontrado";

                return View("ExibirEmpresa", eq);
            }
            catch (Exception erro)
            {
                return View("Erro", erro.ToString());
            }
        }
    }
}
