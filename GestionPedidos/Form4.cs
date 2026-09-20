using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace GestionPedidos
{
    public partial class FormPedido : Form
    {
        private readonly List<LibreriaClases.DetallePedido> detalles = new();

        public FormPedido()
        {
            InitializeComponent();
            button1.Click += AgregarDetalle;
            button2.Click += FinalizarPedido;
            textBox1.Leave += (_, _) => MostrarProveedor();
            ConfigurarGrid();
        }

        private void MostrarProveedor()
        {
            var proveedor = int.TryParse(textBox1.Text, out var numero)
                ? DatosAplicacion.Gestor.BuscarProveedorPorNumero(numero)
                : null;
            textBox2.Text = proveedor?.NombreComercial ?? string.Empty;
        }

        private void AgregarDetalle(object? sender, EventArgs e)
        {
            try
            {
                var producto = DatosAplicacion.Gestor.Catalogo.BuscarPorCodigo(textBox3.Text.Trim())
                    ?? throw new InvalidOperationException("No existe un producto con ese código.");
                if (!decimal.TryParse(textBox4.Text, out var cantidad) || cantidad <= 0)
                    throw new InvalidOperationException("Ingrese una cantidad válida mayor que cero.");
                detalles.Add(new LibreriaClases.DetallePedido(producto, cantidad));
                ActualizarDetalle(); textBox3.Clear(); textBox4.Clear(); textBox3.Focus();
            }
            catch (Exception ex) { MessageBox.Show(ex.Message, "No se pudo agregar", MessageBoxButtons.OK, MessageBoxIcon.Warning); }
        }

        private void FinalizarPedido(object? sender, EventArgs e)
        {
            try
            {
                if (!int.TryParse(textBox1.Text, out var numeroProveedor))
                    throw new InvalidOperationException("Ingrese un número de proveedor válido.");
                var proveedor = DatosAplicacion.Gestor.BuscarProveedorPorNumero(numeroProveedor)
                    ?? throw new InvalidOperationException("No existe un proveedor con ese número.");
                if (detalles.Count == 0) throw new InvalidOperationException("Agregue al menos un producto al pedido.");
                var pedido = DatosAplicacion.Gestor.CrearPedido(proveedor, DateOnly.FromDateTime(dateTimePicker1.Value));
                foreach (var detalle in detalles) pedido.AgregarDetalle(detalle);
                MessageBox.Show($"Pedido #{pedido.Numero} guardado. Total: {pedido.Total:C2}", "Pedido confirmado", MessageBoxButtons.OK, MessageBoxIcon.Information);
                detalles.Clear(); textBox1.Clear(); textBox2.Clear(); ActualizarDetalle();
            }
            catch (Exception ex) { MessageBox.Show(ex.Message, "No se pudo finalizar", MessageBoxButtons.OK, MessageBoxIcon.Warning); }
        }

        private void ConfigurarGrid()
        {
            dgvDetalle.ReadOnly = true; dgvDetalle.AllowUserToAddRows = false; dgvDetalle.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
        }

        private void ActualizarDetalle() => dgvDetalle.DataSource = detalles.Select(d => new { Código = d.Producto.Codigo, Producto = d.Producto.Descripcion, d.Cantidad, Precio = d.Producto.Precio.ToString("C2"), Total = d.Total.ToString("C2") }).ToList();

        private void button1_Click(object sender, EventArgs e)
        {

        }

        private void button2_Click(object sender, EventArgs e)
        {

        }
    }
}
