using System;
using System.Collections.Generic;

namespace LoginBoilerPlate;

public partial class Role
{
    public int IdRole { get; set; }

    public bool RoleUser { get; set; }

    public bool RoleAdmin { get; set; }

    public virtual ICollection<User> Users { get; set; } = new List<User>();
}
