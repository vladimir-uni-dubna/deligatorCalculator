using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace deligatorCalculator
{
    public partial class Form1 : Form
    {
        private Calculator calculator;

        public Form1()
        {
            InitializeComponent();
            
            // Создаём калькулятор и подписываемся на событие
            calculator = new Calculator();
            calculator.CalculationPerformed += OnCalculationPerformed;
        }

        // Обработчик события - выводит результат в лог
        private void OnCalculationPerformed(object sender, CalculationEventArgs e)
        {
            string logMessage = $"[{DateTime.Now:HH:mm:ss}] {e.Operation}: {e.Operand1} и {e.Operand2} = {e.Result}";
            listBoxLog.Items.Add(logMessage);
            listBoxLog.SelectedIndex = listBoxLog.Items.Count - 1;
        }

        // Обработчики кнопок операций
        private void btnAdd_Click(object sender, EventArgs e)
        {
            PerformOperation(Calculator.Add, "Сложение");
        }

        private void btnSubtract_Click(object sender, EventArgs e)
        {
            PerformOperation(Calculator.Subtract, "Вычитание");
        }

        private void btnMultiply_Click(object sender, EventArgs e)
        {
            PerformOperation(Calculator.Multiply, "Умножение");
        }

        private void btnDivide_Click(object sender, EventArgs e)
        {
            PerformOperation(Calculator.Divide, "Деление");
        }

        // Общий метод для выполнения операции
        private void PerformOperation(MathOperation operation, string operationName)
        {
            try
            {
                double a = double.Parse(txtOperand1.Text);
                double b = double.Parse(txtOperand2.Text);
                
                double result = calculator.Execute(operation, a, b, operationName);
                
                lblResult.Text = result.ToString();
            }
            catch (FormatException)
            {
                MessageBox.Show("Введите корректные числа!", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            catch (DivideByZeroException ex)
            {
                MessageBox.Show(ex.Message, "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnClear_Click(object sender, EventArgs e)
        {
            txtOperand1.Clear();
            txtOperand2.Clear();
            lblResult.Text = "0";
        }

        private void btnClearLog_Click(object sender, EventArgs e)
        {
            listBoxLog.Items.Clear();
        }
    }
}
