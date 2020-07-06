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
            ViewBag.pessoa_id_pessoa = new SelectList(db.pessoa, "id_pessoa", "nome");
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
            ViewBag.pessoa_id_pessoa = new SelectList(db.pessoa, "id_pessoa", "nome", usuario.pessoa_id_pessoa);
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
                usuario.senha = Criptografia.Codifica(usuario.senha);
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
