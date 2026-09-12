namespace LibreriaClases
{
    /// <summary>
    /// Punto de entrada del backend: administra Proveedores, el Catálogo
    /// de Productos y los Pedidos generados. Esta es la clase que el
    /// Frontend (Windows Forms) va a consumir.
    /// </summary>
    public class GestorPedidos
    {
        public CatalogoProductos Catalogo { get; }

        private readonly List<Proveedor> proveedores;
        private readonly List<Pedido> pedidos;
        private int siguienteNumeroPedido = 1;

        public GestorPedidos()
        {
            Catalogo = new CatalogoProductos();
            proveedores = new List<Proveedor>();
            pedidos = new List<Pedido>();
        }

        public IReadOnlyList<Proveedor> Proveedores => proveedores.AsReadOnly();
        public IReadOnlyList<Pedido> Pedidos => pedidos.AsReadOnly();

        // ---------- Proveedores ----------

        public void AgregarProveedor(Proveedor proveedor)
        {
            if (proveedor is null)
                throw new ArgumentNullException(nameof(proveedor));

            if (proveedores.Any(p => p.CUI == proveedor.CUI))
                throw new InvalidOperationException($"Ya existe un proveedor con el CUI '{proveedor.CUI}'.");

            proveedores.Add(proveedor);
        }

        public bool EliminarProveedor(string cui)
        {
            var proveedor = BuscarProveedorPorCui(cui);
            return proveedor is not null && proveedores.Remove(proveedor);
        }

        public Proveedor? BuscarProveedorPorCui(string cui)
        {
            return proveedores.FirstOrDefault(p => p.CUI == cui);
        }

        // ---------- Pedidos ----------

        /// <summary>Crea un pedido vacío (sin detalle todavía) asociado a un proveedor.</summary>
        public Pedido CrearPedido(Proveedor proveedor, DateOnly? fecha = null)
        {
            var pedido = new Pedido(proveedor, siguienteNumeroPedido, fecha);
            siguienteNumeroPedido++;
            pedidos.Add(pedido);
            return pedido;
        }

        public Pedido? BuscarPedidoPorNumero(int numero)
        {
            return pedidos.FirstOrDefault(p => p.Numero == numero);
        }

        public bool EliminarPedido(int numero)
        {
            var pedido = BuscarPedidoPorNumero(numero);
            return pedido is not null && pedidos.Remove(pedido);
        }

        public IEnumerable<Pedido> PedidosPorProveedor(string cui)
        {
            return pedidos.Where(p => p.Proveedor.CUI == cui);
        }

        public decimal TotalGeneral()
        {
            return pedidos.Sum(p => p.Total);
        }
    }
}
