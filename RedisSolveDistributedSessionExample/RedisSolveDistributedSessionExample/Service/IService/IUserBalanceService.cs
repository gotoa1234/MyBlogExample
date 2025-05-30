﻿namespace RedisSolveDistributedSessionExample.Service.IService
{
    public interface IUserBalanceService
    {
        Task UpdateBalance(int userId, decimal amount);

        Task<decimal> GetBalance(int userId, decimal amount);
    }
}
