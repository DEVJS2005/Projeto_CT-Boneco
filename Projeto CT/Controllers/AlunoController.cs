using Projeto_CT.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity.Migrations;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Projeto_CT.Controllers
{
    public class AlunoController : Controller
    {
        BDEntities bd = new BDEntities();
        public bool VerificarLogin()
        {
            if (Session["UserAuth"] != null)
            {
                return true;
            }
            return false;
        }

        public FileContentResult getImg(int id)
        {
            byte[] byteArray = bd.aluno.Find(id).foto;
            return byteArray != null 
                ? new FileContentResult(byteArray,"image/jpge") : null;

        }

        public string getColor(int id)
        {
            string faixa = bd.graduacao.Find(id).corGraduacao;
            return faixa;
        }
        
        // GET: Aluno
        public ActionResult telaCadastro()
        {
            if (VerificarLogin())
            {
                ViewBag.tipoMatricula = bd.tipoMatricula.ToList();
                return View();
            }
            else
            {
                return RedirectToAction("telaLogin","Login");
            }
        }

        public ActionResult cadastro(string nome,string rg,string cpf,DateTime dataNascimento,string endereco,string planoSaude,string numeroAluno,string numeroParente,HttpPostedFileBase foto, int tipoMatricula) 
        { 
            usuario user = (usuario)Session["UserAuth"];


            aluno aluno = new aluno();  
            aluno.nome = nome;
            aluno.rg = rg;
            aluno.cpf = cpf;
            aluno.data_nascimento = dataNascimento;
            aluno.endereco = endereco;
            aluno.planoSaude = planoSaude;
            aluno.numeroAluno = numeroAluno;
            aluno.numeroParente = numeroParente;
            aluno.idTipoMatricula = tipoMatricula;
            aluno.idTipoMatricula = tipoMatricula; 
            aluno.dataPagamento = DateTime.Now.AddMonths(1);
            aluno.idUsuario = user.idUsuario;

            MemoryStream target = new MemoryStream();
            foto.InputStream.CopyTo(target);
            byte[] data = target.ToArray();
            aluno.foto = data;

            bd.aluno.Add(aluno);
            bd.SaveChanges();

            return RedirectToAction("Index","Home");
        }

        public ActionResult telaListarAlunos()
        {
            if (VerificarLogin())
            {
                ViewBag.alunos = bd.aluno.ToList().OrderBy(x => x.nome);
                return View();
            }
            else
            {
                return RedirectToAction("telaLogin","Login");
            }
        }

        public ActionResult ficha(int id)
        {
             graduacaoAluno gradAluno = null;

            int idGradAluno = 0;

            var quantGraduacao = bd.graduacao.SqlQuery("SELECT * FROM graduacao").ToList();
            for (int i= 1; i < quantGraduacao.Count ; i++)
            {
                gradAluno = bd.graduacaoAluno.Find(id, i);
                if(gradAluno != null)
                {
                    idGradAluno = gradAluno.idGraduacao;
                }
            }
            if (idGradAluno != 0)
            {
                ViewBag.graduacao = idGradAluno;
            }
            else
            {
                ViewBag.graduacao = 0;
            }
            ViewBag.aluno = bd.aluno.Find(id);
            return View();
        }

        public ActionResult telaEditar(int id)
        {
            if (VerificarLogin())
            {
                ViewBag.alunoEdit = bd.aluno.Find(id);
                ViewBag.tipoMatricula = bd.tipoMatricula.ToList();
                return View();
            }
            else
            {
                return RedirectToAction("telaLogin","Login");
            }
        }

        public ActionResult alterarCad(int idA,string nome, string rg, string cpf, DateTime dataNascimento, string endereco, string planoSaude, string numeroAluno, string numeroParente, HttpPostedFileBase foto, int tipoMatricula)
        {
            usuario user = (usuario)Session["UserAuth"];

            aluno aluno = bd.aluno.Find(idA);
            aluno.idAluno = idA;
            aluno.nome = nome;
            aluno.rg = rg;
            aluno.cpf = cpf;
            aluno.data_nascimento = dataNascimento;
            aluno.endereco = endereco;
            aluno.planoSaude = planoSaude;
            aluno.numeroAluno = numeroAluno;
            aluno.numeroParente = numeroParente;
            aluno.idTipoMatricula = tipoMatricula;
            aluno.idTipoMatricula = tipoMatricula;
            aluno.dataPagamento = DateTime.Now.AddMonths(1);
            aluno.idUsuario = user.idUsuario;

            if(foto != null)
            {
                MemoryStream target = new MemoryStream();
                foto.InputStream.CopyTo(target);
                byte[] data = target.ToArray();
                aluno.foto = data;
            }
            bd.aluno.AddOrUpdate(aluno);
            bd.SaveChanges();

            return RedirectToAction("telaListarAlunos", "Aluno");
        }

    }
}