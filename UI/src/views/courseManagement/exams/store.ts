import {IExamState, IExamDetail } from './types';

const state = {
	exam: null
} as IExamState;

const mutations = {
	setCurrentExam(examState: IExamState, exam: IExamDetail| null) {
		examState.exam = exam;
	}
};

export const ExamVuexModule = {
    state,
    mutations,
    namespaced: true
}