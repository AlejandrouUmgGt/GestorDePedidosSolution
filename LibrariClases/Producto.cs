namespace LibreriaClases
{
    public class Producto
    {
        public string Codigo { get; set; }
        public string Descripcion { get; set; }

        private decimal precio;
        public decimal Precio
        {
            get => precio;
            set
            {
                if (value < 0)
                    throw new ArgumentException("El precio no puede ser negativo.", nameof(value));
                precio = value;
            }
        }

        public Producto()
        {
            Codigo = string.Empty;
            Descripcion = string.Empty;
            precio = 0;
        }

        public Producto(string codigo, string descripcion, decimal precio)
        {
            Codigo = codigo;
            Descripcion = descripcion;
            Precio = precio; // pasa por el setter, valida negativos
        }

        public override string ToString()
        {
            return $"{Codigo} - {Descripcion} (Q{Precio:0.00})";
        }

        public override bool Equals(object? obj)
        {
            return obj is Producto p && p.Codigo == Codigo;
        }

        public override int GetHashCode() => Codigo.GetHashCode();
    }
}
