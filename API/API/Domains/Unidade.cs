using System;
using System.Collections.Generic;

namespace API.Domains;

public partial class Unidade
{
    public Guid UnidadeID { get; set; }

    public string NomeUnidade { get; set; } = null!;

    public string CEP { get; set; } = null!;

    public Guid UsuarioResponsavelID { get; set; }

    public Guid TipoUnidadeID { get; set; }

    public virtual ICollection<Item> Item { get; set; } = new List<Item>();

    public virtual TipoUnidade TipoUnidade { get; set; } = null!;

    public virtual ICollection<Transferencia> TransferenciaUnidadeDestino { get; set; } = new List<Transferencia>();

    public virtual ICollection<Transferencia> TransferenciaUnidadeOrigem { get; set; } = new List<Transferencia>();

    public virtual Usuario UsuarioResponsavel { get; set; } = null!;
}
