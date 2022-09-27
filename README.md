# AarcoLimpiadorDB
algoritmo para limpiar la base de datos y darle relación

# funciones 
1. divide la informacion en un objeto general llamado baseSql, y este es una entidad completa para dividir la informacion.
2. mantiene la informacion dividida pero sin los id en la listSqlBase
3. primero ordena la informacion y despues pasa a acomodarla en la base de datos
4. la lista ordenada ya cuenta con los id desde la base de datos para crear la relacion.

# bugs
1. aun no se a logrado limpiar y relacionar las id de descripciones con modeloSubMar
2. tal vez no fue la mejor manera xD pero se logro un 80%

# nota
1. para cargar la informacion es necesario agregarlo en csv y dividir la informacion por ','
2. el tiempo de demora es bastante, al realizar la migracion.
