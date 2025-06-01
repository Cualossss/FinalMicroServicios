namespace MicroservicioValidaciones.Settings
{
    public class MongoDbSettings
    {
        public string ConnectionString { get; set; } = string.Empty;
        public string DatabaseName { get; set; } = string.Empty;
        public string CotizacionesCollectionName { get; set; } = string.Empty;
    }
}
