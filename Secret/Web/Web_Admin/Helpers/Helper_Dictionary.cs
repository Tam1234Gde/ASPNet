namespace HIDE.RestAndRant.Web_Admin.Helpers
{
    /// <summary>
    /// Словарь > Иконки
    /// </summary>
    public static class Helper_Dictionary
    {
        public static List<KeyValuePair<string, string>> Dictionary_Icons()
        {
            List<KeyValuePair<string, string>> list_result = [];
            KeyValuePair<string, string> item;

            // 0
            item = new KeyValuePair<string, string>(key: "Пользователи", value: "<i class='nav-icon fa fa-users' style='margin-right:5px;'></i>");
            list_result.Add(item);

            // 1
            item = new KeyValuePair<string, string>(key: "Категории", value: "<i class='nav-icon fa fa-cubes' style='margin-right:5px;'></i>");
            list_result.Add(item);

            // 2
            item = new KeyValuePair<string, string>(key: "Блюда", value: "<i class='nav-icon fa fa-coffee' style='margin-right:5px;'></i>");
            list_result.Add(item);

            // 3
            item = new KeyValuePair<string, string>(key: "Удалить", value: "<i class='nav-icon nav-icon fas fa-trash' style='margin-right:5px;'></i>");
            list_result.Add(item);

            // 4
            item = new KeyValuePair<string, string>(key: "Добавить", value: "<i class='nav-icon nav-icon fas fa-plus' style='margin-right:5px;'></i>");
            list_result.Add(item);

            // 5
            item = new KeyValuePair<string, string>(key: "Изменить", value: "<i class='nav-icon nav-icon fas fa-edit' style='margin-right:5px;'></i>");
            list_result.Add(item);

            // 6
            item = new KeyValuePair<string, string>(key: "Клиенты", value: "<i class='nav-icon nav-icon fas fa-male' style='margin-right:5px;'></i>");
            list_result.Add(item);

            // 7
            item = new KeyValuePair<string, string>(key: "Заказы", value: "<i class='nav-icon nav-icon fas fa-shopping-cart' style='margin-right:5px;'></i>");
            list_result.Add(item);

            // 8
            item = new KeyValuePair<string, string>(key: "Рестораны", value: "<i class='nav-icon nav-icon fas fa-building' style='margin-right:5px;'></i>");
            list_result.Add(item);

            // 9
            item = new KeyValuePair<string, string>(key: "Столики в ресторане", value: "<i class='nav-icon nav-icon fas fa-certificate' style='margin-right:5px;'></i>");
            list_result.Add(item);

            // 10
            item = new KeyValuePair<string, string>(key: "Объёмы", value: "<i class='nav-icon nav-icon fas fa-puzzle-piece' style='margin-right:5px;'></i>");
            list_result.Add(item);

            // 11
            item = new KeyValuePair<string, string>(key: "Бонусы", value: "<i class='nav-icon nav-icon fas fa-gift' style='margin-right:5px;'></i>");
            list_result.Add(item);

            // 12
            item = new KeyValuePair<string, string>(key: "Статусы заказов", value: "<i class='nav-icon nav-icon fas fa-tag' style='margin-right:5px;'></i>");
            list_result.Add(item);

            // 13
            item = new KeyValuePair<string, string>(key: "Оценка приложения", value: "<i class='nav-icon nav-icon fas fa-star' style='margin-right:5px;'></i>");
            list_result.Add(item);

            // 14
            item = new KeyValuePair<string, string>(key: "Города", value: "<i class='nav-icon nav-icon fas fa-bank' style='margin-right:5px;'></i>");
            list_result.Add(item);

            // 15
            item = new KeyValuePair<string, string>(key: "Пуши", value: "<i class='nav-icon nav-icon fas fa-commenting' style='margin-right:5px;'></i>");
            list_result.Add(item);

            // 16
            item = new KeyValuePair<string, string>(key: "Баннеры", value: "<i class='nav-icon fa-solid fa-panorama' style='margin-right:5px;'></i>");
            list_result.Add(item);

            return list_result;
        }

    }
}