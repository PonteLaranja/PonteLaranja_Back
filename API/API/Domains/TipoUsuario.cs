using System;
using System.Collections.Generic;

namespace API.Domains;

public partial class TipoUsuario
{
    public Guid TipoUsuarioID { get; set; }

    public string NomeTipoUsuario { get; set; } = null!;

    public virtual ICollection<Usuario> Usuario { get; set; } = new List<Usuario>();
}
