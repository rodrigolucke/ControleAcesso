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

                string i = Criptografia.Codifica(user.senha);

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
        [Authorize(Roles = "recuperacao")]
        public ActionResult RecuperaSenha(int idususario)
        {

            usuario usuario = db.usuario.Find(idususario);
            if (usuario == null)
            {
                return HttpNotFound();
            }

            return View(usuario);
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
            SmtpClient mailClient = new SmtpClient();
            //This object stores the authentication values      
            System.Net.NetworkCredential basicCredential =
                new System.Net.NetworkCredential("digolucke@gmail.com", "04171824");
            mailClient.Host = "smtp.gmail.com";
            mailClient.Port = 587;
            mailClient.EnableSsl = true;
            mailClient.DeliveryMethod = SmtpDeliveryMethod.Network;
            mailClient.UseDefaultCredentials = false;
            mailClient.Credentials = basicCredential;            



            MailMessage message = new MailMessage();

            MailAddress fromAddress = new MailAddress("info@mydomain.com");
            message.Body = "https://localhost:44344/Account/RecuperaSenha?idususario=" + vUser.id_usuario;
            message.From = fromAddress;
            //here you can set address    
            message.To.Add(p1.email);
            //here you can put your message details 

            mailClient.Send(message);
            usuario_grupo ug = new usuario_grupo();
            ug.id_usuario = vUser.id_usuario;
            ug.id_grupo = 3;
            db.usuario_grupo.Add(ug);

            db.SaveChanges();

            return RedirectToAction("Login", "Account");
        }

        [Authorize(Roles = "recuperacao")]
        public ActionResult EditarSenha()
        {
            int id = Int32.Parse(Request.Form["id_usuario"]);
            usuario u = db.usuario.Find(id);
            u.senha = Criptografia.Codifica(Request.Form["senha"]);
            db.SaveChanges();

            usuario_grupo ug = db.usuario_grupo.Where(
                    p => p.id_grupo.Equals(3)
                    ).Where(x=> x.id_usuario.Equals(id)).FirstOrDefault();

            db.usuario_grupo.Remove(ug);
            db.SaveChanges();

            return RedirectToAction("Login", "Account");
        }
       
    }
}