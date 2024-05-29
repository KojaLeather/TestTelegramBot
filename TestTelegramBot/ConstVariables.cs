﻿public static class BotMessages
{
    public const string StartCommandMessage =  "Доброго дня!\n" +
                                        "Данный бот может найти информацию о компании по ИНН, а также имеет другой функционал\n" +
                                        "Для вывода всех комманд введите /help";


    public const string HelpCommandMessage =   "Данный бот имеет следующие команды:\n" +
                                        "/help - Справка о всех доступных коммандах\n" +
                                        "/hello - Информация о создателе данного бота\n" +
                                        "/inn - Получить название и адрес компании по ИНН\n" +
                                        "/ergul - Получить выписку из ЕГРЮЛ по ИНН компании\n" +
                                        "/ogrn - Получить ОГРН по ИИН\n" +
                                        "/last - Повторить предыдущую комманду";


    public const string HelloCommandMessage = "Доброго дня!\n" +
                                              "Меня зовут Шумейко Никита, со мной можно связаться по этой почте: shumnfrommagadan@gmail.com\n" +
                                              "Данный проект с открытым кодом, который можно посмотреть по следующей ссылке: {https://github.com/KojaLeather/TestTelegramBot}";


    public const string InnCommandMessage = "Введите ИНН компании. Если ИНН несколько, то напишите их через запятую с пробелом (пример: 7736207543, 7531075125)\n" +
                                              "После ввода информации процесс получения информации занимает несколько секунд.\n";

    public const string ErgulCommandMessage = "Для получения выписки из ЕГРЮЛ введите ИНН без пробелов и лишних символов. После ввода информации процесс получения информации занимает несколько секунд.\n";

    public const string OgrnCommandMessage = "Для получения ОГРН введите ИНН без пробелов и лишних символов. После ввода информации процесс получения информации занимает несколько секунд.";

    public const string InvalidCommandMessage = "Вы ввели неправильную команду, введите /help для получения справки о командах";


    public const string InvalidValueMessage = "Вы ввели неправильные значения команды. Попробуйте снова следуя инструкциям.";

}
