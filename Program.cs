// See https://aka.ms/new-console-template for more information
using auditoria;
using System.Globalization;
using System.Runtime.CompilerServices;
//Reemplazar por la ruta de archivo original.
string exportTo = @"C:\auditoria\";
string filePath = @"C:\auditoria\Resources\directorioNomiplus.txt";
//Se procede a leer el archivo intacto y original a auditar.
string[] content = File.ReadAllLines(filePath);
//Aqui se agregaran todos los ficheros encontrados y etiquetados como ejecutables.
List<Fichero> list = new();
CultureInfo culture = new("en-US");
//Identificacion de lineas procesadas.
var linea = 0;

foreach (string line in content)
{
    linea++;//Se cuentan las lineas procesadas, para identificar cual linea del documento estamos auditando.
    //Se separan las palabras por espacios en una linea.
    string[] row = line.Split(' ');
    //Se verifica que el primer dato de la linea, no este nula ni vacia.
    if (row[0] != null && row[0] != string.Empty)
    {
        //Convertimos este primer dato a su tipo original ya que es la fecha del fichero.
        DateTime tempDate = Convert.ToDateTime(row[0], culture);
        //Se Obtiene el nombre del fichero o extension, segun la ultima columna obtenida de la data. 
        string fileName = row[^1];
        //Se depuran las lineas para seleccionar las que tengan o posean informacion exclusiva sobre lo que buscamos (ejecutables).
        if (Procesar.IsExecute(fileName))
        {
            //Si coincide lo agregamos a la lista, con el numero de la linea, fecha,nombre del archivo y la linea original del archivo auditado.
            list.Add(new Fichero(linea,DateOnly.FromDateTime(tempDate), fileName,line));
        }
    }
}
//Se reordena la lista segun la fecha de modificacion o creacion del fichero.
list = list.OrderBy(x => x.Fecha).ToList();
//Se hace un analicis inicial, de los datos globales.
Console.WriteLine($"Se analizó el archivo '{filePath}', con {decimal.Parse(linea.ToString()).ToString("N0")} líneas. {Environment.NewLine}De todos los ficheros auditados se encontraros {list.Count} archivos que pueden ser ejecutables({string.Join(',',Procesar.extensionesEjecutables)}).\n\nHe aquí un deglose detallado.");
//Separamos un arreglo para exportarlos a excel
Exportar.ToExcel(list, $"{exportTo}FicherosDepurados");
//Iniciamos el procesamiento global de los ficheros encontrados, para mostrar las estadisticas.
Procesar.Files(list);
