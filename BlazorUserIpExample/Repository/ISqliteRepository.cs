using BlazorUserIpExample.Model;

namespace BlazorUserIpExample.Repository
{
    public interface ISqliteRepository
    {
        public void CreatedRow(UserIpRecordModel row);
    }
}
