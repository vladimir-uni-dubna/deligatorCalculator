using System;
using System.Windows;
using System.Windows.Controls;

namespace deligatorCalculator
{
    public partial class MainWindow : Window
    {
        private Calculator calculator;
        private double firstOperand = 0;
        private string currentOperation = "";
        private bool isNewEntry = true;

        public MainWindow()
        {
            InitializeComponent();
            
            // Создаём калькулятор и подписываемся на событие
            calculator = new Calculator();
            calculator.CalculationPerformed += OnCalculationPerformed;
        }

        // Обработчик события - вызывается после каждого вычисления
        private void OnCalculationPerformed(object sender, CalculationEventArgs e)
        {
            // Событие сработало - можно добавить логику (например, логирование)
            Console.WriteLine($"[Событие] {e.Operation}: {e.Operand1} и {e.Operand2} = {e.Result}");
        }

        // Обработчик кнопок с цифрами
        private void NumberButton_Click(object sender, RoutedEventArgs e)
        {
            Button button = (Button)sender;
            string number = button.Content.ToString();

            if (isNewEntry)
            {
                txtDisplay.Text = number;
                isNewEntry = false;
            }
            else
            {
                if (txtDisplay.Text == "0")
                    txtDisplay.Text = number;
                else
                    txtDisplay.Text += number;
            }
        }

        // Обработчик кнопки десятичной точки
        private void DecimalButton_Click(object sender, RoutedEventArgs e)
        {
            if (isNewEntry)
            {
                txtDisplay.Text = "0.";
                isNewEntry = false;
            }
            else if (!txtDisplay.Text.Contains("."))
            {
                txtDisplay.Text += ".";
            }
        }

        // Обработчик кнопок операций
        private void OperationButton_Click(object sender, RoutedEventArgs e)
        {
            Button button = (Button)sender;
            string operation = button.Content.ToString();

            if (!string.IsNullOrEmpty(currentOperation) && !isNewEntry)
            {
                // Если уже есть операция, сначала вычисляем результат
                EqualsButton_Click(sender, e);
            }

            firstOperand = double.Parse(txtDisplay.Text);
            currentOperation = operation;
            isNewEntry = true;
        }

        // Обработчик кнопки равно
        private void EqualsButton_Click(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrEmpty(currentOperation))
                return;

            try
            {
                double secondOperand = double.Parse(txtDisplay.Text);
                double result = 0;
                string operationName = "";

                // Выбираем делегат в зависимости от операции
                MathOperation operation = null;

                switch (currentOperation)
                {
                    case "+":
                        operation = Calculator.Add;
                        operationName = "Сложение";
                        break;
                    case "-":
                        operation = Calculator.Subtract;
                        operationName = "Вычитание";
                        break;
                    case "×":
                        operation = Calculator.Multiply;
                        operationName = "Умножение";
                        break;
                    case "÷":
                        operation = Calculator.Divide;
                        operationName = "Деление";
                        break;
                }

                if (operation != null)
                {
                    // Выполняем операцию через делегат - здесь сработает событие
                    result = calculator.Execute(operation, firstOperand, secondOperand, operationName);
                    txtDisplay.Text = result.ToString();
                }

                currentOperation = "";
                isNewEntry = true;
            }
            catch (DivideByZeroException)
            {
                MessageBox.Show("Деление на ноль!", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                ClearButton_Click(sender, e);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                ClearButton_Click(sender, e);
            }
        }

        // Обработчик кнопки очистки
        private void ClearButton_Click(object sender, RoutedEventArgs e)
        {
            txtDisplay.Text = "0";
            firstOperand = 0;
            currentOperation = "";
            isNewEntry = true;
        }
    }
}
