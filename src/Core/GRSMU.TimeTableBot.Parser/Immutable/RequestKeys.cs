namespace GRSMU.TimeTableBot.Core.Immutable
{
    public class RequestKeys
    {
        public const string LastFocus = "__LASTFOCUS";
        public const string ViewState = "__VIEWSTATE";
        public const string ViewStateGenerator = "__VIEWSTATEGENERATOR";
        public const string EventValidation = "__EVENTVALIDATION";
        public const string Faculty = "ddlFaculty";
        public const string Department = "ddlDepartment";
        public const string Course = "ddlCourses";
        public const string Week = "ddlWeek";
        public const string Group = "ddlGroups";

        public static readonly KeyValuePair<string, string> DefaultButton = new("btnShowTT", "Показать");
        public static readonly KeyValuePair<string, string> DefaultFrame = new("iframeheight", "400");
        public static readonly KeyValuePair<string, string> DefaultDepartment = new(Department, "2");
    }
}
