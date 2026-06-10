using System;
using System.Collections.Generic;

namespace API.Domains;

public partial class Usuario
{
    public Guid UsuarioID { get; set; }

    public string Nome { get; set; } = null!;

    public string? Email { get; set; }

    public byte[]? Senha { get; set; }

    public DateTime? DataCriacao { get; set; }

    public Guid TipoUsuarioID { get; set; }

    public virtual ICollection<Item> Item { get; set; } = new List<Item>();

    public virtual TipoUsuario TipoUsuario { get; set; } = null!;

    public virtual ICollection<Unidade> Unidade { get; set; } = new List<Unidade>();
}
