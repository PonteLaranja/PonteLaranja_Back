using System;
using System.Collections.Generic;

namespace API.Domains;

public partial class Item
{
    public Guid ItemID { get; set; }

    public string NomeItem { get; set; } = null!;

    public DateTime? DataRecebimento { get; set; }

    public DateTime? DataDoacao { get; set; }

    public decimal Medida { get; set; }

    public int Quantidade { get; set; }

    public bool Ativo { get; set; }

    public Guid? TipoItemID { get; set; }

    public Guid? TipoMedidaID { get; set; }

    public Guid? UsuarioID { get; set; }

    public Guid? UnidadeID { get; set; }

    public virtual TipoItem? TipoItem { get; set; }

    public virtual TipoMedida? TipoMedida { get; set; }

    public virtual ICollection<Transferencia> Transferencia { get; set; } = new List<Transferencia>();

    public virtual Unidade? Unidade { get; set; }

    public virtual Usuario? Usuario { get; set; }
}
