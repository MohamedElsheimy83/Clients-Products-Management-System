//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace Mohamed_Projet_426
{
    using System;
    using System.Collections.Generic;
    
    public partial class Commande
    {
        public int CommandeID { get; set; }
        public string ClientID { get; set; }
        public Nullable<int> EmployeID { get; set; }
        public Nullable<System.DateTime> DateCommande { get; set; }
        public Nullable<System.DateTime> DateRequise { get; set; }
        public Nullable<System.DateTime> DateEnvoi { get; set; }
        public string AdresseEnvoi { get; set; }
        public string VilleEnvoi { get; set; }
        public string CodePostalEnvoi { get; set; }
        public string PaysEnvoi { get; set; }
    
        public virtual Client Client { get; set; }
        public virtual Employe Employe { get; set; }
    }
}
