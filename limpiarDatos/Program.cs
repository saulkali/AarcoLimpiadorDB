using System.Collections.Generic;
using System.IO;
using System.Data;
using System.Data.SqlClient;
using limpiarDatos.Models.Context;
using limpiarDatos.common.entities;

ApiContext dbContext = new ApiContext();

// variables para la base de datos
List<BaseSql> _listaDatosOrdenados = new List<BaseSql>();


// variables para el archivo txt 
string ruta = @"C:\Users\ADM\Downloads\marcasAutos.txt";
List<Datos> listDatos = new List<Datos>();
List<BaseSql> listBaseSql = new List<BaseSql>();

void ObtenerDatosCSV()
{
    string[] text = File.ReadAllLines(ruta);
    Console.WriteLine("Cargando archivo csv..");
    foreach (string linea in text)
    {

        string[] lineaCortada = linea.Split(',');
        Datos datos = new Datos();
        datos.marca = lineaCortada[0];
        datos.subMarca = lineaCortada[1];
        datos.modeloSubMarca = lineaCortada[2];
        datos.descripcion = lineaCortada[3];
        datos.descripcionId = lineaCortada[4];
        listDatos.Add(datos);
    }
    Console.Clear();
}


void ObtenerMarcas() {
    Console.WriteLine("Ordenando marcas... ");
    List<Marca> listMarcas = (from dato in listDatos
                              group dato by dato.marca into nuevoGrupo
                              select new Marca
                              {
                                  nombre = nuevoGrupo.Select(mar => mar.marca).FirstOrDefault()
                              }).ToList();

    // filtramos los repetidos de marcas
    foreach (Marca marcas in listMarcas)
    {
        BaseSql baseSql = new BaseSql();
        baseSql.marca = marcas;
        listBaseSql.Add(baseSql);
    }
    Console.Clear();
}

void ObtenerSubMarcas()
{
    Console.WriteLine("Ordenando SubMarcas relacion con Marcas");
    //limpiamos subMarcas
    foreach (BaseSql bsSql in listBaseSql)
    {
        List<SubMarca> listSubMarca = (from dato in listDatos
                                       group dato by dato.subMarca into nuevoGrupo
                                       where nuevoGrupo.Select(da=> da.marca).FirstOrDefault() == bsSql.marca.nombre
                                       select new SubMarca{
                                           nombre = nuevoGrupo.Select(da=> da.subMarca).FirstOrDefault()
                                       }
                                       ).ToList();
        bsSql.listSubMarca = listSubMarca;
    
    }
    Console.Clear();

}


void ObtenerModeloSubMarca()
{
    Console.WriteLine("Ordenando Modelos con relacion a submarcas");
    foreach (BaseSql bsSql in listBaseSql)
    {
        foreach (SubMarca subMarc in bsSql.listSubMarca)
        {
            List<ModeloSubMar> listModelSubMar = (from dato in listDatos
                                                  
                                                  where dato.subMarca == subMarc.nombre
                                                  group dato by dato.modeloSubMarca into nuevoGrupo
                                                  select new ModeloSubMar
                                                  {
                                                      nombre = nuevoGrupo.Select(da => da.modeloSubMarca).FirstOrDefault()
                                                  }).ToList();
            subMarc.listModeloSubMar = listModelSubMar;
        }
    }
    Console.Clear();
}


void ObtenerDescripciones()
{
    Console.WriteLine("Ordenando descripciones");
    foreach(BaseSql bsSql in listBaseSql)
    {
        foreach(SubMarca subMar in bsSql.listSubMarca)
        {
            foreach (ModeloSubMar modelSubMarc in subMar.listModeloSubMar)
            {
                List<Descripcion> listDescripciones = (from dato in listDatos
                                                       where dato.modeloSubMarca == modelSubMarc.nombre && dato.subMarca == subMar.nombre
                                                       group dato by dato.descripcion into nuevoGrupo
                                                       select new Descripcion
                                                       {
                                                           descripcion = nuevoGrupo.Select(da => da.descripcion).FirstOrDefault(),
                                                           descripcionId = nuevoGrupo.Select(da => da.descripcionId).FirstOrDefault()
                                                       }).ToList();
                modelSubMarc.listDescripciones = listDescripciones;
            }
        }
    }
    Console.Clear();
}



void AgregarMarcasSQL()
{
    if (dbContext.Marcas.ToList().Count == 0)
    {
        Console.WriteLine("Agregando las marcas ordenadas a la base de datos...");
        foreach (BaseSql bsSql in listBaseSql)
        {
            dbContext.Marcas.Add(new limpiarDatos.Models.Entities.Marcas
            {
                Nombre = bsSql.marca.nombre
            });
            dbContext.SaveChanges();
        }
        Console.Clear();
    }
    else
    {
        Console.WriteLine("tabla Marcas con datos... ");
        Console.WriteLine("Consultando datos marcas...");
        foreach (limpiarDatos.Models.Entities.Marcas marca in dbContext.Marcas.ToList())
        {
            Marca mar = new Marca();
            mar.id = marca.Id;
            mar.nombre = marca.Nombre;

            BaseSql basesql = new BaseSql();
            basesql.marca = mar;
            basesql.listSubMarca = new List<SubMarca>();

            _listaDatosOrdenados.Add(basesql);
        }
    }
        
    
}



