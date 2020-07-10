using ControleAcesso.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Data;
using System.Data.Entity;
using System.Net;
using System.Net.Mail;
using System.Web.Security;

namespace ControleAcesso.Controllers
{
    public class AccountController : Controller
    {
        private Database1Entities1 db = new Database1Entities1();

        // POST: usuarios/Create
        // Para se proteger de mais ataques, habilite as propriedades específicas às quais você quer se associar. Para 
        // obter mais detalhes, veja https://go.microsoft.com/fwlink/?LinkId=317598.


        // <param name = "login" ></ param >
        // < param name="returnUrl"></param>
        // <returns></returns>
        public ActionResult Login(usuario user, string returnUrl)
        {
            var sessao = Session;


            if (TempData.ContainsKey("Error") && TempData["Error"].ToString() != null)
            {
                ModelState.AddModelError("Error", TempData.Peek("Error").ToString());
               
            }
       
            if (!ModelState.IsValid)
            {
                return View(user);
            }
            else
            {

              

                if (user.login_usuario == null && user.senha == null)
                {
                    return View(user);
                }
                var vUser = db.usuario.Where(
                    p => p.login_usuario.Equals(user.login_usuario)).FirstOrDefault();

               // string i = Criptografia.Codifica(user.senha);
                if (vUser != null && vUser.login_usuario == user.login_usuario && Criptografia.Compara(user.senha,vUser.senha))
                {
                    
                        FormsAuthentication.SetAuthCookie(vUser.pessoa.email, false);
                        if (Url.IsLocalUrl(returnUrl)
                        && returnUrl.Length > 1
                        && returnUrl.StartsWith("/")
                        && !returnUrl.StartsWith("//")
                        && returnUrl.StartsWith("/\\"))
                        
                        /*código abaixo cria uma session para armazenar o nome do usuário*/
                        Session["Nome"] = vUser.pessoa.nome;
                        Session["userid"] = vUser.id_usuario;
                        Session["idpessoa"] = vUser.pessoa_id_pessoa;

                    /*retorna para a tela inicial do Home*/
                    return RedirectToAction("Index", "Home");
                }
                else
                {
                    ModelState.AddModelError("Error", "Usuário sem acesso para usar o sistema!!!");
                }

            }

            return View(user);

        }
        public ActionResult Logout(usuario user, string returnUrl)
        {
                FormsAuthentication.SignOut();

              return RedirectToAction("Index", "Home");
        }

        public ActionResult Recupera()
        {
            return View();
        }


        // <param name = "idususario" ></ param >
        // <returns></returns>
      
        public ActionResult RecuperaSenha(int idususario, string code)
        {

            usuario usr = db.usuario.Find(idususario);

            if (usr == null)
            {
                return HttpNotFound();
            }

            usuario_grupo ug = db.usuario_grupo.Where(
                    p => p.id_grupo.Equals(3)
                    ).Where(x => x.id_usuario.Equals(usr.id_usuario)).FirstOrDefault();

            if (ug == null)
            {
                TempData["Error"] = "Usuário sem solicitacao de troca de senha!";

                return RedirectToAction("Login", "Account");
            }

            rec r = db.rec.Where(
                    p => p.id_suario.Equals(usr.id_usuario)
                    ).Where(x => x.str.Equals(code)).FirstOrDefault();

            if (r == null)
            {
                TempData["Error"] = "Código de verificação inválido!";

                return RedirectToAction("Login", "Account");
            }


            usr.senha = null;
            return View(usr);
        }

     

