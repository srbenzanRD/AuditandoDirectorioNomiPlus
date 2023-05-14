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
    
    if(linea == 47316)
    {
        Console.WriteLine(line);
    }
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

Procesar.Files(list);
