using System;
using System.Collections.Generic;

namespace limpiarDatos.Models.Entities
{
    public partial class Descripcion
    {
        public int Id { get; set; }
        public string Descripcion1 { get; set; } = null!;
        public string DescripcionId { get; set; } = null!;
        public int? IdModeloSubM { get; set; }

        public virtual ModelSubMar? IdModeloSubMNavigation { get; set; }
    }
}
