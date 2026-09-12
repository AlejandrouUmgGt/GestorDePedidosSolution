namespace LibreriaClases
{
    /// <summary>
    /// Línea de detalle de un Pedido: un producto, la cantidad solicitada
    /// y el porcentaje de descuento aplicado. El total se calcula solo.
    /// </summary>
    public class DetallePedido
    {
        public Producto Producto { get; set; }

        private decimal cantidad;
        public decimal Cantidad
        {
            get => cantidad;
            set
            {
                if (value < 0)
                    throw new ArgumentException("La cantidad no puede ser negativa.", nameof(value));
                cantidad = value;
            }
        }

        private decimal porcentajeDescuento;
        public decimal PorcentajeDescuento
        {
            get => porcentajeDescuento;
            set
            {
                if (value < 0)
                    throw new ArgumentException("El porcentaje de descuento debe ser 0 o positivo.", nameof(value));
                porcentajeDescuento = value;
            }
        }

        /// <summary>Total calculado de la línea (precio * cantidad, menos el descuento).</summary>
        public decimal Total
        {
            get
            {
                decimal subtotal = Producto.Precio * Cantidad;
                decimal montoDescuento = subtotal * (PorcentajeDescuento / 100m);
                return subtotal - montoDescuento;
            }
        }

        public DetallePedido(Producto producto, decimal cantidad, decimal porcentajeDescuento = 0)
        {
            Producto = producto ?? throw new ArgumentNullException(nameof(producto));
            Cantidad = cantidad;
            PorcentajeDescuento = porcentajeDescuento;
        }

        public override string ToString()
        {
            return $"{Producto.Descripcion} x{Cantidad} (-{PorcentajeDescuento}%) = Q{Total:0.00}";
        }
    }
}
