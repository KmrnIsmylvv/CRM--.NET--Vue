using System;
using System.Collections.Generic;
using System.Dynamic;
using System.Linq;
using Elfo.Contoso.LearningRoundKamran.Domain.CourseManagement.ValueObjects;
using Elfo.Contoso.LearningRoundKamran.Domain.SystemManagement.ValueObjects;
using Elfo.Round.WriteCycle;

namespace Elfo.Contoso.LearningRoundKamran.Domain.CourseManagement
{
    public class Course : AggregateRoot<CourseID>
    {
        #region Constants
        private const short maxCourseNameLength = 50;
        #endregion

        #region Ctor
        protected Course()
        {
            IsEnabled = true;
        }
        #endregion

        #region Properties
        public virtual string Name { get; set; }
        public virtual string Description { get; set; }
        public virtual DateTimeOffset StartDate { get; set; }
        public virtual DateTimeOffset EndDate { get; set; }
        public virtual bool IsEnabled { get; set; }

        public virtual SystemUserID IdTeacher { get; set; }

        protected readonly IList<Lesson> lessons = new List<Lesson>();
        public virtual IEnumerable<Lesson> Lessons { get { return lessons.AsEnumerable(); } }
        #endregion

        #region Methods
        public static Course Create(string name, string description, SystemUserID idTeacher,
            DateTimeOffset startDate, DateTimeOffset endDate)
        {
            var ex = Validate(name, idTeacher, startDate, endDate);

            var course = new Course
            {
                IdTeacher = idTeacher,
                Name = name,
                Description = description,
                StartDate = startDate,
                EndDate = endDate,
            };

            ex.CollectErrors(() =>
            {
                course.Enable();
            }).TryThrow();

            return course;
        }

        public virtual void Update(string name, string description, DateTimeOffset startDate,
            DateTimeOffset endDate)
        {
            Name = name;
            Description = description;
            StartDate = startDate;
            EndDate = endDate;
        }

        public virtual void Enable()
        {
            IsEnabled = true;
        }

        public virtual void Disable()
        {
            IsEnabled = false;
        }

        public virtual Lesson AddLesson(string name, string description, int duration)
        {
            Lesson lesson;

            lesson = Lesson.Create(Id, name, description, duration);

            lessons.Add(lesson);

            return lesson;
        }

        public virtual void UpdateLesson(LessonID idLesson, string name, string description,
            int duration)
        {
            var lesson = lessons.Single(l => l.Id == idLesson);

            lesson.Update(name, description, duration);
        }

        public virtual void RemoveLesson(LessonID idLesson)
        {
            var lesson = lessons.Single(l => l.Id == idLesson);

            lessons.Remove(lesson);
        }

        private static ValidationException Validate(string courseName, SystemUserID idTeacher, DateTimeOffset startDate, DateTimeOffset endDate)
        {
            var ex = new ValidationException();

            if (string.IsNullOrWhiteSpace(courseName))
                ex.AddError(nameof(Name), ValidationErrorCode.EmptyCourseName);
            else if (courseName.Length > maxCourseNameLength)
                ex.AddError(nameof(Name), ValidationErrorCode.InvalidCourseNameLength);

            if (startDate > endDate)
                ex.AddError(nameof(StartDate), ValidationErrorCode.InvalidCourseDate);

            return ex;
        }

        #endregion
    }
}
