using DistributedeXtendedArchitectureWithMysql.Model;

namespace DistributedeXtendedArchitectureWithMysql.Service
{
    public interface IBankTransferService
    {
        Task<TransferResult> TransferWithoutXAAsync(TransferRequest request);

        Task<TransferResult> TransferWithXAAsync(TransferRequest request);
    }
}
