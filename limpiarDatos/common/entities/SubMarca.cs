using System;

namespace limpiarDatos.common.entities
{
    public class SubMarca
    {
        public SubMarca()
        {
        }

        public string nombre { get; set; }
        public List<ModeloSubMar> listModeloSubMar { get; set; }
        public int? idMarcas { get; set; }
        public int id { get; set; }
    }
}

