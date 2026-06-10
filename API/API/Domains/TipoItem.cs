using System;
using System.Collections.Generic;

namespace API.Domains;

public partial class TipoItem
{
    public Guid TipoItemID { get; set; }

    public string NomeTipoItem { get; set; } = null!;

    public virtual ICollection<Item> Item { get; set; } = new List<Item>();
}
