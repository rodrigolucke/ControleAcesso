using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using ControleAcesso.Models;


namespace ControleAcesso.Controllers
{
    public class usuariosController : Controller
    {
        private Database1Entities1 db = new Database1Entities1();

        // GET: usuarios
        [Authorize(Roles = "administrador,usuarios")]
        public ActionResult Index()
        {
            var usuario = db.usuario.Include(u => u.pessoa);
            return View(usuario.ToList());
        }

        // GET: usuarios/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            usuario usuario = db.usuario.Find(id);
            if (usuario == null)
            {
                return HttpNotFound();
            }
            return View(usuario);
        }

        // GET: usuarios/Create
        [Authorize(Roles = "administrador")]
        public ActionResult Create()
        {

            var itemIds = db.usuario.Select(x => x.pessoa_id_pessoa).ToArray();
            var listaPessoasDisponiveis = db.pessoa.Where(x => !itemIds.Contains(x.id_pessoa));


            ViewBag.pessoa_id_pessoa = new SelectList(listaPessoasDisponiveis, "id_pessoa", "nome");
            return View();
        }

        // POST: usuarios/Create
        // Para se proteger de mais ataques, habilite as propriedades específicas às quais você quer se associar. Para 
        // obter mais detalhes, veja https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "administrador")]
        public ActionResult Create([Bind(Include = "id_usuario,login_usuario,senha,pessoa_id_pessoa")] usuario usuario)
        {
            if (ModelState.IsValid)
            {
                db.usuario.Add(usuario);
                usuario.senha = Criptografia.Codifica(usuario.senha);
                db.SaveChanges();

                return RedirectToAction("Index");
            }

            ViewBag.pessoa_id_pessoa = new SelectList(db.pessoa, "id_pessoa", "nome", usuario.pessoa_id_pessoa);
            return View(usuario);
        }

        // GET: usuarios/Edit/5
        [Authorize(Roles = "administrador")]
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            usuario usuario = db.usuario.Find(id);
            if (usuario == null)
            {
                return HttpNotFound();
            }
            var listaPessoasDisponiveis = db.pessoa.Where(x => x.id_pessoa.Equals(usuario.pessoa_id_pessoa));
            ViewBag.pessoa_id_pessoa = new SelectList(listaPessoasDisponiveis, "id_pessoa", "nome");
            return View(usuario);
        }

        // POST: usuarios/Edit/5
        // Para se proteger de mais ataques, habilite as propriedades específicas às quais você quer se associar. Para 
        // obter mais detalhes, veja https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "administrador")]
        public ActionResult Edit([Bind(Include = "id_usuario,login_usuario,senha,pessoa_id_pessoa")] usuario usuario)
        {
            if (ModelState.IsValid)
            {   
                db.Entry(usuario).State = EntityState.Modified;

                usuario us = db.usuario.Find(usuario.id_usuario);

                if (!us.senha.Equals(usuario.senha))
                {
                    usuario.senha = Criptografia.Codifica(usuario.senha);
                }
                else
                {
                    usuario.senha = us.senha;
                }
                usuario.senha = Criptografia.Codifica(usuario.senha);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            var listaPessoasDisponiveis = db.pessoa.Where(x => x.id_pessoa.Equals(usuario.pessoa_id_pessoa));
            ViewBag.pessoa_id_pessoa = new SelectList(listaPessoasDisponiveis, "id_pessoa", "nome");
            return View(usuario);
        }

        [Authorize(Roles = "administrador,usuarios")]
        public ActionResult Editar(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            usuario us = db.usuario.Find(id);
            if (us == null)
            {
                return HttpNotFound();
            }
            if (us.pessoa.email != HttpContext.User.Identity.Name)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
           // var itemIds = db.usuario.Select(x => x.pessoa_id_pessoa).ToArray();
            var listaPessoasDisponiveis = db.pessoa.Where(x => x.id_pessoa.Equals(us.pessoa_id_pessoa));


            ViewBag.pessoa_id_pessoa = new SelectList(listaPessoasDisponiveis, "id_pessoa", "nome");
            return View(us);
        }


        // POST: pessoas/Edit/5
        // Para se proteger de mais ataques, habilite as propriedades específicas às quais você quer se associar. Para 
        // obter mais detalhes, veja https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "administrador,usuarios")]
        public ActionResult Editar([Bind(Include = "id_usuario,login_usuario,senha,pessoa_id_pessoa")] usuario usuario)
        {
            if (ModelState.IsValid)
            {
                db.Entry(usuario).State = EntityState.Modified;
                usuario us =  db.usuario.Find(usuario.id_usuario);


                if (!us.senha.Equals(usuario.senha))
                {
                    usuario.senha = Criptografia.Codifica(usuario.senha);
                }
                else
                {
                    usuario.senha = us.senha;
                }

                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.pessoa_id_pessoa = new SelectList(db.pessoa, "id_pessoa", "nome", usuario.pessoa_id_pessoa);
            return View(usuario);
        }

        // GET: usuarios/Delete/5
        [Authorize(Roles = "administrador")]
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            usuario usuario = db.usuario.Find(id);
            if (usuario == null)
            {
                return HttpNotFound();
            }
            return View(usuario);
        }

        // POST: usuarios/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "administrador")]
        public ActionResult DeleteConfirmed(int id)
        {
            usuario usuario = db.usuario.Find(id);
            db.usuario.Remove(usuario);
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}
