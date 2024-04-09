# EffectiveMobileTestTask

## Задание
Дан  файла, содержащий список IP-адресов протокола IPv4 из журнала доступа. На каждой строке записан адрес и время, в которое с него пришёл запрос.

Необходимо разработать консольное приложение, которое  выводит в файл список IP-адресов из файла журнала, входящих в указанный диапазон с количеством обращений с этого адреса в указанный интервал времени. Адрес и время доступа разделено двоеточием. 
Дата в журнале доступа записана в формате: yyyy-MM-dd HH:mm:ss

Все параметры передаются приложению через параметры командной строки:

--file-log — путь к файлу с логами
--file-output — путь к файлу с результатом
--address-start —  нижняя граница диапазона адресов, необязательный параметр, по умолчанию обрабатываются все адреса
--address-mask — маска подсети, задающая верхнюю границу диапазона десятичное число. Необязательный параметр. В случае, если он не указан, обрабатываются все адреса, начиная с нижней границы диапазона. Параметр нельзя использовать, если не задан address-start
--time-start —  нижняя граница временного интервала
--time-end — верхняя граница временного интервала.

Даты в параметрах задаются в формате dd.MM.yyyy


По возможности, кроме передачи параметров через командную строку, предусмотреть возможность частичной/полной передачи параметров через файлы конфигурации или переменные среды

Программа не должна ломаться от некорректных входных данных, ошибок ввода-вывода и прочим причинам, которые можно предусмотреть.

Код должен быть оптимальным и читаемым, при разработке желательно использовать общераспространённые практики (паттерны проектирования, тесты...)


## Запуск программы
Помимо передачи параметров через параметры командной строки, реализовано передача параметров через переменные окружения. Ниже представлено сопостановление параметров командной строки с переменными окружения

--file-log — FILE_LOG <br />
--file-output — FILE_OUTPUT <br />
--address-start —  ADDRESS_START <br />
--address-mask — ADDRESS_MASK <br />
--time-start —  TIME_START <br />
--time-end — TIME_END <br />

Для получение дополнительной информации, необходимо запустить приложение с флагом --help

### Команда для запуска
`NumberOfRequestsFromIP.exe --file-log <path_to_log.txt> --file-output <path_to_res.txt> --time-start <time_start> --time-end <time_end> --address-start <address_start_ipv4> --address-mask <address_mask_ipv4_or_prefix>`