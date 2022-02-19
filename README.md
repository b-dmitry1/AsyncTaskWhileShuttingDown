# Задача: дождаться завершения асинхронной задачи при завершении сеанса, перезагрузке или отключении питания.

При завершении пользовательской сессии нам надо отправить данные на сервер при помощи асинхронного API,
реализованного в виде метода async Task SendData.
Задача: модифицировать код таким образом, чтобы данные успели записаться в файл до завершения сессии.
Изменять код метода SendData нельзя, заменять его другим методом нельзя.

Как воспроизвести: собираем и запускаем приложение, разлогиниваемся пользователем из системы, логинимся снова, проверяем файл out.txt в папке с приложением. 
Если файл будет содержать строку вида "Time: 19.02.2022 21:23:24 Reason: Logout", то победа.

PS. Один из известных способов сделать это - выполнить асинхронный код синхронно при помощи магии, описаной здесь:
https://stackoverflow.com/questions/5095183/how-would-i-run-an-async-taskt-method-synchronously
Этот метод работает, но он не кошерный.
