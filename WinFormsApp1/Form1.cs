using LibreriaClases;
using System.Drawing;

namespace WinFormsApp1;

public partial class Form1 : Form
{
    private readonly GestorPedidos gestor = new();
    private readonly List<DetallePedido> detalle = new();
    private DataGridView proveedores = null!, productos = null!, lineas = null!, historialPedidos = null!, historialDetalle = null!;
    private ComboBox proveedorPedido = null!, productoPedido = null!;
    private NumericUpDown cantidad = null!, descuento = null!;
    private Label resumen = null!, totalPedido = null!;

    public Form1() { InitializeComponent(); Construir(); Actualizar(); }

    private void Construir()
    {
        Text = "Gestor de Pedidos"; MinimumSize = new Size(1050, 680); ClientSize = new Size(1180, 740); BackColor = Color.FromArgb(245, 247, 250); Font = new Font("Segoe UI", 10);
        var cabecera = new Panel { Dock = DockStyle.Top, Height = 82, BackColor = Color.FromArgb(31, 78, 121) };
        cabecera.Controls.Add(new Label { Text = "Gestor de Pedidos", ForeColor = Color.White, Font = new Font("Segoe UI Semibold", 22), AutoSize = true, Location = new Point(26, 12) });
        cabecera.Controls.Add(new Label { Text = "Control de proveedores, productos y órdenes de compra", ForeColor = Color.FromArgb(220, 235, 248), AutoSize = true, Location = new Point(29, 51) }); Controls.Add(cabecera);
        var tabs = new TabControl { Dock = DockStyle.Fill, Padding = new Point(20, 8), Font = new Font("Segoe UI Semibold", 10) };
        tabs.TabPages.Add(Resumen()); tabs.TabPages.Add(Proveedores()); tabs.TabPages.Add(Productos()); tabs.TabPages.Add(Pedidos()); tabs.TabPages.Add(Historial()); Controls.Add(tabs);
    }

    private TabPage Resumen()
    {
        var page = Pagina("Resumen"); resumen = new Label { Dock = DockStyle.Top, Height = 230, Padding = new Padding(35), TextAlign = ContentAlignment.MiddleLeft, BackColor = Color.White, Font = new Font("Segoe UI Semibold", 18), ForeColor = Color.FromArgb(31, 78, 121) };
        page.Controls.Add(resumen); page.Controls.Add(new Label { Dock = DockStyle.Bottom, Height = 100, Padding = new Padding(35, 25, 35, 25), Text = "Registre proveedores y productos. Después podrá crear una orden desde la pestaña Pedidos.", Font = new Font("Segoe UI", 12), ForeColor = Color.DimGray }); return page;
    }

    private TabPage Proveedores()
    {
        var page = Pagina("Proveedores"); var split = Division(); var form = Formulario("Nuevo proveedor");
        var cui = Campo(form, "CUI / Identificación", 48); var nombre = Campo(form, "Nombre", 112); var apellido = Campo(form, "Apellido", 176); var nit = Campo(form, "NIT", 240); var comercial = Campo(form, "Nombre comercial", 304); var direccion = Campo(form, "Dirección", 368);
        var guardar = Boton("Guardar proveedor", 432); guardar.Click += (_, _) => Ejecutar(() => { Obligatorio(cui, "CUI / Identificación"); Obligatorio(comercial, "Nombre comercial"); gestor.AgregarProveedor(new Proveedor(cui.Text.Trim(), nombre.Text.Trim(), apellido.Text.Trim(), direccion.Text.Trim(), nit.Text.Trim(), comercial.Text.Trim(), direccion.Text.Trim(), direccion.Text.Trim())); Limpiar(cui, nombre, apellido, nit, comercial, direccion); Actualizar(); }); form.Controls.Add(guardar); split.Panel1.Controls.Add(form);
        proveedores = Grid(); split.Panel2.Controls.Add(Lista(proveedores, () => { if (Valor(proveedores, "CUI") is string id) gestor.EliminarProveedor(id); Actualizar(); })); page.Controls.Add(split); return page;
    }

