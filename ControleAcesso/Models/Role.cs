using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;
using System.Web.Security;

namespace ControleAcesso.Models
{
    public class Role : RoleProvider
    {
        Database1Entities1 db = new Database1Entities1();
        public override void AddUsersToRoles(string[] usernames, string[] roleNames)
        {
            throw new NotImplementedException();
        }

        public override string ApplicationName
        {
            get
            {
                throw new NotImplementedException();
            }
            set
            {
                throw new NotImplementedException();
            }
        }

        public override void CreateRole(string roleName)
        {
            throw new NotImplementedException();
        }

        public override bool DeleteRole(string roleName, bool throwOnPopulatedRole)
        {
            throw new NotImplementedException();
        }

        public override string[] FindUsersInRole(string roleName, string usernameToMatch)
        {
            throw new NotImplementedException();
        }

        public override string[] GetAllRoles()
        {
            throw new NotImplementedException();
        }

        public override string[] GetRolesForUser(string username)
        {


            List<string> list = new List<string>();         

            pessoa p1 = db.pessoa.Where(
                  p => p.email.Equals(username)).FirstOrDefault();

            usuario vUser = db.usuario.Where(
                    p => p.pessoa_id_pessoa.Equals(p1.id_pessoa)).FirstOrDefault();
           var usuarioGrupo = db.usuario_grupo.Where(p => p.id_usuario == vUser.id_usuario);
            string sRoles = "t";
            foreach (usuario_grupo ug in usuarioGrupo)
            {
                //list.Add(ug.grupo.nome_grupo);
                list.Add("administrador");
                sRoles = ug.grupo.nome_grupo;              
            
            }
            String[] retorno = list.ToArray();
            /* foreach (usuario_grupo ug in usuarioGrupo)
             {

                 list.Add(ug.grupo.nome_grupo);

             }*/


            //retorno = list.ToArray();


            return retorno;
            throw new Exception("Sem Acesso");

        }

        public int userId(string username)
        {
            pessoa p1 = db.pessoa.Where(
                 p => p.email.Equals(username)).FirstOrDefault();
            return p1.id_pessoa;
        }
        public override string[] GetUsersInRole(string roleName)
        {
            throw new NotImplementedException();
        }

        public override bool IsUserInRole(string username, string roleName)
        {
            throw new NotImplementedException();
        }

        public override void RemoveUsersFromRoles(string[] usernames, string[] roleNames)
        {
            throw new NotImplementedException();
        }

        public override bool RoleExists(string roleName)
        {
            throw new NotImplementedException();
        }
    }

}

