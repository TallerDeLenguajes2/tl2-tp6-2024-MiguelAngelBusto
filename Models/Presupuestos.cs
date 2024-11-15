namespace tl2_tp6_2024_MiguelAngelBusto.Models
{
    public class Presupuestos
    {
        private int idPresupuesto;
        private string nombreDestinatario;
        private List<PresupuestoDetalle> detalle;
        private DateTime fechaCreacion;
        public int IdPresupuesto { get => idPresupuesto; set => idPresupuesto = value; }
        public string NombreDestinatario { get => nombreDestinatario; set => nombreDestinatario = value; }
        public List<PresupuestoDetalle> Detalle { get => detalle; set => detalle = value; }
        public DateTime FechaCreacion { get => fechaCreacion; set => fechaCreacion = value; }

        public Presupuestos(){
            
        }

        public Presupuestos(int idPresupuesto, string nombreDestinatario, List<PresupuestoDetalle> detalle)
        {
            this.IdPresupuesto = idPresupuesto;
            this.NombreDestinatario = nombreDestinatario;
            this.Detalle = detalle;
        }

        public int MontoPresupuesto()
        {
            int total = 0;
            foreach (PresupuestoDetalle item in Detalle)
            {
                total = total + item.Productos.Precio * item.Cantidad;
            }
            return total;
        }

        public double MontoPresupuestoConIva()
        {
            int total = 0;
            foreach (PresupuestoDetalle item in Detalle)
            {
                total = total + item.Productos.Precio * item.Cantidad;
            }
            return total * 1.21;
        }

        public int CantidadProductos()
        {
            int contador = 0;
            foreach (PresupuestoDetalle item in Detalle)
            {
                contador = contador + item.Cantidad;
            }
            return contador;
        }
        public void setDetallesPresupuesto(List<PresupuestoDetalle> pdList) {
        this.Detalle = pdList;
        }

    }
}