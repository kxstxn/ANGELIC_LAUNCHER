# Angelic Launcher

Кастомный Minecraft-лаунчер для Windows на WPF.

Позволяет выбрать версию, ввести ник, указать сервер и запустить игру в оффлайн-режиме.

## Возможности

- Список release-версий Minecraft
- Оффлайн-авторизация
- Автоустановка версии через CmlLib
- Прогресс скачивания
- Автоподключение к серверу
- Сохранение настроек в `settings.json`

## Стек

- .NET 10 (`net10.0-windows`)
- WPF + WPF-UI
- [CmlLib.Core](https://github.com/CmlLib/CmlLib.Core)

## Запуск

```bash
git clone https://github.com/kxstxn/ANGELIC_LAUNCHER.git
cd ANGELIC_LAUNCHER
dotnet run --project ANGELIC_LAUNCHER
```

Нужны Windows, [.NET 10 SDK](https://dotnet.microsoft.com/download) и Java.

## Использование

1. Введи ник (по умолчанию — `Angelic_User`)
2. Выбери версию
3. При желании укажи IP сервера
4. Нажми **Play**

## Лицензия

MIT — см. [LICENSE.txt](LICENSE.txt)
