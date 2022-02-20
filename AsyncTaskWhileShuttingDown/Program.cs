using Microsoft.Win32;

// При завершении пользовательской сессии нам надо отправить данные на сервер при помощи асинхронного API,
// реализованного в виде метода async Task SendData.
// Задача: модифицировать код таким образом, чтобы данные успели записаться в файл до завершения сессии.
// Изменять код метода SendData нельзя, заменять его другим методом нельзя.

EventWaitHandle ev = new EventWaitHandle(false, EventResetMode.AutoReset);

(new Thread(() =>
{
    ev.WaitOne();
})).Start();

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

// Нужно либо выполнить SaveTime, либо ev.Set();
// иначе программа не завершится
SaveTime("Program quit");

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

void Log(string s)
{
    File.AppendAllText("out.txt", $"{s}\n");
}

async Task SendData(string reason)
{
    // connect to service
    await Task.Delay(500);

    // Здесь нужно уложиться в 30 секунд, иначе появится сообщение о зависшей задаче,
    // и Windows предложит завершить ее вручную

    // send data
    Log($"Time: {DateTime.Now} Reason: {reason}\n");
}

void SaveTime(string reason)
{
    Log($"{DateTime.Now} Starting to send data.\n");

    SendData(reason).Wait();

    // Данные сохранены, можно завершить guard thread
    // На самом деле здесь не всегда надо завершать поток!
    // Если пользователь заблокировал сеанс, то программа продолжит работать
    ev.Set();
}
