using System;
using System.Collections.Generic;
using System.Text;
using HIDE.RestAndRant.Business;
using HIDE.RestAndRant.Business.Definitions;
using HIDE.RestAndRant.Business_YooKassa;

namespace HIDE.RestAndRant.Web_Api_Test
{
    [TestClass]
    public class NotificationUnitTest
    {

        private readonly Client_Notifications _client_Notifications = new();


        /// <summary>
        /// Конструктор
        /// </summary>
        public NotificationUnitTest()
        {
            TestHelper.Init();
        }

        /// <summary>
        /// Проверяет получение уведомления с базы
        /// </summary>
        /// <param name="sum">Сумма платежа</param>
        [TestMethod]
        [DataRow(1)]
        [DataRow(2)]
        [DataRow(3)]
        public void CreateUrl(int ID)
        {
            CClient_Notification? url = _client_Notifications.Get(ID: ID);
            Assert.IsNotNull(url);
        }
    }
}
