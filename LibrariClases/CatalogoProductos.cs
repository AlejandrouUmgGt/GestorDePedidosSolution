namespace LibreriaClases
{
    /// <summary>
    /// Colabora entre Proveedor y Producto: administra el listado de
    /// productos que pueden incluirse en un pedido.
    /// </summary>
    public class CatalogoProductos
    {
        private readonly List<Producto> productos;

        public CatalogoProductos()
        {
            productos = new List<Producto>();
        }

        public IReadOnlyList<Producto> Productos => productos.AsReadOnly();

        public void AgregarProducto(Producto producto)
        {
            if (producto is null)
                throw new ArgumentNullException(nameof(producto));

            if (productos.Any(p => p.Codigo == producto.Codigo))
                throw new InvalidOperationException($"Ya existe un producto con el código '{producto.Codigo}'.");

            productos.Add(producto);
        }

        public bool EliminarProducto(string codigo)
        {
            var producto = BuscarPorCodigo(codigo);
            return producto is not null && productos.Remove(producto);
        }

        public Producto? BuscarPorCodigo(string codigo)
        {
            return productos.FirstOrDefault(p => p.Codigo == codigo);
        }

        public IEnumerable<Producto> Buscar(string textoDescripcion)
        {
            return productos.Where(p =>
                p.Descripcion.Contains(textoDescripcion, StringComparison.OrdinalIgnoreCase));
        }
    }
}
