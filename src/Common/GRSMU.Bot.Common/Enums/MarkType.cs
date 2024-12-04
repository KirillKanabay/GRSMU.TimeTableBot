namespace GRSMU.Bot.Common.Enums;

public enum MarkType
{
    None = 0,
    /// <summary>
    /// Оценка
    /// </summary>
    DefaultMark = 1,
    /// <summary>
    /// Оценка х зеленого цвета - за отработанное занятие по уважительной причине;
    /// </summary>
    SeriousWorkOutMark = 2,
    /// <summary>
    /// Оценка х красного цвета - за отработанное занятие по неуважительной причине;
    /// </summary>
    NotSeriousWorkOutMark = 3,
    /// <summary>
    /// н - Отсутствие на занятии до выяснения причины
    /// </summary>
    UnknownAbsence = 4,
    /// <summary>
    /// ну - Отсутствие на занятии по уважительной причине;
    /// </summary>
    SeriousAbsence = 5,
    /// <summary>
    /// нн - Отсутствие на занятии по неуважительной причине;
    /// </summary>
    NotSeriousAbsence = 6,
    /// <summary>
    /// нбп - Отсутствие на занятии по уважительной причине, без повторного посещения;
    /// </summary>
    SeriousAbsenceWithoutRepeatVisit,
    /// <summary>
    /// Зачтено (при пропуске лекции);
    /// </summary>
    LecturePassed,
    /// <summary>
    /// нзч - Не зачтено (при пропуске лекции);
    /// </summary>
    LectureFail
}