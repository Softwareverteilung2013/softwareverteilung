namespace SVApi.Models
{
    public enum RequestTyp
    {
        upDateRequest,
        getDatabaseGroups,
        getDatabaseClients,
        getDatabaseSoftwarePackages,
        addDatabaseClient,
        addDatabaseGroup,
        addDatabaseSoftwarePackage,
        delDatabaeClient,
        delDatabaseGroup,
        delDatabaseSoftwarePackage,
        sendSoftwarePackage,
    };

    public class StandardRequestModel
    {
        public RequestTyp request { get; set; }
        public object requestData { get; set; }
        public ClientInfoModel Client { get; set; }
    }
}