        public ActionResult EnviaEmail()
        {
            string emailDigitado = Request.Form["pessoa.email"];

            pessoa p1 = db.pessoa.Where(
                p => p.email.Equals(emailDigitado)).FirstOrDefault();

            if (p1 == null)
            {
                ModelState.AddModelError("Error", "Email não enconttrado");
                return RedirectToAction("Login", "Account");
            }
            usuario vUser = db.usuario.Where(
                    p => p.pessoa_id_pessoa.Equals(p1.id_pessoa)).FirstOrDefault();

            
            rec r2 = db.rec.Where(
                  p => p.id_suario.Equals(vUser.id_usuario)
                  ).FirstOrDefault();
            while (r2 != null)
            {
                 
                db.rec.Remove(r2);
                db.SaveChanges();
                r2 = db.rec.Where(
                  p => p.id_suario.Equals(vUser.id_usuario)
                  ).FirstOrDefault();
               
            }

            rec r = new rec();
            r.id_suario = vUser.id_usuario;
            Random rnd = new Random();
            rnd.Next(100, 1000000);
            r.str = rnd.Next(100, 1000000).ToString();
            db.rec.Add(r);
            db.SaveChanges();
           // SmtpClient mailClient = new SmtpClient();
           // System.Net.NetworkCredential basicCredential =
            //  new System.Net.NetworkCredential("pi3unisc@gmail.com", "pi3unisc123");
           //      new System.Net.NetworkCredential("digolucke@gmail.com", "04171824Thor#");
            //new System.Net.NetworkCredential("projetouniscpi@gmail.com", "projetouniscpi123");
           
           /* SmtpClient SmtpServer = new SmtpClient("smtp.gmail.com");
            var mail = new MailMessage();
            mail.From = new MailAddress("noreply@hotmail.com");
            mail.To.Add(p1.email);
            mail.Subject = "Recovery";
            string htmlBody;
            htmlBody = "http://luckerodrigo-001-site1.btempurl.com//Account/RecuperaSenha?idususario=" + vUser.id_usuario + "&code=" + r.str;
            mail.Body = htmlBody;
            SmtpServer.Port = 587;
            SmtpServer.UseDefaultCredentials = false;
            SmtpServer.Credentials = new System.Net.NetworkCredential("projetouniscpi@gmail.com", "projetouniscpi123");

            SmtpServer.EnableSsl = false;
            SmtpServer.Send(mail);*/


            MailMessage mail = new MailMessage();
            mail.To.Add(p1.email);
            mail.From = new MailAddress("noreply@gmail.com");
            mail.Subject = "Passwor Recovery";
            string Body = "http://luckerodrigo-001-site1.btempurl.com//Account/RecuperaSenha?idususario=" + vUser.id_usuario + "&code=" + r.str;
            mail.Body = Body;
            SmtpClient smtp = new SmtpClient("smtp.gmail.com", 25);
            // smtp.Host = "smtp.gmail.com"; //Or Your SMTP Server Address
            smtp.UseDefaultCredentials = false;
            smtp.EnableSsl = true;
            smtp.Credentials = new System.Net.NetworkCredential("informaticacolegio01@gmail.com", "colegio.01");
            //smtp.Credentials = new System.Net.NetworkCredential("projetouniscpi@gmail.com", "projetouniscpi123");
            // smtp.Port = 587;
            //Or your Smtp Email ID and Password
            smtp.Send(mail);
            /*    MailMessage message = new MailMessage();

                MailAddress fromAddress = new MailAddress("info@mydomain.com");
                message.Body = "http://luckerodrigo-001-site1.btempurl.com//Account/RecuperaSenha?idususario=" + vUser.id_usuario + "&code=" + r.str;
                message.From = fromAddress;
                //here you can set address    
                message.To.Add(p1.email);
                message.Body = "http://luckerodrigo-001-site1.btempurl.com//Account/RecuperaSenha?idususario=" + vUser.id_usuario+"&code=" + r.str;
                message.From = fromAddress;
                //here you can set address    
                message.To.Add(p1.email);
                //here you can put your message details 

                mailClient.Send(message);*/
            usuario_grupo ug = new usuario_grupo();
            ug.id_usuario = vUser.id_usuario;
            ug.id_grupo = 3;
            db.usuario_grupo.Add(ug);
            

            //Números de 10 até 999999
           

            db.SaveChanges();

            return RedirectToAction("Login", "Account");
        }

      
        public ActionResult EditarSenha()
        {
            int id = Int32.Parse(Request.Form["id_usuario"]);
            usuario_grupo ug = db.usuario_grupo.Where(
                   p => p.id_grupo.Equals(3)
                   ).Where(x => x.id_usuario.Equals(id)).FirstOrDefault();



            if (ug == null)
            {
                return HttpNotFound();
            }
            usuario u = db.usuario.Find(id);
            u.senha = Criptografia.Codifica(Request.Form["senha"]);
            db.SaveChanges();
            
            rec r = db.rec.Where(
                   p => p.id_suario.Equals(u.id_usuario)
                   ).FirstOrDefault();
            while (r != null)
            {
                 
                db.rec.Remove(r);
                db.SaveChanges();
                r = db.rec.Where(
                  p => p.id_suario.Equals(u.id_usuario)
                  ).FirstOrDefault();
               
            }
            db.usuario_grupo.Remove(ug);
            

            return RedirectToAction("Login", "Account");
        }
       
    }
}