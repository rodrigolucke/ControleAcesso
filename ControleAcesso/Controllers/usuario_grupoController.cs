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
    [Authorize(Roles = "administrador")]
    public class usuario_grupoController : Controller
    {

        private Database1Entities1 db = new Database1Entities1();

        // GET: usuario_grupo

        public ActionResult Index()
        {
            var usuario_grupo = db.usuario_grupo.Include(u => u.grupo).Include(u => u.usuario);
            return View(usuario_grupo.ToList());
        }

        // GET: usuario_grupo/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            usuario_grupo usuario_grupo = db.usuario_grupo.Find(id);
            if (usuario_grupo == null)
            {
                return HttpNotFound();
            }
            return View(usuario_grupo);
        }

        // GET: usuario_grupo/Create
        public ActionResult Create()
        {
            ViewBag.id_grupo = new SelectList(db.grupo, "id_grupo", "nome_grupo");
            ViewBag.id_usuario = new SelectList(db.usuario, "id_usuario", "login_usuario");
            return View();
        }

        // POST: usuario_grupo/Create
        // Para se proteger de mais ataques, habilite as propriedades específicas às quais você quer se associar. Para 
        // obter mais detalhes, veja https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "seq,id_usuario,id_grupo")] usuario_grupo usuario_grupo)
        {
            if (ModelState.IsValid)
            {
                db.usuario_grupo.Add(usuario_grupo);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.id_grupo = new SelectList(db.grupo, "id_grupo", "nome_grupo", usuario_grupo.id_grupo);
            ViewBag.id_usuario = new SelectList(db.usuario, "id_usuario", "login_usuario", usuario_grupo.id_usuario);
            return View(usuario_grupo);
        }

        // GET: usuario_grupo/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            usuario_grupo usuario_grupo = db.usuario_grupo.Find(id);
            if (usuario_grupo == null)
            {
                return HttpNotFound();
            }
            ViewBag.id_grupo = new SelectList(db.grupo, "id_grupo", "nome_grupo", usuario_grupo.id_grupo);
            ViewBag.id_usuario = new SelectList(db.usuario, "id_usuario", "login_usuario", usuario_grupo.id_usuario);
            return View(usuario_grupo);
        }

        // POST: usuario_grupo/Edit/5
        // Para se proteger de mais ataques, habilite as propriedades específicas às quais você quer se associar. Para 
        // obter mais detalhes, veja https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "seq,id_usuario,id_grupo")] usuario_grupo usuario_grupo)
        {
            if (ModelState.IsValid)
            {
                db.Entry(usuario_grupo).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.id_grupo = new SelectList(db.grupo, "id_grupo", "nome_grupo", usuario_grupo.id_grupo);
            ViewBag.id_usuario = new SelectList(db.usuario, "id_usuario", "login_usuario", usuario_grupo.id_usuario);
            return View(usuario_grupo);
        }

        // GET: usuario_grupo/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            usuario_grupo usuario_grupo = db.usuario_grupo.Find(id);
            if (usuario_grupo == null)
            {
                return HttpNotFound();
            }
            return View(usuario_grupo);
        }

        // POST: usuario_grupo/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            usuario_grupo usuario_grupo = db.usuario_grupo.Find(id);
            db.usuario_grupo.Remove(usuario_grupo);
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
