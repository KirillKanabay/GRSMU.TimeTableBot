﻿using GRSMU.TimeTable.Common.Data.Documents;

namespace GRSMU.TimeTableBot.Data.Documents;

public class ReportDocument : DocumentBase
{
    public string TelegramId { get; set; }

    public string Message { get; set; }
}