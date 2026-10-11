using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;

Console.OutputEncoding = System.Text.Encoding.UTF8;


string carpeta = Path.Combine(Directory.GetCurrentDirectory(), "Cursos");
Directory.CreateDirectory(carpeta);

Console.WriteLine("COLEGIO DIOS ES BUENO");

Curso curso = SeleccionarCurso(carpeta);

Console.WriteLine($"\nCurso seleccionado: {curso.Nombre}");
Console.WriteLine($"Los datos se guardan en: {curso.Ruta}");

bool salir = false;

while (!salir)
{
    MostrarMenu(curso);
    string opcion = (Console.ReadLine() ?? "").Trim();

    switch (opcion)
    {
        case "1":
            AgregarEstudiante(curso);
            break;

        case "2":
            EliminarEstudiante(curso);
            break;

        case "3":
            MostrarReportes(curso);
            break;

        case "4":
            salir = true;
            Console.WriteLine("\nGracias por usar el sistema. ¡Hasta luego!");
            break;

        default:
            Console.WriteLine("\nOpción no válida. Elija un número del 1 al 4.");
            Pausar();
            break;
    }
}



static Curso SeleccionarCurso(string carpeta)
{
    string[] archivos = Directory.GetFiles(carpeta, "*.txt")
        .OrderBy(archivo => archivo)
        .ToArray();

    if (archivos.Length > 0)
    {
        Console.WriteLine("\nCursos disponibles:");

        for (int i = 0; i < archivos.Length; i++)
            Console.WriteLine($"{i + 1}. {Path.GetFileNameWithoutExtension(archivos[i])}");

        Console.WriteLine("0. Crear un curso nuevo");

        while (true)
        {
            Console.Write("\nSeleccione una opción: ");

            if (int.TryParse(Console.ReadLine(), out int numero) &&
                numero >= 0 && numero <= archivos.Length)
            {
                if (numero > 0)
                {
                    string ruta = archivos[numero - 1];

                    Curso existente = new()
                    {
                        Nombre = Path.GetFileNameWithoutExtension(ruta),
                        Ruta = ruta
                    };
                    existente.Cargar();
                    return existente;
                }

                break;
            }

            Console.WriteLine("Opción no válida.");
        }
    }

    string nombre = LeerTexto("\nNombre del curso: ");

    foreach (char invalido in Path.GetInvalidFileNameChars())
        nombre = nombre.Replace(invalido.ToString(), "");

    nombre = nombre.Trim();
    if (nombre == "")
        nombre = "Curso sin nombre";

    Curso nuevo = new()
    {
        Nombre = nombre,
        Ruta = Path.Combine(carpeta, nombre + ".txt")
    };

    nuevo.Cargar();
    nuevo.Guardar();
    return nuevo;
}


static void MostrarMenu(Curso curso)
{
    Console.WriteLine("\n==============================");
    Console.WriteLine("COLEGIO DIOS ES BUENO");
    Console.WriteLine($"Curso: {curso.Nombre}");
    Console.WriteLine($"Estudiantes inscritos: {curso.Estudiantes.Count}");
    Console.WriteLine("==============================");
    Console.WriteLine("1. Agregar un estudiante");
    Console.WriteLine("2. Eliminar un estudiante");
    Console.WriteLine("3. Reportes");
    Console.WriteLine("4. Salir");
    Console.Write("\nSeleccione una opción: ");
}

static void AgregarEstudiante(Curso curso)
{
    string continuar;

    do
    {
        Console.WriteLine("\nAGREGAR ESTUDIANTE");
        Console.WriteLine($"Curso: {curso.Nombre}\n");

        string nombre = LeerTexto("Nombre: ");
        string apellido = LeerTexto("Apellido: ");
        double nota1 = LeerNota("Nota 1: ");
        double nota2 = LeerNota("Nota 2: ");
        double nota3 = LeerNota("Nota 3: ");
        double nota4 = LeerNota("Nota 4: ");

        curso.Estudiantes.Add(new Estudiante
        {
            Nombre = nombre,
            Apellido = apellido,
            Nota1 = nota1,
            Nota2 = nota2,
            Nota3 = nota3,
            Nota4 = nota4
        });

        curso.Guardar();

        Console.WriteLine($"\nEstudiante {nombre} {apellido} agregado y guardado en el archivo.");

        Console.Write("\n¿Desea ingresar otro estudiante? (s/n): ");
        continuar = (Console.ReadLine() ?? "").Trim().ToLower();

    } while (continuar == "s");
}

