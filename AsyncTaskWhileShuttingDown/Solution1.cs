using Microsoft.Win32;

// При завершении пользовательской сессии нам надо отправить данные на сервер при помощи асинхронного API,
// реализованного в виде метода async Task SendData.
// Задача: модифицировать код таким образом, чтобы данные успели записаться в файл до завершения сессии.
// Изменять код метода SendData нельзя, заменять его другим методом нельзя.


Console.WriteLine("Disconnect, lock, reboot or shutdown to save your time.");

SystemEvents.SessionEnding += SystemEvents_SessionEnding;
SystemEvents.SessionSwitch += SystemEvents_SessionSwintc;

// Чтобы можно было отпределить, когда пользователь вернулся в систему
// после Lock или после смены пользователя
SystemEvents.PowerModeChanged += SystemEvents_PowerModeChanged;

while (true)
{
    var command = Console.ReadLine();
    if (command == "quit")
    {
        break;
    }
}

void SystemEvents_SessionEnding(object sender, SessionEndingEventArgs e)
{
    SaveTime(e.Reason.ToString());
}

void SystemEvents_SessionSwintc(object sender, SessionSwitchEventArgs e)
{
    SaveTime(e.Reason.ToString());
}

void SystemEvents_PowerModeChanged(object sender, PowerModeChangedEventArgs e)
{
    SaveTime(e.Mode.ToString());
}

async Task SendData(string reason)
{
    // connect to service
    await Task.Delay(500);
    
    // Здесь нужно уложиться в 30 секунд, иначе появится сообщение о зависшей задаче,
    // и Windows предложит завершить ее вручную

    // send data
    File.AppendAllText("out.txt", $"Time: {DateTime.Now} Reason: {reason}");
}

async void SaveTime(string reason)
{
    File.AppendAllText("out.txt", $"{DateTime.Now} Starting to send data.");

    // Эту строку Visual Studio подчеркнет зеленой линией и предупредит,
    // что здесь нет await, и код будет выполнен синхронно, но нам так и надо!
    Task.Run(async() => await SendData(reason));
    
    // Если нужна поддержка временной блокировки рабочего места,
    // то можно не отписываться от этих событий, либо после восстановления
    // сеанса заново добавлять
    // SystemEvents.SessionEnding -= SystemEvents_SessionEnding;
    // SystemEvents.SessionSwitch -= SystemEvents_SessionSwintc;
}
