using System;
using System.Collections.Generic;

namespace API.Domains;

public partial class EstadoTransferencia
{
    public Guid EstadoTransID { get; set; }

    public string EstadoTransNome { get; set; } = null!;

    public virtual ICollection<Transferencia> Transferencia { get; set; } = new List<Transferencia>();
}