static void EliminarEstudiante(Curso curso)
{
    Console.WriteLine("\nELIMINAR ESTUDIANTE");
    Console.WriteLine($"Curso: {curso.Nombre}\n");

    if (curso.Estudiantes.Count == 0)
    {
        Console.WriteLine("No hay estudiantes en el curso.");
        Pausar();
        return;
    }

    for (int i = 0; i < curso.Estudiantes.Count; i++)
    {
        Estudiante e = curso.Estudiantes[i];
        Console.WriteLine($"{i + 1}. {e.Nombre} {e.Apellido}");
    }

    Console.Write("\nNúmero del estudiante a eliminar (0 para cancelar): ");

    if (!int.TryParse(Console.ReadLine(), out int numero) ||
        numero < 0 ||
        numero > curso.Estudiantes.Count)
    {
        Console.WriteLine("Número no válido.");
        Pausar();
        return;
    }

    if (numero == 0)
    {
        Console.WriteLine("Operación cancelada.");
        Pausar();
        return;
    }

    Estudiante estudiante = curso.Estudiantes[numero - 1];

    Console.Write($"¿Seguro que desea eliminar a {estudiante.Nombre} {estudiante.Apellido}? (s/n): ");
    string confirmar = (Console.ReadLine() ?? "").Trim().ToLower();

    if (confirmar == "s")
    {
        curso.Estudiantes.RemoveAt(numero - 1);
        curso.Guardar();
        Console.WriteLine("Estudiante eliminado correctamente.");
    }
    else
    {
        Console.WriteLine("Operación cancelada.");
    }

    Pausar();
}

static void MostrarReportes(Curso curso)
{
    curso.Cargar();

    Console.WriteLine("\n");
    Console.WriteLine("COLEGIO DIOS ES BUENO");
    Console.WriteLine($"CALIFICACIONES DEL CUATRIMESTRE - {curso.Nombre}");
    Console.WriteLine();

    if (curso.Estudiantes.Count == 0)
    {
        Console.WriteLine("No hay estudiantes registrados en este curso.");
        Pausar();
        return;
    }

    List<Estudiante> ordenados = curso.Estudiantes
        .OrderBy(estudiante => estudiante.Literal())
        .ThenBy(estudiante => estudiante.Apellido)
        .ToList();

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

    Console.WriteLine(new string('=', 92));

    foreach (Estudiante estudiante in ordenados)
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

    int estudiantesA = curso.Estudiantes.Count(estudiante => estudiante.Literal() == "A");
    int estudiantesB = curso.Estudiantes.Count(estudiante => estudiante.Literal() == "B");
    int estudiantesC = curso.Estudiantes.Count(estudiante => estudiante.Literal() == "C");
    int reprobados = curso.Estudiantes.Count(estudiante => estudiante.Literal() == "D");

    Console.WriteLine();
    Console.WriteLine("TOTALES");
    Console.WriteLine(new string('=', 30));

    Console.WriteLine($"Estudiantes en A: {estudiantesA}");
    Console.WriteLine($"Estudiantes en B: {estudiantesB}");
    Console.WriteLine($"Estudiantes en C: {estudiantesC}");
    Console.WriteLine($"Estudiantes en Reprobados: {reprobados}");
    Console.WriteLine($"Total de estudiantes: {curso.Estudiantes.Count}");

    Pausar();
}


static string LeerTexto(string mensaje)
{
    while (true)
    {
        Console.Write(mensaje);

        string texto = (Console.ReadLine() ?? "").Replace("|", " ").Trim();

        if (texto != "")
            return texto;

        Console.WriteLine("Este campo no puede estar vacío.");
    }
}

static double LeerNota(string mensaje)
{
    while (true)
    {
        Console.Write(mensaje);
        string entrada = (Console.ReadLine() ?? "").Trim().Replace(',', '.');

        if (double.TryParse(entrada, NumberStyles.Float, CultureInfo.InvariantCulture, out double nota)
            && nota >= 0 && nota <= 100)
        {
            return nota;
        }

        Console.WriteLine("Nota no válida. Escriba un número entre 0 y 100.");
    }
}

static void Pausar()
{
    Console.Write("\nPresione Enter para volver al menú...");
    Console.ReadLine();
}


class Curso
{
    public required string Nombre { get; set; }
    public required string Ruta { get; set; }
    public List<Estudiante> Estudiantes { get; } = [];

    public void Cargar()
    {
        Estudiantes.Clear();

        if (!File.Exists(Ruta))
            return;

        foreach (string linea in File.ReadAllLines(Ruta))
        {
            if (string.IsNullOrWhiteSpace(linea))
                continue;

            string[] partes = linea.Split('|');
            if (partes.Length != 6)
                continue;

            double[] notas = new double[4];
            bool valido = true;

            for (int i = 0; i < 4; i++)
            {
                if (!double.TryParse(partes[i + 2], NumberStyles.Float,
                        CultureInfo.InvariantCulture, out notas[i]))
                {
                    valido = false;
                }
            }

            if (!valido)
                continue;

            Estudiantes.Add(new Estudiante
            {
                Nombre = partes[0],
                Apellido = partes[1],
                Nota1 = notas[0],
                Nota2 = notas[1],
                Nota3 = notas[2],
                Nota4 = notas[3]
            });
        }
    }

    public void Guardar()
    {
        try
        {
            IEnumerable<string> lineas = Estudiantes.Select(e => string.Join("|",
                e.Nombre,
                e.Apellido,
                e.Nota1.ToString(CultureInfo.InvariantCulture),
                e.Nota2.ToString(CultureInfo.InvariantCulture),
                e.Nota3.ToString(CultureInfo.InvariantCulture),
                e.Nota4.ToString(CultureInfo.InvariantCulture)));

            File.WriteAllLines(Ruta, lineas);
        }
        catch (IOException ex)
        {
            Console.WriteLine($"No se pudo guardar el archivo: {ex.Message}");
        }
    }
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