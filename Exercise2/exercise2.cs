using System;
using System.Collections.Generic;

List<Estudiante> estudiantes = [];
string continuar;

do
{
    Console.WriteLine("\nCOLEGIO DIOS ES BUENO");
    Console.WriteLine("CALIFICACIONES DEL CUATRIMESTRE\n");

    Console.Write("Nombre: ");
    string nombre = Console.ReadLine() ?? "";

    Console.Write("Apellido: ");
    string apellido = Console.ReadLine() ?? "";

    Console.Write("Nota 1: ");
    double nota1 = Convert.ToDouble(Console.ReadLine());

    Console.Write("Nota 2: ");
    double nota2 = Convert.ToDouble(Console.ReadLine());

    Console.Write("Nota 3: ");
    double nota3 = Convert.ToDouble(Console.ReadLine());

    Console.Write("Nota 4: ");
    double nota4 = Convert.ToDouble(Console.ReadLine());

    estudiantes.Add(new Estudiante
    {
        Nombre = nombre,
        Apellido = apellido,
        Nota1 = nota1,
        Nota2 = nota2,
        Nota3 = nota3,
        Nota4 = nota4
    });

    Console.Write("\n¿Desea ingresar otro estudiante? (s/n): ");
    continuar = (Console.ReadLine() ?? "").ToLower();

} while (continuar == "s");


Console.WriteLine("\n");
Console.WriteLine(
    $"{"Nombre",-15}" +
    $"{"Apellido",-15}" +
    $"{"Nota1",-10}" +
    $"{"Nota2",-10}" +
    $"{"Nota3",-10}" +
    $"{"Nota4",-10}" +
    $"{"Promedio",-12}" +
    $"{"Literal",-10}"
);

Console.WriteLine(new string('=', 87));

foreach (Estudiante estudiante in estudiantes)
{
    Console.WriteLine(
        $"{estudiante.Nombre,-15}" +
        $"{estudiante.Apellido,-15}" +
        $"{estudiante.Nota1,-10}" +
        $"{estudiante.Nota2,-10}" +
        $"{estudiante.Nota3,-10}" +
        $"{estudiante.Nota4,-10}" +
        $"{estudiante.Promedio(),-12:F2}" +
        $"{estudiante.Literal(),-10}"
    );
}


class Estudiante
{
    public required string Nombre { get; set; }
    public required string Apellido { get; set; }
    public double Nota1 { get; set; }
    public double Nota2 { get; set; }
    public double Nota3 { get; set; }
    public double Nota4 { get; set; }

    public double Promedio()
    {
        return (Nota1 + Nota2 + Nota3 + Nota4) / 4;
    }

    public string Literal()
    {
        double promedio = Promedio();

        if (promedio >= 90)
            return "A";
        else if (promedio >= 80)
            return "B";
        else if (promedio >= 70)
            return "C";
        else
            return "D";
    }
}