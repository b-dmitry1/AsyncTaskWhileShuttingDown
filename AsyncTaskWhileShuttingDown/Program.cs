using Microsoft.Win32;

// При завершении пользовательской сессии нам надо отправить данные на сервер при помощи асинхронного API,
// реализованного в виде метода async Task SendData.
// Задача: модифицировать код таким образом, чтобы данные успели записаться в файл до завершения сессии.
// Изменять код метода SendData нельзя, заменять его другим методом нельзя.


Console.WriteLine("Disconnect, lock, reboot or shutdown to save your time.");

SystemEvents.SessionEnding += SystemEvents_SessionEnding;
SystemEvents.SessionSwitch += SystemEvents_SessionSwintc;



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

async Task SendData(string reason)
{
    // connect to service
    await Task.Delay(500);

    // send data
    File.AppendAllText("out.txt", $"Time: {DateTime.Now} Reason: {reason}");
}

void SaveTime(string reason)
{
    File.AppendAllText("out.txt", $"{DateTime.Now} Starting to send data.");

    Task.Run(async() => await SendData(reason));
    
    SystemEvents.SessionEnding -= SystemEvents_SessionEnding;
    SystemEvents.SessionSwitch -= SystemEvents_SessionSwintc;
}