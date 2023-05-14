using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OfficeOpenXml;
using System.IO;


namespace auditoria;

public class Exportar
{
    public static void ToExcel(List<Fichero> list,string to)
    {
        var ficheros_depurados_para_exportar_a_excel = list.Select(f => new {
            Linea = f.linea,
            f.Fecha,
            Archivo = f.LineaOriginal,
            Tipo = f.Type
        }).ToList();
        // Crear un nuevo archivo de Excel
        var nombreArchivo = $"{to}.xlsx";
        FileInfo archivo = new FileInfo(nombreArchivo);
        ExcelPackage.LicenseContext = LicenseContext.NonCommercial;
        using (ExcelPackage package = new ExcelPackage(archivo))
        {
            // Crear una nueva hoja de cálculo
            ExcelWorksheet worksheet = package.Workbook.Worksheets.Add("Datos");

            // Escribir encabezados de columnas
            worksheet.Cells[1, 1].Value = "Linea";
            worksheet.Cells[1, 2].Value = "Fecha";
            worksheet.Cells[1, 3].Value = "Archivo";
            worksheet.Cells[1, 4].Value = "Tipo";

            // Escribir los datos de la lista en las filas siguientes
            int fila = 2;
            foreach (var item in ficheros_depurados_para_exportar_a_excel)
            {
                worksheet.Cells[fila, 1].Value = item.Linea;
                worksheet.Cells[fila, 2].Value = item.Fecha;
                worksheet.Cells[fila, 3].Value = item.Archivo;
                worksheet.Cells[fila, 4].Value = item.Tipo;
                fila++;
            }

            // Ajustar el ancho de las columnas automáticamente
            worksheet.Cells.AutoFitColumns(0);

            // Guardar el archivo de Excel
            package.Save();
        }

    }
}
