using System;
using System.Collections.Generic;

namespace API.Domains;

public partial class TipoMedida
{
    public Guid TipoMedidaID { get; set; }

    public string NomeTipoMedida { get; set; } = null!;

    public virtual ICollection<Item> Item { get; set; } = new List<Item>();
}
