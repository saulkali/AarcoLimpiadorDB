using System;
using System.Collections.Generic;

namespace limpiarDatos.Models.Entities
{
    public partial class SubMarca
    {
        public SubMarca()
        {
            ModelSubMar = new HashSet<ModelSubMar>();
        }

        public int Id { get; set; }
        public string Nombre { get; set; } = null!;
        public int? IdMarcas { get; set; }

        public virtual Marcas? IdMarcasNavigation { get; set; }
        public virtual ICollection<ModelSubMar> ModelSubMar { get; set; }
    }
}
