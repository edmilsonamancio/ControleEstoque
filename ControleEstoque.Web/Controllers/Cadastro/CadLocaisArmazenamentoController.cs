using ControleEstoque.Web.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;

namespace ControleEstoque.Web.Controllers.Cadastro
{
	[Authorize(Roles = "Gerente,Administrativo,Operador")]
	public class CadLocaisArmazenamentoController : Controller
	{
		private const int _quantMaxLinhasPorPagina = 5;

		public ActionResult Index()
		{
			ViewBag.ListaTamPag = new SelectList(new int[] { _quantMaxLinhasPorPagina, 10, 15, 20 }, _quantMaxLinhasPorPagina);
			ViewBag.QuantMaxLinhasPorPagina = _quantMaxLinhasPorPagina;
			ViewBag.PaginaAtual = 1;

			var lista = LocaisArmazenamentoModel.RecuperarLista(ViewBag.PaginaAtual, _quantMaxLinhasPorPagina);
			var quant = LocaisArmazenamentoModel.RecuperarQuantidade();

			var difQuantPaginas = (quant % ViewBag.QuantMaxLinhasPorPagina) > 0 ? 1 : 0;
			ViewBag.QuantPaginas = (quant / ViewBag.QuantMaxLinhasPorPagina) + difQuantPaginas;

			return View(lista);
		}

		[HttpPost]
		[ValidateAntiForgeryToken]
		public JsonResult LocaisArmazenamentoPagina(int pagina, int tamPag)
		{
			var lista = LocaisArmazenamentoModel.RecuperarLista(pagina, tamPag);

			return Json(lista);
		}

		[HttpPost]
		[ValidateAntiForgeryToken]
		public JsonResult RecuperarLocaisArmazenamento(int id)
		{
			return Json(LocaisArmazenamentoModel.RecuperarPeloId(id));
		}

		[HttpPost]
		[Authorize(Roles = "Gerente,Administrativo")]
		[ValidateAntiForgeryToken]
		public JsonResult ExcluirLocaisArmazenamento(int id)
		{
			return Json(LocaisArmazenamentoModel.ExcluirPeloId(id));
		}

		[HttpPost]
		[ValidateAntiForgeryToken]
		public JsonResult SalvarLocaisArmazenamento(LocaisArmazenamentoModel model)
		{
			var resultado = "OK";
			var mensagens = new List<string>();
			var idSalvo = string.Empty;

			if (!ModelState.IsValid)
			{
				resultado = "AVISO";
				mensagens = ModelState.Values.SelectMany(x => x.Errors).Select(x => x.ErrorMessage).ToList();
			}
			else
			{
				try
				{
					var id = model.Salvar();
					if (id > 0)
					{
						idSalvo = id.ToString();
					}
					else
					{
						resultado = "ERRO";
					}
				}
				catch (Exception ex)
				{
					resultado = "ERRO";
				}
			}
			return Json(new { Resultado = resultado, Mensagens = mensagens, IdSalvo = idSalvo });
		}
	}
}