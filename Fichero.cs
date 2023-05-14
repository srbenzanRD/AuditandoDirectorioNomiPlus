using static System.Net.Mime.MediaTypeNames;

namespace auditoria;
public record Fichero(int linea,DateOnly Fecha, string Archivo,string LineaOriginal)
{
    public bool EsEjecutable => Procesar.IsExecute(LineaOriginal);
    public string Type => Procesar.GetExtention(LineaOriginal);

}
public class Functions
{
    public static void PrintHeader(string header)
    {
        int totalWidth = 60;  // Ancho total del encabezado y el separador
        int headerWidth = header.Length;
        int padding = (totalWidth - headerWidth) / 2;

        Console.WriteLine(new string('-', totalWidth));
        Console.WriteLine("{0}{1}{0}", new string(' ', padding), header);
        Console.WriteLine(new string('-', totalWidth));
    }

}

public class Procesar
{
    public static List<string> extensionesEjecutables = new List<string>()
        {
                ".exe",     // Ejecutable de Windows
                ".msi",     // Instalador de Windows
                ".bat",     // Archivo por lotes de Windows
                ".cmd",     // Archivo de comando de Windows
                ".com",     // Archivo ejecutable de MS-DOS
                ".scr"      // Protector de pantalla
        };
    public static bool IsExecute(string file)
    {

        var isExecuteble = false;
        foreach (var _extention in extensionesEjecutables)
        {
            if (file.ToLower().Contains(_extention)) return true;
        }
        return isExecuteble;
    }
    public static string GetExtention(string file)
    {

        var isExecuteble = "";
        foreach (var _extention in extensionesEjecutables)
        {
            if (file.ToLower().Contains(_extention)) return _extention;
        }
        return isExecuteble;
    }
    public static void PrintByType(List<Fichero> ficheros)
    {
        Console.WriteLine("EJECUTABLES POR TIPO (EXTENCIÓN)");
        extensionesEjecutables.ForEach(ext =>
        {
            var count = ficheros.Where(f => f.Type == ext).Count();
            Console.WriteLine($"\t{ext} ({count})");
        });
    }
    public static void PrintByYear(List<Fichero> ficheros)
    {
        Console.WriteLine("EJECUTABLES POR AÑO");

        var anios = ficheros.Select(f => f.Fecha.Year).Distinct().OrderDescending().ToList();
        anios.ForEach(anio =>
        {
            var ficheros_del_anio = ficheros
            .Where(f => f.Fecha.Year == anio)
            .ToList();
            if (ficheros_del_anio.Any())
            {
                Console.WriteLine($"\t{anio} ({ficheros_del_anio.Count})");
            }
        });
    }
    public static void Files(List<Fichero> ficheros)
    {
        
        Functions.PrintHeader("ARCHIVOS POR AÑO Y TIPO");
        var anios = ficheros.Select(f => f.Fecha.Year).Distinct().OrderDescending().ToList();
        extensionesEjecutables.ForEach(ext =>
        {
            var year_print = 0;
            Console.WriteLine($"{ext.ToUpper()}");
            anios.ForEach(anio =>
            {
                var ficheros_del_anio = ficheros
                .Where(f => f.Type == ext && f.Fecha.Year == anio)
                .ToList();
                if (ficheros_del_anio.Any())
                {
                    Console.WriteLine($"\t{anio} ({ficheros_del_anio.Count})");
                    ficheros_del_anio.ForEach(fa =>
                    {
                        Console.WriteLine($"\t\t {fa.Fecha.ToString("dd-MM-yyyy")} -> {fa.Archivo}");
                    });
                    year_print++;
                }
            });
            if (year_print == 0) Console.WriteLine("\tN/A");
        });
        Functions.PrintHeader("TOTALES");
        PrintByType(ficheros);
        PrintByYear(ficheros);
        Console.WriteLine($"Para un total de {ficheros.Count} archivos ejecutables");
    }
}