    private TabPage Productos()
    {
        var page = Pagina("Productos"); var split = Division(); var form = Formulario("Nuevo producto"); var codigo = Campo(form, "Código", 48); var descripcion = Campo(form, "Descripción", 112);
        form.Controls.Add(new Label { Text = "Precio (Q)", Location = new Point(14, 166), AutoSize = true, ForeColor = Color.DimGray }); var precio = new NumericUpDown { Location = new Point(14, 190), Width = 300, DecimalPlaces = 2, Maximum = 1000000, ThousandsSeparator = true }; form.Controls.Add(precio);
        var guardar = Boton("Guardar producto", 244); guardar.Click += (_, _) => Ejecutar(() => { Obligatorio(codigo, "Código"); Obligatorio(descripcion, "Descripción"); gestor.Catalogo.AgregarProducto(new Producto(codigo.Text.Trim(), descripcion.Text.Trim(), precio.Value)); Limpiar(codigo, descripcion); precio.Value = 0; Actualizar(); }); form.Controls.Add(guardar); split.Panel1.Controls.Add(form);
        productos = Grid(); split.Panel2.Controls.Add(Lista(productos, () => { if (Valor(productos, "Código") is string id) gestor.EliminarProducto(id); Actualizar(); })); page.Controls.Add(split); return page;
    }

