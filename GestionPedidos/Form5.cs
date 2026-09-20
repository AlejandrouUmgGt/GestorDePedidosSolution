using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace GestionPedidos
{
    public partial class Form5 : Form
    {
        public Form5()
        {
            InitializeComponent();
            button1.Click += Buscar;
            textBox1.KeyDown += (_, e) => { if (e.KeyCode == Keys.Enter) Buscar(this, EventArgs.Empty); };
            dataGridView1.ReadOnly = true;
            dataGridView1.AllowUserToAddRows = false;
            dataGridView1.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
        }

        private void Buscar(object? sender, EventArgs e)
        {
            IEnumerable<LibreriaClases.Pedido> resultado;
            if (!string.IsNullOrWhiteSpace(textBox1.Text))
            {
                if (!int.TryParse(textBox1.Text, out var numero)) { MessageBox.Show("Ingrese un número de pedido válido."); return; }
                var pedido = DatosAplicacion.Gestor.BuscarPedidoPorNumero(numero);
                resultado = pedido is null ? [] : [pedido];
            }
            else
            {
                var desde = DateOnly.FromDateTime(dateTimePicker1.Value.Date);
                var hasta = DateOnly.FromDateTime(dateTimePicker2.Value.Date);
                if (desde > hasta) { MessageBox.Show("La fecha inicial no puede ser mayor que la fecha final."); return; }
                resultado = DatosAplicacion.Gestor.Pedidos.Where(p => p.Fecha >= desde && p.Fecha <= hasta);
            }
            dataGridView1.DataSource = resultado.Select(p => new { Número = p.Numero, Fecha = p.Fecha.ToString("dd/MM/yyyy"), Proveedor = p.Proveedor.NombreComercial, Líneas = p.Detalles.Count, Total = p.Total.ToString("C2") }).ToList();
        }
    }
}
