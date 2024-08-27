using Example.Common.FakeDataBase.Model;
using System.Collections.Concurrent;

namespace Example.Common.FakeDataBase
{
    public class FakeDataBase
    {
        protected readonly ConcurrentDictionary<int, AccountTradeOrderModel> _clientQuickPageDict = new();

        public IReadOnlyDictionary<int, AccountTradeOrderModel> ClientQuickPageDict => _clientQuickPageDict;

        /// <summary>
        /// 模擬帳戶系統查詢訂單 - 當前資料
        /// </summary>
        public IReadOnlyDictionary<int, AccountTradeOrderModel> GetAccountTradeOrderAll()
        {
            return _clientQuickPageDict;
        }

        /// <summary>
        /// 模擬帳戶建單 - 消費者 - 更新/插入資料
        /// </summary>
        public void AddOrUpdate(AccountTradeOrderModel insertItem)
        {
            _clientQuickPageDict.AddOrUpdate(insertItem.AccountTradeOrderId, insertItem, (k, oldValue) => insertItem);
        }
    }
}