    private TabPage Pedidos()
    {
        var page = Pagina("Pedidos"); var layout = new TableLayoutPanel { Dock = DockStyle.Fill, ColumnCount = 2, Padding = new Padding(22) }; layout.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 58)); layout.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 42));
        var izq = new Panel { Dock = DockStyle.Fill }; izq.Controls.Add(new Label { Text = "Detalle del pedido", Dock = DockStyle.Top, Height = 32, Font = new Font("Segoe UI Semibold", 13) }); lineas = Grid(); izq.Controls.Add(lineas); totalPedido = new Label { Text = "Total: Q0.00", Dock = DockStyle.Bottom, Height = 52, TextAlign = ContentAlignment.MiddleRight, Font = new Font("Segoe UI Semibold", 16), ForeColor = Color.FromArgb(31, 78, 121) }; izq.Controls.Add(totalPedido);
        var quitar = new Button { Text = "Quitar línea seleccionada", Dock = DockStyle.Bottom, Height = 38, FlatStyle = FlatStyle.Flat }; quitar.Click += (_, _) => { if (lineas.CurrentRow?.DataBoundItem is DetallePedido d) detalle.Remove(d); ActualizarDetalle(); }; izq.Controls.Add(quitar);
        var der = new Panel { Dock = DockStyle.Fill, Padding = new Padding(18, 0, 0, 0) }; var form = Formulario("Crear pedido");
        form.Controls.Add(new Label { Text = "Proveedor", Location = new Point(14, 48), AutoSize = true, ForeColor = Color.DimGray }); proveedorPedido = new ComboBox { Location = new Point(14, 70), Width = 320, DropDownStyle = ComboBoxStyle.DropDownList }; form.Controls.Add(proveedorPedido);
        form.Controls.Add(new Label { Text = "Producto", Location = new Point(14, 112), AutoSize = true, ForeColor = Color.DimGray }); productoPedido = new ComboBox { Location = new Point(14, 134), Width = 320, DropDownStyle = ComboBoxStyle.DropDownList }; form.Controls.Add(productoPedido);
        form.Controls.Add(new Label { Text = "Cantidad", Location = new Point(14, 176), AutoSize = true, ForeColor = Color.DimGray }); cantidad = new NumericUpDown { Location = new Point(14, 198), Width = 150, DecimalPlaces = 2, Minimum = .01M, Maximum = 100000, Value = 1 }; form.Controls.Add(cantidad);
        form.Controls.Add(new Label { Text = "Descuento %", Location = new Point(184, 176), AutoSize = true, ForeColor = Color.DimGray }); descuento = new NumericUpDown { Location = new Point(184, 198), Width = 150, DecimalPlaces = 2, Maximum = 100 }; form.Controls.Add(descuento);
        var agregar = Boton("Agregar línea", 252); agregar.Click += (_, _) => Ejecutar(() => { if (productoPedido.SelectedItem is not Producto p) throw new InvalidOperationException("Seleccione un producto."); detalle.Add(new DetallePedido(p, cantidad.Value, descuento.Value)); ActualizarDetalle(); }); form.Controls.Add(agregar);
        var confirmar = Boton("Confirmar pedido", 306); confirmar.BackColor = Color.FromArgb(35, 145, 112); confirmar.Click += (_, _) => Ejecutar(() => { if (proveedorPedido.SelectedItem is not Proveedor p) throw new InvalidOperationException("Seleccione un proveedor."); if (!detalle.Any()) throw new InvalidOperationException("Agregue al menos una línea al pedido."); var pedido = gestor.CrearPedido(p); foreach (var d in detalle) pedido.AgregarDetalle(d); detalle.Clear(); Actualizar(); MessageBox.Show($"Pedido #{pedido.Numero} guardado correctamente.", "Pedido confirmado", MessageBoxButtons.OK, MessageBoxIcon.Information); }); form.Controls.Add(confirmar); der.Controls.Add(form);
        layout.Controls.Add(izq, 0, 0); layout.Controls.Add(der, 1, 0); page.Controls.Add(layout); return page;
    }

    private TabPage Historial()
    {
        var page = Pagina("Historial de pedidos");
        var split = new SplitContainer { Dock = DockStyle.Fill, Orientation = Orientation.Horizontal };
        split.Panel1.Padding = new Padding(22, 22, 22, 8); split.Panel2.Padding = new Padding(22, 8, 22, 22);
        split.Panel1.Controls.Add(new Label { Text = "Pedidos confirmados", Dock = DockStyle.Top, Height = 30, Font = new Font("Segoe UI Semibold", 13) });
        historialPedidos = Grid(); historialPedidos.SelectionChanged += (_, _) => MostrarDetalleHistorial(); split.Panel1.Controls.Add(historialPedidos);
        split.Panel2.Controls.Add(new Label { Text = "Detalle del pedido seleccionado", Dock = DockStyle.Top, Height = 30, Font = new Font("Segoe UI Semibold", 13) });
        historialDetalle = Grid(); split.Panel2.Controls.Add(historialDetalle);
        var eliminar = new Button { Text = "Eliminar pedido seleccionado", Dock = DockStyle.Bottom, Height = 40, FlatStyle = FlatStyle.Flat, ForeColor = Color.Firebrick };
        eliminar.Click += (_, _) => Ejecutar(() => { if (Valor(historialPedidos, "Número") is int numero) gestor.EliminarPedido(numero); Actualizar(); });
        split.Panel1.Controls.Add(eliminar); page.Controls.Add(split); return page;
    }

    private void Actualizar()
    {
        if (proveedores is null) return;
        proveedores.DataSource = gestor.Proveedores.Select(p => new { p.CUI, p.NombreComercial, Contacto = p.NombreCompleto, p.Nit, Dirección = p.DireccionEntrega }).ToList();
        productos.DataSource = gestor.Catalogo.Productos.Select(p => new { Código = p.Codigo, p.Descripcion, Precio = p.Precio.ToString("C2") }).ToList();
        if (historialPedidos is not null)
            historialPedidos.DataSource = gestor.Pedidos.Select(p => new { Número = p.Numero, Fecha = p.Fecha.ToString("dd/MM/yyyy"), Proveedor = p.Proveedor.NombreComercial, Total = p.Total.ToString("C2") }).ToList();
        proveedorPedido.DataSource = gestor.Proveedores.ToList(); productoPedido.DataSource = gestor.Catalogo.Productos.ToList(); resumen.Text = $"{gestor.Proveedores.Count}  PROVEEDORES\n\n{gestor.Catalogo.Productos.Count}  PRODUCTOS\n\n{gestor.Pedidos.Count}  PEDIDOS CONFIRMADOS\n\n{gestor.TotalGeneral():C2}  TOTAL EN PEDIDOS"; ActualizarDetalle();
    }
    private void ActualizarDetalle() { if (lineas is null) return; lineas.DataSource = null; lineas.DataSource = detalle.ToList(); totalPedido.Text = $"Total: {detalle.Sum(d => d.Total):C2}"; }
    private void MostrarDetalleHistorial()
    {
        if (historialDetalle is null || historialPedidos?.CurrentRow?.Cells["Número"].Value is not int numero) return;
        var pedido = gestor.BuscarPedidoPorNumero(numero);
        historialDetalle.DataSource = pedido?.Detalles.Select(d => new { Código = d.Producto.Codigo, Producto = d.Producto.Descripcion, d.Cantidad, Descuento = $"{d.PorcentajeDescuento:0.##}%", Total = d.Total.ToString("C2") }).ToList();
    }
    private static TabPage Pagina(string texto) => new() { Text = texto, BackColor = Color.FromArgb(245, 247, 250) };
    private static SplitContainer Division() => new()
    {
        Dock = DockStyle.Fill,
        // El control se crea antes de tener el ancho final de la ventana.
        // Dejar que WinForms calcule inicialmente la división evita que
        // SplitterDistance sea incompatible con los tamaños mínimos.
        Panel1 = { Padding = new Padding(22) },
        Panel2 = { Padding = new Padding(12, 22, 22, 22) }
    };
    private static DataGridView Grid() => new() { Dock = DockStyle.Fill, ReadOnly = true, AllowUserToAddRows = false, AllowUserToDeleteRows = false, AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill, SelectionMode = DataGridViewSelectionMode.FullRowSelect, MultiSelect = false, BackgroundColor = Color.White, BorderStyle = BorderStyle.None, RowHeadersVisible = false };
    private static Panel Formulario(string titulo) { var p = new Panel { Dock = DockStyle.Fill, BackColor = Color.White, BorderStyle = BorderStyle.FixedSingle }; p.Controls.Add(new Label { Text = titulo, Location = new Point(14, 14), AutoSize = true, Font = new Font("Segoe UI Semibold", 13), ForeColor = Color.FromArgb(31, 78, 121) }); return p; }
    private static TextBox Campo(Panel p, string titulo, int y) { p.Controls.Add(new Label { Text = titulo, Location = new Point(14, y), AutoSize = true, ForeColor = Color.DimGray }); var t = new TextBox { Location = new Point(14, y + 22), Width = 300 }; p.Controls.Add(t); return t; }
    private static Button Boton(string texto, int y) => new() { Text = texto, Location = new Point(14, y), Width = 320, Height = 38, FlatStyle = FlatStyle.Flat, BackColor = Color.FromArgb(45, 120, 180), ForeColor = Color.White, FlatAppearance = { BorderSize = 0 } };
    private static Panel Lista(DataGridView g, Action eliminar) { var p = new Panel { Dock = DockStyle.Fill }; p.Controls.Add(g); var b = new Button { Text = "Eliminar seleccionado", Dock = DockStyle.Bottom, Height = 40, FlatStyle = FlatStyle.Flat, ForeColor = Color.Firebrick }; b.Click += (_, _) => Ejecutar(eliminar); p.Controls.Add(b); return p; }
    private static void Obligatorio(TextBox campo, string nombre) { if (string.IsNullOrWhiteSpace(campo.Text)) throw new InvalidOperationException($"El campo '{nombre}' es obligatorio."); }
    private static object? Valor(DataGridView g, string columna) => g.CurrentRow?.Cells[columna].Value;
    private static void Limpiar(params TextBox[] campos) { foreach (var c in campos) c.Clear(); }
    private static void Ejecutar(Action accion) { try { accion(); } catch (Exception ex) { MessageBox.Show(ex.Message, "No se pudo completar la operación", MessageBoxButtons.OK, MessageBoxIcon.Warning); } }
}
