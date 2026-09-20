using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace GestionPedidos
{
    public partial class FormProducto : Form
    {
        public FormProducto()
        {
            InitializeComponent();
            button1.Click += GuardarProducto;
            button2.Click += (_, _) => Close();
        }

        private void GuardarProducto(object? sender, EventArgs e)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(textBox1.Text) || string.IsNullOrWhiteSpace(textBox2.Text))
                    throw new InvalidOperationException("El código y la descripción son obligatorios.");
                if (!decimal.TryParse(textBox3.Text, out var precio) || precio < 0)
                    throw new InvalidOperationException("Ingrese un precio válido mayor o igual a cero.");

                DatosAplicacion.Gestor.Catalogo.AgregarProducto(new LibreriaClases.Producto(textBox1.Text.Trim(), textBox2.Text.Trim(), precio));
                MessageBox.Show("Producto registrado correctamente.", "Gestor de pedidos", MessageBoxButtons.OK, MessageBoxIcon.Information);
                textBox1.Clear(); textBox2.Clear(); textBox3.Clear(); txtCantidad.Clear(); textBox1.Focus();
            }
            catch (Exception ex) { MessageBox.Show(ex.Message, "No se pudo guardar", MessageBoxButtons.OK, MessageBoxIcon.Warning); }
        }
    }
}
