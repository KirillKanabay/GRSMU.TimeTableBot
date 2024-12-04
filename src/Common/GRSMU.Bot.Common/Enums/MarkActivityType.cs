namespace GRSMU.Bot.Common.Enums;

public enum MarkActivityType
{
    Unknown = -1,
    /// <summary>
    /// Пр--Практическое занятие
    /// </summary>
    Practise = 0,
    /// <summary>
    /// И--Итоговое занятие
    /// </summary>
    Final = 1,
    /// <summary>
    /// Т--Тестирование
    /// </summary>
    Test = 2,
    /// <summary>
    /// ИБ--История болезней
    /// </summary>
    HistoryOfDiseases = 3,
    /// <summary>
    /// ДЗ--Дифференцированный зачет
    /// </summary>
    DifferentiatedExam = 4,
    /// <summary>
    /// Э--Экзамен - обычный (без рейтинговой системы)
    /// </summary>
    Exam = 5,
    /// <summary>
    /// С--Семинарское занятие
    /// </summary>
    Seminar = 6,
    /// <summary>
    /// Лек--Лекция
    /// </summary>
    Lecture = 7,
    /// <summary>
    /// УЗ--Учебное занятие
    /// </summary>
    Training = 8,
    /// <summary>
    /// ПН--Практические навыки,
    /// </summary>
    PractiseSkills = 9
}