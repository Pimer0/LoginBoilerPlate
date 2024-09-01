using System;
using System.Collections.Generic;

namespace LoginBoilerPlate;

public partial class ResetMotDePasse
{
    public int IdReset { get; set; }

    public string ResetMotDePasse1 { get; set; } = null!;

    public DateOnly DateCreation { get; set; }

    public int IdUser { get; set; }

    public virtual User IdUserNavigation { get; set; } = null!;
}
