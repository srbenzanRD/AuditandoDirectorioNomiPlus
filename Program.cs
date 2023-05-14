// See https://aka.ms/new-console-template for more information
using auditoria;
using System.Globalization;
//Reemplazar por la ruta de archivo original.
string filePath = @"C:\auditoria\Resources\directorioNomiplus.txt";

string[] content = File.ReadAllLines(filePath);
List<Fichero> list = new();
CultureInfo culture = new("en-US");

foreach (string line in content)
{
    string[] row = line.Split(' ');

    if (row[0] != null && row[0] != string.Empty)
    {
        DateTime tempDate = Convert.ToDateTime(row[0], culture);

        string fileName = row[^1];

        if (Procesar.IsExecute(fileName))
        {
            list.Add(new Fichero(DateOnly.FromDateTime(tempDate), fileName));
        }
    }


}

list = list.OrderBy(x => x.Fecha).ToList();

Procesar.Files(list);
