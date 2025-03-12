﻿namespace DistributedeXtendedArchitectureWithMysql.Model
{
    public class TransferResult
    {
        public bool Success { get; set; }

        public string Message { get; set; } = string.Empty;

        public string TransactionId { get; set; } = string.Empty;
    }
}