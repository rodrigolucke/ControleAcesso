//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace ControleAcesso.Models
{
    using System;
    using System.Collections.Generic;

    public partial class usuario
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public usuario()
        {
            this.usuario_grupo = new HashSet<usuario_grupo>();
        }
    
        public int id_usuario { get; set; }
        public string login_usuario { get; set; }
        public string senha { get; set; }
        public int pessoa_id_pessoa { get; set; }
    
        public virtual pessoa pessoa { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<usuario_grupo> usuario_grupo { get; set; }

       
    }
}
