using AtmoTrack_web_page.DAO;
using AtmoTrack_web_page.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.Data.SqlClient;
using System.Diagnostics;

namespace AtmoTrack_web_page.Controllers
{
    public class UsuarioController : Controller
    {
        public IActionResult Index()
        {
            try
            {
                UsuarioDAO dao = new UsuarioDAO();
                var us = dao.Listagem();

                return View(us);
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
                ViewBag.cadastroUsuario = "I";
                UsuarioDAO dao = new UsuarioDAO();
                UsuarioViewModel us = new UsuarioViewModel();
                us.Id = dao.LastId();

                var estados = dao.GetAllStates().Select(e => new SelectListItem
                {
                    Value = e.Id.ToString(),
                    Text = e.Estado
                }).ToList();

                ViewBag.Estados = estados;

                return View("Form",us);
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

        public IActionResult Salvar(UsuarioViewModel us, string cadastroUsuario)
        {
            try
            {
                var dao = new UsuarioDAO();
                if(cadastroUsuario == "I")
                {
                    dao.Inserir(us);
                }
                else
                {
                    dao.Alterar(us);
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
                ViewBag.cadastroUsuario = "A";
                UsuarioDAO dao = new UsuarioDAO();
                UsuarioViewModel us = dao.Consulta(id);
                var estados = dao.GetAllStates().Select(e => new SelectListItem
                {
                    Value = e.Id.ToString(),
                    Text = e.Estado
                }).ToList();

                ViewBag.Estados = estados;

                return View("Form", us);
            }
            catch(Exception erro)
            {
                return View("Erro", erro.ToString());
            }
        }

        public IActionResult Delete(int id)
        {
            try
            {
                UsuarioDAO dao = new UsuarioDAO();
                dao.Excluir(id);
                return RedirectToAction("Index");
            }catch (Exception erro)
            {
                return View("Erro", erro.ToString());
            }
        }

        public IActionResult Exibir(int id)
        {
            try
            {
                UsuarioDAO dao = new UsuarioDAO();
                var us = dao.Consulta(id);
                if (us == null)
                {
                    return NotFound();
                }

                var estado = dao.ConsultaEstado(us.EstadoId);

                ViewBag.EstadoNome = estado != null ? estado.Estado : "Estado não encontrado";

                if(estado == null)
                {
                    return NotFound();
                }

                var cidade = dao.ConsultaCidade(estado.Id);
                ViewBag.CidadeNome = cidade != null ? cidade.Cidade : "Estado não encontrado";

                return View("VisualizarUsuario", us);
            }catch(Exception erro)
            {
                return View("Erro", erro.ToString());
            }
        }
    }
}
