using System;
using System.Collections.Generic;

namespace API.Domains;

public partial class Transferencia
{
    public Guid TransferenciaID { get; set; }

    public DateTime? DataEnvio { get; set; }

    public DateTime? DataChegada { get; set; }

    public Guid UnidadeOrigemID { get; set; }

    public Guid UnidadeDestinoID { get; set; }

    public Guid EstadoTransferenciaID { get; set; }

    public Guid ItemID { get; set; }

    public virtual EstadoTransferencia EstadoTransferencia { get; set; } = null!;

    public virtual Item Item { get; set; } = null!;

    public virtual Unidade UnidadeDestino { get; set; } = null!;

    public virtual Unidade UnidadeOrigem { get; set; } = null!;
}
