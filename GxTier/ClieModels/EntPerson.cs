namespace GxTie.ClieModels
{
    public interface IHasRowguid
    {
        Guid Rowguid { get; set; }
    }
    public class EntPerson : IHasRowguid
    {
        public Guid Rowguid { get; set; }
        public int Id { get; set; }
        public int Iui { get; set; } = 0;
        public int Xadd1 { get; set; } = 0;
        public int Xedt1 { get; set; } = 0;
        public int Xdel1 { get; set; } = 0;
        // Extended fields (2–5)
        public string Liba { get; set; } = string.Empty;
        public string KMatri { get; set; } = string.Empty;
        public string Xmatri { get; set; } = "";
        public string Nom { get; set; } = "";
        public string Pnom { get; set; } = "";
        public string Bnom { get; set; } = "";   // Nom de jeune fille

        // Extended fields (6–9)
        public int Statut { get; set; }
        public int Relat { get; set; }
        public int Sexe { get; set; }
        public int Sitmat { get; set; }

        // Extended fields (10–16)
        public int Ifonc { get; set; }   // Fonction
        public int Igrad { get; set; }   // Grade
        public string Email { get; set; } = "";
        public string Phone { get; set; } = "";
        public DateTime? Dnais { get; set; }     // Date de naissance
        public DateTime? Demb { get; set; }      // Date d’embauche
        public int Nenf { get; set; }            // Nombre d’enfants

        // Extended fields (17–20)
        public int Respon { get; set; }          // Responsabilité
        public string Rpays { get; set; } = "";  // Pays de résidence
        public string Natio { get; set; } = "";  // Nationalité
        public int Ipaid { get; set; }           // Méthode de paiement

    }
}
