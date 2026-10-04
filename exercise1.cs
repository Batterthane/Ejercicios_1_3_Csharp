using System;

class Program
{
    static void Main()
    {
        Console.WriteLine("\nOPERACIONES CON DOS VALORES\n");

        Console.Write("Ingrese el primer valor: ");
        double valor1 = Convert.ToDouble(Console.ReadLine());

        Console.Write("Ingrese el segundo valor: ");
        double valor2 = Convert.ToDouble(Console.ReadLine());

        Console.WriteLine("\nResultados:");
        Console.WriteLine($"Suma: {valor1 + valor2}");
        Console.WriteLine($"Resta: {valor1 - valor2}");
        Console.WriteLine($"Multiplicación: {valor1 * valor2}");

        if (valor2 != 0)
            Console.WriteLine($"División: {valor1 / valor2}");
        else
            Console.WriteLine("División: No se puede dividir entre cero.");

        if (valor1 >= 0)
            Console.WriteLine($"Raíz cuadrada del primer valor: {Math.Sqrt(valor1)}");
        else
            Console.WriteLine("Raíz cuadrada del primer valor: No existe en los números reales.");

        if (valor2 >= 0)
            Console.WriteLine($"Raíz cuadrada del segundo valor: {Math.Sqrt(valor2)}");
        else
            Console.WriteLine("Raíz cuadrada del segundo valor: No existe en los números reales.");
    }
}
