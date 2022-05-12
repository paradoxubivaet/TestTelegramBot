using System;
using System.Collections.Generic;

namespace TestTelegramBot
{
    public interface IControllDataBase
    {
        public void Add(User user);
        public bool CheckUser(long identify);
        public DateTime GetDate(long identify);
        public List<long> GetChatId();
    }
}
