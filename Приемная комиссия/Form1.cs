﻿using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;
using Приемная_комиссия.Haracteristika;
using Приемная_комиссия.Genderi;


namespace Приемная_комиссия
{
    public partial class Form1 : Form
    {

        private readonly List<Abiturient> abiturients;
        private readonly BindingSource bindingSource;
        public Form1()
        {
            InitializeComponent();
            dataGridView1.AutoGenerateColumns = false;
            abiturients = new List<Abiturient>();
            bindingSource = new BindingSource();
            bindingSource.DataSource = abiturients;
            dataGridView1.DataSource = bindingSource;
        }

        private void dataGridView1_SelectionChanged(object sender, EventArgs e)
        {
            Change.Enabled = Delete.Enabled = dataGridView1.SelectedRows.Count > 0;
            Change2.Enabled = Delete2.Enabled = dataGridView1.SelectedRows.Count > 0;
        }

        private void dataGridView1_CellFormatting(object sender, DataGridViewCellFormattingEventArgs e)
        {
            var abitur = (Abiturient)dataGridView1.Rows[e.RowIndex].DataBoundItem;

            if (dataGridView1.Columns[e.ColumnIndex].Name == "Gender_Column")
            {
                var val = (Gender)e.Value;
                switch (val)
                {
                    case Gender.Male:
                        e.Value = "Мужской";
                        break;
                    case Gender.Female:
                        e.Value = "Женский";
                        break;
                }
            }

                if (dataGridView1.Columns[e.ColumnIndex].Name == "FormaObucheniya_Column")
                {
                    var vall = (FormaObucheniya)e.Value;
                    switch (vall)
                    {
                        case (FormaObucheniya.Ochnoe):
                            e.Value = "Очное";
                            break;
                        case (FormaObucheniya.Ocno_zaochnoe):
                            e.Value = "Очно-заочное";
                            break;
                        case (FormaObucheniya.Zaochnoe):
                            e.Value = "Заочное";
                            break;
                    }
                }

            if (dataGridView1.Columns[e.ColumnIndex].Name == "Sum_Column")
            {
                e.Value = Math.Round(abitur.Matem + abitur.Rus + abitur.Inf); 
            }
        }

        private void Exit_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void About_Program_Click(object sender, EventArgs e)
        {
            MessageBox.Show("Разработчик: Коршикова Эльвина", "Программа",
                            MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        private void Add_Click(object sender, EventArgs e)
        {
            var infoform = new Abiturientinfo_form();
            infoform.Text = "Добавить aбитуриента";
            if (infoform.ShowDialog(this) == DialogResult.OK) 
            {
                infoform.Abiturient.Id = Guid.NewGuid();
                abiturients.Add(infoform.Abiturient);
                bindingSource.ResetBindings(false);
                CalculateStatus();
            }
        }

        private void Delete_Click(object sender, EventArgs e)
        {
            var data = (Abiturient)dataGridView1.Rows[dataGridView1.SelectedRows[0].Index].DataBoundItem;
            if (MessageBox.Show($"Вы действительно желаете удалить '{data.FullName}'?",
                "Удаление записи",
                MessageBoxButtons.YesNo) == DialogResult.Yes)
            {
                abiturients.Remove(data);
                bindingSource.ResetBindings(false);
                CalculateStatus();
            }
        }

        private void Change_Click(object sender, EventArgs e)
        {
            var data = (Abiturient)dataGridView1.Rows[dataGridView1.SelectedRows[0].Index].DataBoundItem;
            var infoForm = new Abiturientinfo_form(data);
            infoForm.Text = "Редактирование абитуриента";
            if (infoForm.ShowDialog(this) == DialogResult.OK)
            {
                data.FullName = infoForm.Abiturient.FullName;
                data.Gender = infoForm.Abiturient.Gender;
                data.Birthday = infoForm.Abiturient.Birthday;
                data.FormaObucheniya = infoForm.Abiturient.FormaObucheniya;
                data.Matem = infoForm.Abiturient.Matem;
                data.Rus = infoForm.Abiturient.Rus;
                data.Inf = infoForm.Abiturient.Inf;
                data.Sum = infoForm.Abiturient.Sum;
                bindingSource.ResetBindings(false);
                CalculateStatus();
            }
        }

        public void CalculateStatus()
        {
            var count = abiturients.Count;
            lblstatus.Text = $"Всего абитуриентов: " + count.ToString();

            var Kolichestvo = 0;
            foreach (var abitur in abiturients)
             {
                if ((abitur.Matem + abitur.Rus + abitur.Inf) > 150)
                {
                    Kolichestvo++;
                }  
            }
            lbl150.Text = $"Студенты, набравшие больше 150 баллов: " + Kolichestvo;
        }

        private void Change2_Click(object sender, EventArgs e)
        {
            Change.PerformClick();
        }
    }
}
