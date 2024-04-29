using Graduation_Project.Shared.Framework;

namespace Graduation_Project.Shared.Models
{
    public class Subject : BaseEntity
    {

        public string SubjectName {  get; set; }
        public string ? SubjectAbbreviation {  get; set; }

        public ICollection<TeacherSubject> TeacherSubjects { get; set; }
        public ICollection<Homework> Homeworks { get; set; }

    }
}