// See https://aka.ms/new-console-template for more information
using auditoria;
using System.Globalization;
using System.Runtime.CompilerServices;
//Reemplazar por la ruta de archivo original.
string filePath = @"C:\auditoria\Resources\directorioNomiplus.txt";

string[] content = File.ReadAllLines(filePath);
List<Fichero> list = new();
CultureInfo culture = new("en-US");
var linea = 0;

foreach (string line in content)
{
    linea++;
    string[] row = line.Split(' ');
    if (row[0] != null && row[0] != string.Empty)
    {
        DateTime tempDate = Convert.ToDateTime(row[0], culture);

        string fileName = row[^1];

        if (Procesar.IsExecute(fileName) || Procesar.IsExecute(line))
        {
            list.Add(new Fichero(linea,DateOnly.FromDateTime(tempDate), fileName,line));
        }
    }
}

list = list.OrderBy(x => x.Fecha).ToList();
Console.WriteLine($"Se analizó el archivo '{filePath}', con {decimal.Parse(linea.ToString()).ToString("N0")} líneas. {Environment.NewLine}De todos los ficheros auditados se encontraros {list.Count} archivos que pueden ser ejecutables({string.Join(',',Procesar.extensionesEjecutables)}).\n\nHe aquí un deglose detallado.");
Procesar.Files(list);
