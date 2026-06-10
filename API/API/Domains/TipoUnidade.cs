using System;
using System.Collections.Generic;

namespace API.Domains;

public partial class TipoUnidade
{
    public Guid TipoUnidadeID { get; set; }

    public string NomeTipoUnidade { get; set; } = null!;

    public virtual ICollection<Unidade> Unidade { get; set; } = new List<Unidade>();
}
