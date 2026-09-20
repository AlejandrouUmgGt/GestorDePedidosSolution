using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace GestionPedidos
{
    public partial class FormProveedores : Form
    {
        public FormProveedores()
        {
            InitializeComponent();
            button1.Click += GuardarProveedor;
            button2.Click += (_, _) => Close();
        }

        private void GuardarProveedor(object? sender, EventArgs e)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(textBox1.Text) || string.IsNullOrWhiteSpace(textBox4.Text))
                    throw new InvalidOperationException("El CUI y el nombre comercial son obligatorios.");

                DatosAplicacion.Gestor.AgregarProveedor(new LibreriaClases.Proveedor(
                    textBox1.Text.Trim(), textBox2.Text.Trim(), textBox3.Text.Trim(), textBox6.Text.Trim(),
                    textBox5.Text.Trim(), textBox4.Text.Trim(), textBox7.Text.Trim(), textBox8.Text.Trim()));
                MessageBox.Show("Proveedor registrado correctamente.", "Gestor de pedidos", MessageBoxButtons.OK, MessageBoxIcon.Information);
                foreach (var control in Controls.OfType<TextBox>()) control.Clear();
                textBox1.Focus();
            }
            catch (Exception ex) { MessageBox.Show(ex.Message, "No se pudo guardar", MessageBoxButtons.OK, MessageBoxIcon.Warning); }
        }
    }
}
