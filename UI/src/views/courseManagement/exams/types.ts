export class ExamCreateModel {
    public examName: string | null = null;
    public examDate: Date | null = null;
    public startTime: Date | null = null;
    public endTime: Date | null = null;
    public idCourse: number | null = null;
}

export class ExamUpdateModel {
    public idExam: number | null = null;
    public examName: string | null = null;
    public examDate: Date | null = null;
    public startTime: Date | null = null;
    public endTime: Date | null = null;
}

export class ExamDetail {
    public courseName: string | null = null;
    public examName: string | null = null;
    public examDate: Date | null = null;
    public startTime: Date | null = null;
    public endTime: Date | null = null;
    public isEnabled: boolean | null = null;
}

export interface IExamDetail {
    idExam: number | null;
    examName: string | null;
    examDate: Date | null;
    startTime: Date | null;
    endTime: Date | null;
    isEnabled: boolean | null
}

export interface IExamState{
    exam: IExamDetail | null;
}
