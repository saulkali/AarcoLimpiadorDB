using System;
namespace limpiarDatos.common.entities
{
    public class ModeloSubMar
    {
        public ModeloSubMar()
        {

        }
        public string nombre { get; set; }
        public List<Descripcion> listDescripciones { get; set; }
        public int id { get; set; }
        public int? idSubMarca { get; set; }
    }
}

