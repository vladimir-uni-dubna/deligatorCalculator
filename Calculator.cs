using System;

namespace deligatorCalculator
{
    // Делегат для математических операций
    public delegate double MathOperation(double a, double b);

    // Делегат для события
    public delegate void CalculationEventHandler(object sender, CalculationEventArgs e);

    // Аргументы события
    public class CalculationEventArgs : EventArgs
    {
        public double Operand1 { get; set; }
        public double Operand2 { get; set; }
        public double Result { get; set; }
        public string Operation { get; set; }

        public CalculationEventArgs(double op1, double op2, double result, string operation)
        {
            Operand1 = op1;
            Operand2 = op2;
            Result = result;
            Operation = operation;
        }
    }

    public class Calculator
    {
        // Событие, которое срабатывает после каждого вычисления
        public event CalculationEventHandler CalculationPerformed;

        // Метод для вызова события
        protected virtual void OnCalculationPerformed(CalculationEventArgs e)
        {
            CalculationPerformed?.Invoke(this, e);
        }

        // Выполнение операции через делегат
        public double Execute(MathOperation operation, double a, double b, string operationName)
        {
            double result = operation(a, b);
            
            // Вызываем событие после вычисления
            OnCalculationPerformed(new CalculationEventArgs(a, b, result, operationName));
            
            return result;
        }

        // Готовые операции
        public static double Add(double a, double b) => a + b;
        public static double Subtract(double a, double b) => a - b;
        public static double Multiply(double a, double b) => a * b;
        public static double Divide(double a, double b)
        {
            if (b == 0)
                throw new DivideByZeroException("Деление на ноль!");
            return a / b;
        }
    }
}
