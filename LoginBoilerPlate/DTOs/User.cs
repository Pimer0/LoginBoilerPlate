using System;
using System.Collections.Generic;

namespace LoginBoilerPlate;

public partial class User
{
    public int IdUser { get; set; }

    public string Nom { get; set; } = null!;

    public string Prenom { get; set; } = null!;

    public string Mail { get; set; } = null!;

    public string MotDePasse { get; set; } = null!;

    public bool Valide { get; set; }

    public byte[] PhotoProfile { get; set; } = null!;

    public int IdRole { get; set; }

    public virtual Role IdRoleNavigation { get; set; } = null!;

    public virtual ICollection<ResetMotDePasse> ResetMotDePasses { get; set; } = new List<ResetMotDePasse>();
}
