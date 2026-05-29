using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace TicketOrderingSystem
{
    public partial class Form1 : Form
    {
        // Начално количество билети
        private int ticketStock = 50;

        // Цени на билетите
        private const decimal PriceStudent = 8.00m;
        private const decimal PriceAdult = 15.00m;
        private const decimal PriceSenior = 10.00m;

        public Form1()
        {
            InitializeComponent();
            UpdateStockDisplay();
            
            // Запълваме избора за тип билет
            comboBoxType.Items.Add("Ученик (8 лв.)");
            comboBoxType.Items.Add("Възрастен (15 лв.)");
            comboBoxType.Items.Add("Пенсионер (10 лв.)");
            comboBoxType.SelectedIndex = 1; // По подразбиране е 'Възрастен'
        }

        private void btnBuy_Click(object sender, EventArgs e)
        {
            // Изчистваме предишната сума при нов опит
            lblTotalPrice.Text = "Обща сума: 0 лв.";

            try
            {
                // Проверка за празен или невалиден вход (FormatException)
                if (string.IsNullOrWhiteSpace(txtTicketCount.Text))
                {
                    throw new FormatException("Моля, въведете брой билети.");
                }

                int count;
                if (!int.TryParse(txtTicketCount.Text, out count))
                {
                    throw new FormatException("Въведеният брой трябва да е цяло число.");
                }

                // Проверка за отрицателно число или нула (ArgumentException)
                if (count <= 0)
                {
                    throw new ArgumentException("Броят на билетите трябва да бъде по-голям от нула.");
                }

                // Проверка дали имаме достатъчно наличност (InvalidOperationException)
                if (count > ticketStock)
                {
                    throw new InvalidOperationException($"Няма достатъчно билети. Налични в момента: {ticketStock}");
                }

                // Изчисляване на цената
                decimal unitPrice = 0;
                switch (comboBoxType.SelectedIndex)
                {
                    case 0: unitPrice = PriceStudent; break;
                    case 1: unitPrice = PriceAdult; break;
                    case 2: unitPrice = PriceSenior; break;
                    default: throw new ArgumentException("Моля, изберете тип билет.");
                }

                decimal total = count * unitPrice;

                // Изваждане от наличността
                ticketStock -= count;

                // Обновяване на UI
                lblTotalPrice.Text = $"Обща сума: {total:F2} лв.";
                UpdateStockDisplay();

                MessageBox.Show($"Успешна покупка на {count} билет(а)!", "Информация", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            catch (FormatException ex)
            {
                // Хващаме грешки при парсване на текста
                MessageBox.Show($"Грешка във формата: {ex.Message}", "Внимание", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
            catch (ArgumentException ex)
            {
                // Хващаме грешни аргументи (като < 0 билета)
                MessageBox.Show($"Невалидни данни: {ex.Message}", "Грешка", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            catch (InvalidOperationException ex)
            {
                // Хващаме логически грешки (няма наличност)
                MessageBox.Show($"Операцията е невъзможна: {ex.Message}", "Липса на наличност", MessageBoxButtons.OK, MessageBoxIcon.Stop);
            }
            catch (Exception ex)
            {
                // Общ catch за всичко останало, което не сме предвидили
                MessageBox.Show($"Възникна неочаквана грешка: {ex.Message}", "Критична грешка", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            finally
            {
                // Този блок се изпълнява винаги, независимо дали е имало грешка или не.
                // Може да се използва за логване или изчистване на ресурси.
                Console.WriteLine("Опит за покупка приключи.");
            }
        }

        private void UpdateStockDisplay()
        {
            lblStock.Text = $"Налични билети: {ticketStock}";
            
            // Ако свършат билетите, деактивираме бутона
            if (ticketStock <= 0)
            {
                btnBuy.Enabled = false;
                lblStock.ForeColor = Color.Red;
                lblStock.Text = "БИЛЕТИТЕ СВЪРШИХА!";
            }
        }
    }
}
