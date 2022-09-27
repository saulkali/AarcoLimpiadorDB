using limpiarDatos.common.entities;
using System;
namespace limpiarDatos.common.entities
{
    public class BaseSql
    {
        public BaseSql()
        {
        }

        public Marca marca { get; set; }
        public List<SubMarca> listSubMarca { get; set; }
    }
}

