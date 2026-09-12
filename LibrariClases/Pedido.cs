namespace LibreriaClases
{
    /// <summary>
    /// Pedido realizado a un Proveedor. Se compone de una o más líneas
    /// de DetallePedido (composición: si el pedido se destruye, sus
    /// líneas de detalle dejan de existir).
    /// </summary>
    public class Pedido
    {
        public Proveedor Proveedor { get; set; }
        public int Numero { get; set; }
        public DateOnly Fecha { get; set; }

        private readonly List<DetallePedido> detalles;
        public IReadOnlyList<DetallePedido> Detalles => detalles.AsReadOnly();

        public decimal Total => detalles.Sum(d => d.Total);

        public Pedido(Proveedor proveedor, int numero, DateOnly? fecha = null)
        {
            Proveedor = proveedor ?? throw new ArgumentNullException(nameof(proveedor));
            Numero = numero;
            Fecha = fecha ?? DateOnly.FromDateTime(DateTime.Now);
            detalles = new List<DetallePedido>();
        }

        /// <summary>Permite agregar líneas de detalle dinámicamente.</summary>
        public void AgregarDetalle(DetallePedido detalle)
        {
            if (detalle is null)
                throw new ArgumentNullException(nameof(detalle));
            detalles.Add(detalle);
        }

        public void AgregarDetalle(Producto producto, decimal cantidad, decimal porcentajeDescuento = 0)
        {
            AgregarDetalle(new DetallePedido(producto, cantidad, porcentajeDescuento));
        }

        public bool EliminarDetalle(DetallePedido detalle)
        {
            return detalles.Remove(detalle);
        }

        /// <summary>Un pedido es válido si tiene proveedor y al menos 1 línea de detalle.</summary>
        public bool EsValido()
        {
            return Proveedor is not null && detalles.Count >= 1;
        }

        public override string ToString()
        {
            return $"Pedido #{Numero} - {Proveedor.NombreComercial} - {Fecha:yyyy-MM-dd} - Total: Q{Total:0.00}";
        }
    }
}
