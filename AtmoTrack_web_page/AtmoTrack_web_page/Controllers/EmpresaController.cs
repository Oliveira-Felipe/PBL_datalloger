using AtmoTrack_web_page.DAO;
using AtmoTrack_web_page.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.Diagnostics;

namespace AtmoTrack_web_page.Controllers
{
    public class EmpresaController : Controller
    {
        public IActionResult Index()
        {
            try
            {
                EmpresaDAO dao = new EmpresaDAO();
                var em = dao.Listagem();

                return View(em);
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
                ViewBag.cadastroEmpresa = "I";
                EmpresaDAO dao = new EmpresaDAO();
                EmpresaViewModel em = new EmpresaViewModel();
                em.Id = dao.LastId();

                var estados = dao.GetAllStates().Select(e => new SelectListItem
                {
                    Value = e.Id.ToString(),
                    Text = e.Estado
                }).ToList();

                ViewBag.Estados = estados;

                return View("Form", em);
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

        public IActionResult Salvar(EmpresaViewModel em, string cadastroEmpresa)
        {
            try
            {
                var dao = new EmpresaDAO();
                if (cadastroEmpresa == "I")
                {
                    dao.Inserir(em);
                }
                else
                {
                    dao.Alterar(em);
                }

                return RedirectToAction("Index");
            }
            catch (Exception erro)
            {
                return View("Error", erro.ToString());
            }
        }

        public IActionResult GetCidades(int estadoId)
        {
            try
            {
                EmpresaDAO dao = new EmpresaDAO();
                var cidades = dao.GetAllCitiesEstadoId(estadoId);
                return Json(cidades);
            }
            catch (Exception erro)
            {
                return Json(new { error = erro.Message });
            }
        }

        public IActionResult Editar(int id)
        {
            try
            {
                ViewBag.cadastroEmpresa = "A";
                EmpresaDAO dao = new EmpresaDAO();
                EmpresaViewModel em = dao.Consulta(id);
                var estados = dao.GetAllStates().Select(e => new SelectListItem
                {
                    Value = e.Id.ToString(),
                    Text = e.Estado
                }).ToList();

                ViewBag.Estados = estados;

                return View("Form", em);
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
                EmpresaDAO dao = new EmpresaDAO();
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
                EmpresaDAO dao = new EmpresaDAO();
                var em = dao.Consulta(id);
                if (em == null)
                {
                    return NotFound();
                }

                var estado = dao.ConsultaEstado(em.EstadoId);

                ViewBag.EstadoNome = estado != null ? estado.Estado : "Estado não encontrado";

                if (estado == null)
                {
                    return NotFound();
                }

                var cidade = dao.ConsultaCidade(em.CidadeId);
                ViewBag.CidadeNome = cidade != null ? cidade.Cidade : "Estado não encontrado";

                return View("ExibirEmpresa", em);
            }
            catch (Exception erro)
            {
                return View("Erro", erro.ToString());
            }
        }
    }
}
