namespace GestionPedidos
{
    public partial class FormInicio : Form
    {
        public FormInicio()
        {
            InitializeComponent();
            button1.Click += (_, _) => new FormProveedores().ShowDialog(this);
            button2.Click += (_, _) => new FormProducto().ShowDialog(this);
            button3.Click += (_, _) => new FormPedido().ShowDialog(this);
            button4.Click += (_, _) => new Form5().ShowDialog(this);
        }
    }
}
