namespace tu_vet_back.tuvet.Model.TuVet
{
    public class CasasComerciales
    {
        public Guid IdCasaComercial { get; set; }
            = Guid.NewGuid();

        public string NombreCasaComercial { get; set; }
            = string.Empty;

        public virtual ICollection<Productos> Productos
        {
            get;
            set;
        } = new List<Productos>();
    }
}