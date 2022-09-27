using System;
using System.Collections.Generic;

namespace limpiarDatos.Models.Entities
{
    public partial class Marcas
    {
        public Marcas()
        {
            SubMarca = new HashSet<SubMarca>();
        }

        public int Id { get; set; }
        public string Nombre { get; set; } = null!;

        public virtual ICollection<SubMarca> SubMarca { get; set; }
    }
}
