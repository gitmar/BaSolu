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

        // Extended fields (2–5)
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
    public enum RowState
    {
        Normal,
        EditPending,
        DeletePending,
        AddPending,  // 🔥 NEW for Add
        Locked
    }
    public enum GridMode
    {
        PersAdding,
        PersEditing,
        AflsAdding,
        AflsEditing,
        None
    }
    public enum EntityLevel { Plan, Rub, Fmt, Atr }
}