void AgregarSubMarcas()
{
    if (dbContext.SubMarca.ToList().Count == 0)
    {
        Console.WriteLine("Agregando sub marcas con relacion a marcas a la base de datos");
        foreach (BaseSql bsSql in listBaseSql)
        {
            foreach (SubMarca subMarca in bsSql.listSubMarca)
            {
                int idMarca = _listaDatosOrdenados.Where(bs => bs.marca.nombre == bsSql.marca.nombre)
                    .Select(bs => bs.marca.id).FirstOrDefault();
                dbContext.SubMarca.Add(new limpiarDatos.Models.Entities.SubMarca
                {
                    Nombre = subMarca.nombre,
                    IdMarcas = idMarca
                });
                dbContext.SaveChanges();
            }
        }
        Console.Clear();
    }
    else
    {
        Console.WriteLine("cargando datos submarcas Base de datos...");
        foreach(limpiarDatos.Models.Entities.SubMarca subMarcas in dbContext.SubMarca.ToList())
        {
            SubMarca subMar = new SubMarca();
            subMar.id = subMarcas.Id;
            subMar.nombre = subMarcas.Nombre;
            subMar.idMarcas = subMarcas.IdMarcas;
            subMar.listModeloSubMar = new List<ModeloSubMar>();

            BaseSql bsSql = _listaDatosOrdenados.Where(bs => bs.marca.id == subMar.idMarcas).FirstOrDefault();
            bsSql.listSubMarca.Add(subMar);
            
        }
        Console.Clear();
    }
    
}


void AgregarModelSubMarca()
{
    if (dbContext.ModelSubMar.ToList().Count == 0)
    {
        Console.WriteLine("Agregando ModelSubMarca con relacion a sub marca a la base de datos...");
        foreach (BaseSql bsSql in listBaseSql)
        {
            foreach (SubMarca subMarca in bsSql.listSubMarca)
            {
                foreach (ModeloSubMar modelSubMarc in subMarca.listModeloSubMar)
                {
                    int idSubMarca = _listaDatosOrdenados.Where(bs => bs.marca.nombre == bsSql.marca.nombre)
                        .Select(mar => mar.listSubMarca).FirstOrDefault().Where(subM => subM.nombre == subMarca.nombre)
                        .Select(subM => subM.id).FirstOrDefault();
                    dbContext.ModelSubMar.Add(new limpiarDatos.Models.Entities.ModelSubMar
                    {
                        Nombre = modelSubMarc.nombre,
                        IdSubMarca = idSubMarca
                    });
                    dbContext.SaveChanges();
                }
            }
        }
        Console.Clear();
    }
    else
    {
        Console.WriteLine("cargando datos de ModelSubMarc");
        //foreach (limpiarDatos.Models.Entities.ModelSubMar modelSubMarca in dbContext.ModelSubMar.ToList())
        //{
        //    ModeloSubMar modelSub = new ModeloSubMar();
        //    modelSub.id = modelSubMarca.Id;
        //    modelSub.nombre = modelSubMarca.Nombre;
        //    modelSub.listDescripciones = new List<Descripcion>();
        //    modelSub.idSubMarca = modelSubMarca.IdSubMarca;

        //    _listaDatosOrdenados.Select(bs => bs.listSubMarca).Where(listSubMarc => listSubMarc
        //    .Select(subMar => subMar.id).FirstOrDefault() == modelSubMarca.IdSubMarca).FirstOrDefault()
        //    .Select(subMar => subMar.listModeloSubMar).FirstOrDefault().Add(modelSub); 
        //}
    }    
}


void AgregarDescripciones()
{
    
    if (dbContext.Descripcion.ToList().Count == 0)
    {
        Console.WriteLine("Agregando descripciones con relacion ");
        int counter = 0;
        foreach (BaseSql bsSql in listBaseSql)
        {
            foreach (SubMarca subMarca in bsSql.listSubMarca)
            {
                foreach (ModeloSubMar modelSubMarca in subMarca.listModeloSubMar)
                {
                    foreach (Descripcion descripcion in modelSubMarca.listDescripciones)
                    {
                        List<limpiarDatos.Models.Entities.ModelSubMar> listaModeloSubMar = dbContext.ModelSubMar.ToList();


                        int idModelSubMar = listaModeloSubMar.Where(modeloSub => 
                            modeloSub.IdSubMarca == dbContext.SubMarca.Where(subMar => 
                                subMar.Nombre == subMarca.nombre
                             ).Select(modelSub => modeloSub.Id).FirstOrDefault()
                        )
                            .Select(modeloSub => modeloSub.Id).FirstOrDefault();
                        dbContext.Descripcion.Add(new limpiarDatos.Models.Entities.Descripcion
                        {
                            Descripcion1 = descripcion.descripcion,
                            DescripcionId = descripcion.descripcionId,
                            IdModeloSubM = idModelSubMar
                        });
                        
                        Console.WriteLine($"total: {counter++}");
                    }
                    dbContext.SaveChanges();
                }
            }
        }
        
    }
    

}



// limpiando datos archivo txt o csv
ObtenerDatosCSV();
ObtenerMarcas();
ObtenerSubMarcas();
ObtenerModeloSubMarca();
ObtenerDescripciones();

// agregando los datos ordenados y con relacion a la base de datos
AgregarMarcasSQL();
AgregarSubMarcas();
AgregarModelSubMarca();
AgregarDescripciones();

Console.WriteLine("Migracion finalizada :)");
Console.ReadKey();