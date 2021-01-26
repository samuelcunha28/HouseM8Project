using System.Data.SqlClient;

namespace HouseM8API.Interfaces
{
    public interface IConnection
    {
        SqlConnection Open();
        SqlConnection Fetch();
        void Close();
    }
}
