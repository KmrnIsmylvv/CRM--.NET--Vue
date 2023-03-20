using Elfo.Contoso.LearningRoundKamran.Domain.CourseManagement.ValueObjects;
using Elfo.Round.WriteCycle;

namespace Elfo.Contoso.LearningRoundKamran.Domain.CourseManagement
{
    public class Lesson : Entity<LessonID>
    {
        #region Properties
        public virtual string Name { get; set; }
        public virtual string Description { get; set; }
        public virtual int Duration { get; set; }

        public virtual CourseID IdCourse { get; set; }
        #endregion

        #region Methods
        public static Lesson Create(CourseID idCourse, string name, string description, int duration)
        {
            return new Lesson
            {
                IdCourse = idCourse,
                Name = name,
                Description = description,
                Duration = duration
            };
        }

        public virtual void Update(string name, string decscription, int duration)
        {
            Name = name;
            Description = decscription;
            Duration = duration;
        }
        #endregion
    }
}
