export class CourseCreateModel {
    public name: string | null = null;
    public description: string | null = null;
    public startDate: Date | null = null;
    public endDate: Date | null = null;
    public idTeacher: number | null = null;
}

export class CourseUpdateModel {
    public idCourse: number | null = null;
    public name: string | null = null;
    public description: string | null = null;
    public startDate: Date | null = null;
    public endDate: Date | null = null;
}

export class CourseDetail {
    public teacherFullname: string | null = null;
    public name: string | null = null;
    public description: string | null = null;
    public isEnabled: boolean | null = null;
    public startDate: Date | null = null;
    public endDate: Date | null = null;
}

export class LessonCreateModel {
    public name: string | null = null;
    public description: string | null = null;
    public duration: number | null = null;
}

export class LessonUpdateModel {
    public name: string | null = null;
    public description: string | null = null;
    public duration: number | null = null;
}

export interface ICourseDetail {
    idCourse: number | null;
    name: string | null;
    description: string | null;
    isEnabled: boolean | null;
    startDate: Date | null;
    endDate: Date | null;
}

export interface ICourseState{
    course: ICourseDetail | null;
} 