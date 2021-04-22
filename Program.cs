using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OOP_cv11
{
    public class Program
    {
        static void Main(string[] args)
        {
            DataClassesDataContext db = new DataClassesDataContext();
            FillDatabase(db);
            var MICstudents = StudentsOfSubject(db, "BPC-MIC");
            var Subjects221000 = SubjectsOfStudent(db, 221000);

            var subAvg = from g in db.Gradings
                         group g by g.SubjectAbbreviation into s
                         select new
                         {
                             s.Key,
                             average = s.Average(p => p.Grade)
                         };

            foreach (var entry in subAvg)
            {
                Console.WriteLine("Subject: {0} Average grade: {1}", entry.Key, entry.average);
            }
        }
        public static void FillDatabase(DataClassesDataContext db)
        {
            Subject[] newSubjects = {
                new Subject() { Abbreviation = "BPC-OOP", SubjectName = "Objektově orientované programování" },
                new Subject() { Abbreviation = "BPC-MIC", SubjectName = "Vestavné systémy a mikroprocesory" },
            };
            Student[] newStudents = {
                new Student() { Firstname = "Jan", Surname = "Navrátil", DateOfBirth = new DateTime(1999, 1, 1), ID = 221000 },
                new Student() { Firstname = "Petr", Surname = "Fero", DateOfBirth = new DateTime(1998, 5, 5), ID = 221001 },
            };
            Attendance[] newAttendance = {
                new Attendance() { StudentID = 221000, SubjectAbbreviation = "BPC-OOP" },
                new Attendance() { StudentID = 221001, SubjectAbbreviation = "BPC-OOP" },
                new Attendance() { StudentID = 221000, SubjectAbbreviation = "BPC-MIC" },
            };
            Grading[] newGrading = {
                new Grading() { StudentID = 221000, SubjectAbbreviation = "BPC-OOP", GradingDate = new DateTime(2021, 4, 19), Grade = 1},
                new Grading() { StudentID = 221001, SubjectAbbreviation = "BPC-OOP", GradingDate = new DateTime(2021, 4, 19), Grade = 1.5},
                new Grading() { StudentID = 221000, SubjectAbbreviation = "BPC-MIC", GradingDate = new DateTime(2021, 4, 19), Grade = 2},
            };
            foreach (Subject s in newSubjects)
            {
                if (!db.Subjects.Any(sub => sub.Abbreviation == s.Abbreviation))    //check if primary key doesn't exist already
                {
                    db.Subjects.InsertOnSubmit(s);
                }
            }
            foreach (Student s in newStudents)
            {
                if (!db.Students.Any(stu => stu.ID == s.ID))
                {
                    db.Students.InsertOnSubmit(s);
                }
            }
            foreach (Attendance a in newAttendance)
            {
                if (!db.Attendances.Any(at => (at.StudentID == a.StudentID) && (at.SubjectAbbreviation == a.SubjectAbbreviation)))
                {
                    db.Attendances.InsertOnSubmit(a);
                }
            }
            foreach (Grading g in newGrading)
            {
                if (!db.Gradings.Any(gr => (gr.StudentID == g.StudentID) && (gr.SubjectAbbreviation == g.SubjectAbbreviation)))
                {
                    db.Gradings.InsertOnSubmit(g);
                }
            }
            db.SubmitChanges();
        }
        public static IEnumerable<Student> StudentsOfSubject(DataClassesDataContext db, string subjectAbbreviation)
        {
            return (from a in db.Attendances
                    where a.SubjectAbbreviation == subjectAbbreviation
                    select a.Student);
        }
        public static IEnumerable<Subject> SubjectsOfStudent(DataClassesDataContext db, int studentID)
        {
            return (from a in db.Attendances
                    where a.StudentID == studentID
                    select a.Subject);
        }
    }
}
