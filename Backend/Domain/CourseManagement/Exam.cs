using System;
using System.Runtime.CompilerServices;
using System.Xml.Linq;
using Elfo.Contoso.LearningRoundKamran.Domain.CourseManagement.ValueObjects;
using Elfo.Contoso.LearningRoundKamran.Domain.SystemManagement.ValueObjects;
using Elfo.Round.WriteCycle;

namespace Elfo.Contoso.LearningRoundKamran.Domain.CourseManagement
{
    public class Exam : AggregateRoot<ExamID>
    {
        #region Constants
        private const short maxExamNameLength = 50;
        #endregion

        #region Ctor
        public Exam()
        {
            IsEnabled = true;
        }
        #endregion

        #region Properties
        public virtual string ExamName { get; set; }
        public virtual DateTimeOffset ExamDate { get; set; }
        public virtual DateTimeOffset StartTime { get; set; }
        public virtual DateTimeOffset EndTime { get; set; }
        public virtual bool IsEnabled { get; set; }

        public virtual CourseID IdCourse { get; set; }
        #endregion

        #region Methods
        public static Exam Create(CourseID idCourse, string examName, DateTimeOffset examDate,
            DateTimeOffset startTime, DateTimeOffset endTime)
        {
            var ex = Validate(examName, examDate, startTime, endTime);

            var exam = new Exam
            {
                IdCourse = idCourse,
                ExamName = examName,
                ExamDate = examDate,
                StartTime = startTime,
                EndTime = endTime
            };

            ex.CollectErrors(() =>
            {
                exam.Enable();
            }).TryThrow();

            return exam;

        }

        public virtual void Update(string examName, DateTimeOffset examDate, DateTimeOffset startTime,
            DateTimeOffset endTime)
        {
            var ex = Validate(examName, examDate, startTime, endTime);
            ex.TryThrow();

            ExamName = examName;
            ExamDate = examDate;
            StartTime = startTime;
            EndTime = endTime;
        }

        public virtual void Enable()
        {
            IsEnabled = true;
        }

        public virtual void Disable()
        {
            IsEnabled = false;
        }

        private static ValidationException Validate(string examName, DateTimeOffset examDate, DateTimeOffset startTime, DateTimeOffset endTime)
        {
            var ex = new ValidationException();

            if (string.IsNullOrWhiteSpace(examName))
                ex.AddError(nameof(ExamName), ValidationErrorCode.EmptyExamName);
            else if (examName.Length > maxExamNameLength)
                ex.AddError(nameof(ExamName), ValidationErrorCode.InvalidExamNameLength);
            if (DateTimeOffset.Compare(startTime, endTime) > 0)
                ex.AddError(nameof(StartTime), ValidationErrorCode.InvalidExamStartTime);

            return ex;
        }
        #endregion
    }
}
